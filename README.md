# Boilerplate C# resource project for FiveM (CitizenFX)

## Why?
Setting up a development environment for every new project is a tedious process in FiveM when using C#. This project aims to remove the initial hurdle so you can start coding on your resource instantly.

## Initial setup
1. Copy the DevTarget.target.example file to DevTarget.target
2. Open the file and replace the values.
    * The FiveMDirectory value should point to the directory that contains the `FiveM.app` (potentially aliased as `FiveM Application Data`) folder. This is probably in %localappdata%\FiveM if you're unsure.
    * The ServerDirectory value should point to the root of your server directory. [Make sure you've installed your server properly](https://docs.fivem.net/server-manual/setting-up-a-server/), or building will probably fail when copying the resource files to the resources folder.
    * **The values should not end with a directory separator! Doing so might make the build fail.**
3. When building the project, the resource will be copied to the following folder: `[FiveM_Server]\server-data\[dev]\[NameOfSolution]\`. A `__resource.lua` file will be generated automatically.

## Renaming the projects
1. Rename the solution and the projects the normal way using your IDE.
2. Change the assembly name of each project (client, server, shared) to match the new name. Make sure the assembly name ends in `.net` (*important!* otherwise the server will not load the dll's).
    * Depending on your IDE, the project folders might not be renamed, you'll have to do that manually.

## Remove a project
1. Delete the unnecessary project from the solution.
2. Open `post_build.bat` or `post_build.sh` depending on your OS.
3. Remove the path to the deleted project from the --assemblies commandline argument.
4. Optionally, if you have built the deleted project, go to the resource in your server folder and delete the assembly file there too.

## Add a new project
1. Create a new class library targeting .net 4.5.2.
2. Make sure the platform target is set to AnyCPU on all build configurations (Debug, Release).
3. Append `.net` to the assembly name.
    * For example, if the project is called `MyResource`, change the assembly name to `MyResource.net`.
4. Add a reference to the ManifestGenerator project.
5. Add `[assembly: AssemblyType(AssemblyType.Client/Server/Shared)]` to the AssemblyInfo.cs file. It will decide where the script ends up in the resource.lua file.
4. Open up the project's `.csproj` file and add the following:
```xml
<!-- Clientside -->
<Reference Include="CitizenFX.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null">
    <HintPath>$(FiveMDirectory)\FiveM.app\citizen\clr2\lib\mono\4.5\CitizenFX.Core.dll</HintPath>
</Reference>

<!-- OR -->

<!-- Serverside or shared -->
<Reference Include="CitizenFX.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null">
    <HintPath>$(ServerDirectory)\citizen\clr2\lib\mono\4.5\CitizenFX.Core.dll</HintPath>
</Reference>

<!-- Add the following to the bottom, right before </Project> -->
<Import Project="$(SolutionDir)DevVars.targets" />
```
* Look at the other project's `.csproj` files to set up automatically copying files to the server's resource folder.

## Automatically copying dependant assemblies to the resource folder
1. Open the .csproj file for the project that depends on an assembly.
2. Near the bottom, inside the PostBuild target:
    1. For each dependant assembly file, add this line:
        * \<OutputFiles Include="$(TargetPath)NameOfDll.dll" />
    * Wildcard selections work as well:
        * \<OutputFiles Include="$(TargetPath)deps\\*.dll />
    * Files are copied with their folders remaining intact. Meaning the above example would be copied to `resources\[dev]\BoilerplateResource\deps\`.

## Todo
* Add a content folder which will be copied to the resource folder after building project.