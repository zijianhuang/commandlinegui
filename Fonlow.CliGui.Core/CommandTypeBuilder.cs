using System;
using System.Reflection;
using System.ComponentModel;
using Plossum.CommandLine;
using System.Reflection.Emit;
using System.Linq;
using Fonlow.CommandLine;

namespace Fonlow.CommandLineGui
{
    internal static class CommandTypeBuilder
    {
        /// <summary>
        /// Merge optionsType into parametersType to create a new type with both, and translate CommandLineOptionAttribute to BCL attributes.
        /// </summary>
        /// <param name="optionsType">For options.</param>
        /// <returns>New type</returns>
        /// <remarks>Actually CLR seems to allow multiple dyanmic assemblies having the same name since the dynamic types are generated and instantiated through reflection,
        /// however to make them look beautiful in Process Explorer, they are given different names.</remarks>
        internal static Type CreateProxyType(Type optionsType)
        {
            var namespaceSuffix = GetNamespaceSuffix(optionsType.Namespace);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Fonlow.CommandLineGui.DynamicTypes" + namespaceSuffix), AssemblyBuilderAccess.Run);//build assembly at runtime.
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("Fonlow.CommandLineGui.DynamicModule");//build module of the assembly
            TypeBuilder typeBuilder = moduleBuilder.DefineType("Fonlow.CommandLineGui.DynamicTypes."+optionsType.Name+"Proxy",// to build a new type with suffix Proxy
                TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.BeforeFieldInit);

            var propertiesOfOptions = optionsType.GetProperties();
            var fieldsOfOptions = optionsType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            var groupNamesDic = PlossumAttributesHelper.GetCommandLineOptionGroupAttributesDic(optionsType);

            typeBuilder.SetParent(typeof(object));//parametersType has to be the parent since its attributes don't need to be changed.

            CopyCommandLineOptionGroupAttributes(typeBuilder, optionsType);
            CopyCommandLineManagerAttribute(typeBuilder, PlossumAttributesHelper.GetCommandLineManagerAttribute(optionsType));

            foreach (var fieldInfo in fieldsOfOptions)
            {
                FieldBuilder fieldBuilder = typeBuilder.DefineField(fieldInfo.Name, fieldInfo.FieldType, fieldInfo.Attributes);
            }

            foreach (var propertyItem in propertiesOfOptions)
            {
                var propertyBuilder = typeBuilder.DefineProperty(propertyItem.Name, PropertyAttributes.None, propertyItem.PropertyType, null);

                FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyItem.Name.ToLower(), propertyItem.PropertyType, FieldAttributes.Public);

                // copy the "get" accessor
                MethodInfo getterInfo = propertyItem.GetGetMethod();
                var parametersTypes = getterInfo.GetParameters().Select(d => d.ParameterType).ToArray(); 
                MethodBuilder getPropMthdBldr = typeBuilder.DefineMethod(getterInfo.Name, getterInfo.Attributes, getterInfo.CallingConvention, getterInfo.ReturnType, Type.EmptyTypes);
                getPropMthdBldr.DefineParameter(0, ParameterAttributes.Retval, null);

                ILGenerator propertyGetIL = getPropMthdBldr.GetILGenerator();
                propertyGetIL.Emit(OpCodes.Ldarg_0);
                propertyGetIL.Emit(OpCodes.Ldfld, fieldBuilder);
                propertyGetIL.Emit(OpCodes.Ret);
                
                propertyBuilder.SetGetMethod(getPropMthdBldr);

                // copy the "set" accessor
                MethodInfo setterInfo = propertyItem.GetSetMethod();
                var setterParameters= setterInfo.GetParameters();
                parametersTypes = setterParameters.Select(d => d.ParameterType).ToArray();
                MethodBuilder setPropMthdBldr = typeBuilder.DefineMethod(setterInfo.Name, setterInfo.Attributes, setterInfo.CallingConvention, setterInfo.ReturnType, parametersTypes);// new Type[] { propertyItem.PropertyType });
                setPropMthdBldr.DefineParameter(1, ParameterAttributes.In, setterParameters[0].Name);
                setPropMthdBldr.DefineParameter(0, ParameterAttributes.Retval, null);

                ILGenerator propertySetIL = setPropMthdBldr.GetILGenerator();
                propertySetIL.Emit(OpCodes.Ldarg_0);
                propertySetIL.Emit(OpCodes.Ldarg_1);
                propertySetIL.Emit(OpCodes.Stfld, fieldBuilder);
                propertySetIL.Emit(OpCodes.Ret);
             
                propertyBuilder.SetSetMethod(setPropMthdBldr);

                var optionAttribute = PlossumAttributesHelper.GetCommandLineOptionAttribute(propertyItem);
                if (optionAttribute == null)//for fixed parameters
                {
                    var fixedParameterAttribute = PropertyHelper.ReadAttribute<FixedParameterAttribute>(propertyItem);

                    if (fixedParameterAttribute != null)
                    {
                        CreateCustomAttributeWith1ParameterCtorOfProperty(propertyBuilder, typeof(DisplayNameAttribute), typeof(string), fixedParameterAttribute.DisplayName);
                        CreateCustomAttributeWith1ParameterCtorOfProperty(propertyBuilder, typeof(DescriptionAttribute), typeof(string), fixedParameterAttribute.Description);
                        CreateCustomAttributeWith1ParameterCtorOfProperty(propertyBuilder, typeof(CategoryAttribute), typeof(string), fixedParameterAttribute.Category);
                        CreateCustomAttributeWith1ParameterCtorOfProperty(propertyBuilder, typeof(ParameterOrderAttribute), typeof(int), fixedParameterAttribute.Order);

                        if (fixedParameterAttribute.DefaultValue != null)
                        {
                            CreateCustomAttributeWith1ParameterCtorOfProperty(propertyBuilder, typeof(DefaultValueAttribute), propertyItem.PropertyType, fixedParameterAttribute.DefaultValue);
                        }
                    }

                    var editorAttribute = ReadEditorAttribute(propertyItem);
                    if (editorAttribute != null)
                    {
                        CreateEditorAttributeOfProperty(propertyBuilder, Type.GetType(editorAttribute.EditorTypeName));
                    }
                }
                else
                {
                    #region Create PropertyGrid friendly attributes

                    if (optionAttribute.DefaultAssignmentValue != null)
                    {
                        CreateCustomAttributeWith1ParameterCtorOfProperty(propertyBuilder, typeof(DefaultValueAttribute), propertyItem.PropertyType, optionAttribute.DefaultAssignmentValue);
                    }

                    string displayName = optionAttribute.Name;
                    if (String.IsNullOrEmpty(displayName))
                    {
                        if (String.IsNullOrEmpty(displayName))
                        {
                            displayName = propertyItem.Name;
                            optionAttribute.Name = displayName;
                        }
                    }

                    string category = null;

                    if (optionAttribute.GroupId != null)
                    {
                        Plossum.CommandLine.CommandLineOptionGroupAttribute groupAttribute;
                        bool foundGroupAttribute = groupNamesDic.TryGetValue(optionAttribute.GroupId, out groupAttribute);
                        if (foundGroupAttribute)
                        {
                            category = String.IsNullOrEmpty(groupAttribute.Name) ? groupAttribute.Id : groupAttribute.Name;
                        }
                    }

                    CreateCustomAttributeWith1ParameterCtorOfProperty(propertyBuilder, typeof(DisplayNameAttribute), typeof(string), displayName);
                    if (!String.IsNullOrEmpty(optionAttribute.Description))
                    {
                        CreateCustomAttributeWith1ParameterCtorOfProperty(propertyBuilder, typeof(DescriptionAttribute), typeof(string), optionAttribute.Description);
                    }

                    if (category != null)
                    {
                        CreateCustomAttributeWith1ParameterCtorOfProperty(propertyBuilder, typeof(CategoryAttribute), typeof(string), category);
                    }


                    if (optionAttribute.DefaultAssignmentValue != null)
                    {
                        CreateCustomAttributeWith1ParameterCtorOfProperty(propertyBuilder, typeof(DefaultValueAttribute), propertyItem.PropertyType, optionAttribute.DefaultAssignmentValue);
                    }
                    else
                    {
                        var defaultValueOfType = PropertyHelper.GetDefaultValueOfType(propertyItem.PropertyType);
                        if (defaultValueOfType != null)
                        {
                            CreateCustomAttributeWith1ParameterCtorOfProperty(propertyBuilder, typeof(DefaultValueAttribute), propertyItem.PropertyType, defaultValueOfType);
                        }
                    }


                    var editorAttribute = ReadEditorAttribute(propertyItem);
                    if (editorAttribute != null)
                    {
                        CreateEditorAttributeOfProperty(propertyBuilder, Type.GetType(editorAttribute.EditorTypeName));
                    }

                    CopyCommandLineOptionAttributeOfProperty(propertyBuilder, optionAttribute);


                    #endregion
                }

            }

