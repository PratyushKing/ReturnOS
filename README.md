# ReturnOS<br><sup>An OS thats purely fast and stable and works on a press of a return!</sup>

## Reason?
- I was exhausted working on Lynox so it's my side-project.
- I really love the command-line.
- Really wanna try new cosmos stuff. (Installers, executors, programming languages, etc)

## TODO LIST:
(0 = Not done, 1 = Done)<br>
[ 1 ] Basic Kernel with init system<br>
[ 0 ] LVFS Implementation
[ 0 ] Proper Process Management<br>
[ 0 ] A really good CLI base<br>
[ 0 ] Proper functional execution<br>
[ 0 ] Config parsing<br>
[ 0 ] Full-blown Apps (CLI)<br>

## Building?
### Windows:
- Use Visual Studio 2022 with Cosmos DevKit installed properly and build it with the project cloned.
- Run project with VMWare.

### Linux:
- Use either the [CosmosCLI](https://github.com/PratyushKing/cosmosCLI) tool or use the regular `dotnet build` commands.
- Use qemu to run the project and yes it will work with filesystems as ReturnOS uses [LVFS](Docs/LVFS%20Implementation.md) for fallback. (Alternatively do the command `cosmos -r` if CosmosCLI is being used)