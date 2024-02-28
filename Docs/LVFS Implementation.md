# LVFS Implementation<br><sup>Literally Virtual File System Implementation

## How it works:
You get a working filesystem thats only loaded into RAM. It can be used anytime easily and it maps files as a Dictionary. The dictionary stores the file contents and file data both as strings. LVFS is not `static` and can be used to make multiple filesystems. It can behave as a container, and any OS can boot of it if handled correctly.<br><br>
On ReturnOS LVFS while it is mostly used as a fallback, a lot of applications can use it as their own config manager, for example, if theres a virtual machine implementation sometime, it has a CLI and the CLI use a LVFS to define virtual machines.

### ⚠️ CAUTION: LVFS CAN contain the base OS files to emulate the kernel but it is not required and if used by applications, the application can embed their own files or combine the kernel's file and their own files.
