Command Line GUI project includes a command line arguments parser library for constructing .NET Framework command line programs with complex arguments.

You define a data model class decorated with some attributes for represeting arguments. The core library will parse the commaline arguments and populate the Plain Old CLR Object which is the router of the functionality of your command line program. And the Core library may also generates CLI help page.

Command Line GUI exe is a host program that generates GUI for existing command line programs. .NET command line programs constructed through the core library could become a plugin of Command Line GUI and have the GUI generated.

You may use the core library to wrap existing command line programs like Robocopy, and the wrapper may become a plugin, thus a native command line program could easily have a GUI. This project includes a primary plugin for Robocopy, which reassembles Better Robocopy GUI.


## Features

* Generate GUI controls for editing parameters and options.
* Provide a text editor to edit command line arguments directly, and reflect changes back to the property grid, and vice-versa.
* Provide immediate hints to an option highlighted in the property grid or in the text editor.
* Support rules of inclusion, exclusion and combination among options.
* Command line programs utilizing Plossum library may get GUI without the need for further programming.

## References

* [Get the Best of Both Worlds: Command Line and GUI](https://www.codeproject.com/Articles/649950/Get-the-Best-of-Both-Worlds-Command-Line-and-GUI)




**Remarks**

* Command Line GUI was hosted at https://sourceforge.net/projects/commandlinegui/ since 2010, and is moved to https://github.com/zijianhuang/commandlinegui in 2018.


## Maintainer Notices
The project had started on .NET Framework 4, and the latest targets .NET Framework 4.8 as of 2023-07-29. And a .NET (Core) migration is on the way.



