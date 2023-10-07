# SRModCore
Core shared functions to make modding Synth Riders easier

## For Developers - Adding to a project

### Method 1 - Submodule
- Add SRModCore to your project as a Git submodule: `git submodule add git@github.com:bookdude13/SRModCore`
- Make sure the branch you want is checked out
- Add the SRModCore .csproj file to your solution
- Add a project reference to SRModCore from your mod's project
- Include the built SRModCore.dll with your mod's release, or manage the dependency in some other way
  - It should be in the project's build output by default if your project reference/project dependency is set from the above steps

For building (see SRVoting as an example):
- Copy the build_and_copy_dev.bat, build_tag_release.bat and build_files.txt files from SRModCore and put them in your project's root directory
- For build_tag_release.bat:
  - Update `MOD_NAME` to be the name of your mod
  - Update `BUILD_SCRIPT` to point to the build.py file (it probably should be `SRModCore\build.py`)
- For build_and_copy_dev.bat:
  - Update `MOD_NAME` to be the name of your mod
  - Update `BUILD_SCRIPT` to point to the build.py file (it probably should be `SRModCore\build.py`)
  - Update `SYNTHRIDERS_MODS_DIR` to be the path to your SynthRiders installation (currently set to the default Windows install path)
- For build_files.txt:
  - Each line should be the files included in your mod zip/synthmod packages
  - Include any dependencies (like `SRModCore.dll`) here as well
  - Paths are relative to the input directory given to build.py (default `./<ModName>/bin/<Configuration>/net6.0/publish`)

### Method 2 - Download + reference
- Download the latest version from the Releases tab
- Right click References in your project, and Add Reference. Browse to the dll you downloaded (you'll need to unzip).
- Include the SRModCore.dll file in your mod release
- Figure out your own build/release strategy

See SRPerformanceMeter, SRVoting and SRPlaylistDownloader for example usages. Note that after adding the .dll to the Visual Studio project, I changed the file's properties to "Copy Always", which is why the scripts work.

---

## Disclaimer
This mod is not affiliated with Synth Riders or Kluge Interactive. Use at your own risk.

