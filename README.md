# Halo Online Variant Editor
This open source editor allows extensive control over modifying game variants and partial control over map variants.

![Screenshot](https://i.imgur.com/V5WwDSX.png)

This editor allows access to the entire game variant file structure, including settings not normally present in the in-game interface. Some existing settings also have additional values added to them. If an existing setting has new values added, those are tagged with an `[UNSUPPORTED]` marker to identify that they are not present in the in-game interface but have been verified to still be supported by the engine itself.

While saving and loading variants, the editor will attempt to ensure file integrity by displaying numerous error or warning messages if it encounters unexpected, or known to be bad, values.

In addition to the editor being released, an 010 Editor Binary Template is also included to aid others in researching the variant file structure. The Binary Template contains the majority of the variant structure documented.

The editor fully supports command line arguments and being used as the default application for variant files. In addition to using it as a default application or dragging and dropping variant files onto the executable the following command line arguments (that are not case sensitive) can be used:

* `--MiniUpdater` will launch the editor in an Update-Only mode. This mode exposes only a few basic settings for editing and will prevent the editor from touching any other part of the variant file.
    * This is mainly useful for when Map Variants are going to be edited. It will prevent them from being broken when updated.
    DO NOT open Map Variants without this option used. When opening a Map Variant a prompt will still be displayed stating they are not supported. Click [NO] to continue loading the variant.
* `--NeverNewVariant` will launch the editor without the prompt asking if you want to create a new variant.
* `--Verbose` will enable additional dialogs to appear while performing actions such as saving and loading variants.

Creating a batch file to launch the editor in Updater mode is as simple as
```bat
set UpdateFile="%~1"
if %UpdateFile%=="" (set "UpdateFile=")
start "" "%~dp0Halo Online Variant Editor.exe" --MiniUpdater %UpdateFile%
```
