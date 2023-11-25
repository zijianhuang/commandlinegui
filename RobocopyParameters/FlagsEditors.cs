using Fonlow.CommandLineGui.Gui;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System;

namespace Fonlow.CommandLineGui.Robocopy
{

	public class FlagsEditor<T, TEnumType> : UITypeEditor where T : FlagsUC<TEnumType>, new()
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (value == null)
				throw new ArgumentNullException("value");

			if (value.GetType() != typeof(TEnumType))
				return value;

			// Uses the IWindowsFormsEditorService to display a 
			// drop-down UI in the Properties window.
			using (var flagsUc = (new FlagsUC<TEnumType>((TEnumType)value)) as T)
			{
				IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (edSvc != null)
				{
					edSvc.DropDownControl(flagsUc);
					int flags = flagsUc.Flags;
					return flags;
				}
			}
			return value;

		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

	}

	public sealed class CopyFlagsEditor : FlagsEditor<FlagsUC<CopyFlags>, CopyFlags>
	{
	}

	public sealed class RashFlagsEditor : FlagsEditor<FlagsUC<Rashcneto>, Rashcneto>
	{
	}
}
