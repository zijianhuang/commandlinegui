cd %~dp0
nuget.exe pack Core.csproj -Prop Configuration=Release -IncludeReferencedProjects
pause