            return typeBuilder.CreateType();
        }

        static string GetNamespaceSuffix(string s)
        {
            int i = s.LastIndexOf('.');
            if (i == -1)
                return "." + s;
            else
                return s.Substring(i);
        }


        static void CreateCustomAttributeWith1ParameterCtorOfProperty(PropertyBuilder propertyBuilder, Type typeOfAttribute, Type typeOfCtorParameter, object valueOfCtorParameter)
        {
            var classCtorInfo = typeOfAttribute.GetConstructor(new Type[] { typeOfCtorParameter });
            CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(classCtorInfo, new object[] { valueOfCtorParameter });
            propertyBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        static void CreateEditorAttributeOfProperty(PropertyBuilder propertyBuilder, Type typeOfCustomEditor)
        {
            Type typeOfEditorAttribute = typeof(EditorAttribute);

            var classCtorInfo = typeOfEditorAttribute.GetConstructor(new Type[] { typeof(Type), typeof(Type) });
            CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(classCtorInfo, new object[] { typeOfCustomEditor, typeof(System.Drawing.Design.UITypeEditor) });
            propertyBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        static EditorAttribute ReadEditorAttribute(MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            object[] objects = memberInfo.GetCustomAttributes(typeof(EditorAttribute), false);
            if (objects.Length == 1)
            {
                return (objects[0] as EditorAttribute);
            }
            return null;
        }

        static void CopyCommandLineOptionAttributeOfProperty(PropertyBuilder propertyBuilder, CommandLineOptionAttribute attribute)
        {
            Type typeOfCommandLineOptionAttribute = typeof(CommandLineOptionAttribute);
            var classCtorInfo = typeOfCommandLineOptionAttribute.GetConstructor(new Type[] { });

            var propertyInfoArrayOfAttribute = GetNamedPropertyInfoArray(attribute);
            var valuesOfproperties = propertyInfoArrayOfAttribute.Select(item => item.GetValue(attribute, null)).ToArray();

            CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(classCtorInfo, new object[] { }, propertyInfoArrayOfAttribute, valuesOfproperties);
            propertyBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        static void CopyCommandLineOptionGroupAttributes(TypeBuilder typeBuilder, Type optionsType)
        {
            Type typeOfCommandLineOptionGroupAttribute = typeof(CommandLineOptionGroupAttribute);
            var classCtorInfo = typeOfCommandLineOptionGroupAttribute.GetConstructor(new Type[] { typeof(string) });

            var commandLineOptionGroupAttribute = optionsType.GetCustomAttributes(typeOfCommandLineOptionGroupAttribute, false).Cast<CommandLineOptionGroupAttribute>();
            foreach (var attribute in commandLineOptionGroupAttribute)
            {
                var propertyInfoArrayOfAttribute = GetNamedPropertyInfoArray(attribute);
                var valuesOfproperties = propertyInfoArrayOfAttribute.Select(item => item.GetValue(attribute, null)).ToArray();

                CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(classCtorInfo, new object[] { attribute.Id }, propertyInfoArrayOfAttribute, valuesOfproperties);
                typeBuilder.SetCustomAttribute(customAttributeBuilder);
            }
        }

        const BindingFlags namedPropertiesBindingFlags = BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.DeclaredOnly | BindingFlags.Instance;

        static void CopyCommandLineManagerAttribute(TypeBuilder typeBuilder, CommandLineManagerAttribute attribute)
        {
            Type type = typeof(CommandLineManagerAttribute);
            var classCtorInfo = type.GetConstructor(new Type[] { });
            var propertyInfoArrayOfAttribute = GetNamedPropertyInfoArray(attribute);
            var valuesOfproperties = propertyInfoArrayOfAttribute.Select(item => item.GetValue(attribute, null)).ToArray();

            CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(classCtorInfo, new object[] { }, propertyInfoArrayOfAttribute, valuesOfproperties);

            typeBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        static PropertyInfo[] GetNamedPropertyInfoArray(Attribute attribute)
        {
            return attribute.GetType().GetProperties(namedPropertiesBindingFlags).Where(prop => prop.GetSetMethod() != null).ToArray();
        }

    }
}
