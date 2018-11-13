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

## Renaming the project
1. Rename the solution and the projects the normal way using your IDE.
2. Change the assembly name of each project (client, server, shared) to match the new name. Make sure the assembly name ends in `.net` (*important!* otherwise the server will not load the dll's).
    * **Each project's name needs to match the solution name, followed by Server/Client/Shared, otherwise building will fail!**
        * For example: If you rename project to `MyResource`, each project needs to be named `MyResourceClient`, `MyResourceServer`, `MyResourceShared`.
    * Depending on your IDE, the project folders might not be renamed, you'll have to do that manually.

## Automatically copying dependant assemblies to the resource folder
1. Open the .csproj file for the project that depends on an assembly.
2. Near the bottom, inside the PostBuild target:
    1. For each dependant assembly file, add this line:
        * \<OutputFiles Include="$(TargetPath)NameOfDll.dll" />
    * Wildcard selections work as well:
        * \<OutputFiles Include="$(TargetPath)deps\\*.dll />
    * Files are copied with their folders remaining intact. Meaning the above example would be copied to `resources\[dev]\BoilerplateResource\deps\`.

## Todo
* Easy way to remove an unneeded project (such as server and shared for a clientside only resource).
* Add a content folder which will be copied to the resource folder after building project.
