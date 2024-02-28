# ReturnOS<br><sup>An OS thats pure command-line and works on a return!</sup>

## Reason?
- I was exhausted working on Lynox so it's my side-project.
- I really love the command-line.
- Really wanna try new cosmos stuff. (Installers, executors, programming languages, etc)

## Building?
### Windows:
- Use Visual Studio 2022 with Cosmos DevKit installed properly and build it with the project cloned.
- Run project with VMWare.

### Linux:
- Use either the [CosmosCLI](https://github.com/PratyushKing/cosmosCLI) tool or use the regular `dotnet build` commands.
- Use qemu to run the project and yes it will work with filesystems as ReturnOS uses [LVFS](Docs/LVFS%20Implementation.md) for fallback. (Alternatively do the command `cosmos -r` if CosmosCLI is being used)