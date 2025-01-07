import argparse
import json
import os
import shutil
import subprocess
import tempfile
from pathlib import Path
from zipfile import ZipFile


def dotnet_clean(solution_file, configuration):
    result = subprocess.run(["dotnet", "clean", solution_file, "-c", configuration])
    return result.returncode == 0


def dotnet_publish_solution(solution_file, configuration):
    result = subprocess.run(["dotnet", "publish", solution_file, "-c", configuration])
    return result.returncode == 0


def create_localitem_json(output_dir):
    local_item_contents = {
        "hash": mod_name
    }
    local_item_json = json.dumps(local_item_contents, indent=2)
    local_item_path = Path(output_dir, "LocalItem.json")
    with open(local_item_path, "w") as local_item_file:
        local_item_file.write(local_item_json)
    
    return local_item_path


def create_synthmod(mod_name, input_dir, input_subpaths, output_dir, output_name):
    mod_name = str(mod_name)

    tmpdir = tempfile.mkdtemp()
    print(f"tmp directory is {tmpdir}")

    print("Setting up output dir")
    tmp_outputdir = Path(tmpdir, mod_name)
    tmp_outputdir.mkdir(parents=True, exist_ok=True)

    tmp_output_path = Path(tmp_outputdir, output_name)

    print("Creating LocalItem.json")
    localitem_path = create_localitem_json(tmp_outputdir)

    print(f"Creating zipped synthmod file {tmp_output_path}")
    with ZipFile(tmp_output_path, "w") as zip_file:
        print(f"Adding {localitem_path} to LocalItem.json")
        zip_file.write(localitem_path, "LocalItem.json")

        add_files_to_zip(zip_file, input_dir, input_subpaths, "Mods")

    if not tmp_output_path.exists():
        print(f"Failed to create zip file {tmp_output_path}")
        return False
    
    final_path = Path(output_dir, output_name)
    final_path.parent.mkdir(parents=True, exist_ok=True)

    print(f"Copying tmp synthmod file to {final_path}")
    shutil.copyfile(tmp_output_path, final_path)

    print(f"synthmod file created at {final_path}")
    return True


def copy_with_dir_create(input_dir, input_file, output_dir):
    full_path = Path(input_dir, input_file)
    output_path = Path(output_dir, input_file)
    output_path.parent.mkdir(parents=True, exist_ok=True)
    print(f"Copying {full_path} to {output_path}")
    shutil.copyfile(full_path, output_path)


def copy_raw(mod_name, input_dir, input_subpaths, output_dir):
    mod_name = str(mod_name)

    for subpath in input_subpaths:
        copy_with_dir_create(input_dir, subpath, output_dir)

    # Include dependencies as well (single level)
    dep_dir = Path(input_dir, "libs")
    if dep_dir.exists():
        for dep_file in dep_dir.iterdir():
            copy_with_dir_create(input_dir, dep_file, output_dir)

    return True


def add_files_to_zip(zip_file, input_dir, input_subpaths, zip_dir = "Mods"):
    for input_file in input_subpaths:
        add_file_to_zip(zip_file, input_dir, input_file, zip_dir)


def add_file_to_zip(zip_file, input_dir, input_file, zip_dir = "Mods"):
    real_path = Path(input_dir, input_file)
    zip_path = Path(zip_dir, input_file)
    print(f"Adding {real_path} to {zip_path}")
    zip_file.write(real_path, zip_path)


def create_zip(mod_name, input_dir, input_subpaths, output_dir, output_name):
    mod_name = str(mod_name)

    tmpdir = tempfile.mkdtemp()
    print(f"tmp directory is {tmpdir}")

    print("Setting up output dir")
    # root_output_dir = Path("..", "Downloads")
    tmp_outputdir = Path(tmpdir, mod_name)
    tmp_outputdir.mkdir(parents=True, exist_ok=True)

    tmp_output_path = Path(tmp_outputdir, output_name)
    print(f"Creating zip file {tmp_output_path}")
    with ZipFile(tmp_output_path, "w") as zip_file:
        add_files_to_zip(zip_file, input_dir, input_subpaths, "Mods")

        # Include dependencies as well (single level)
        dep_dir = Path(input_dir, "libs")
        if dep_dir.exists():
            for dep_file in dep_dir.iterdir():
                add_file_to_zip(zip_file, dep_dir, dep_file.name, "Mods")

    if not tmp_output_path.exists():
        print(f"Failed to create zip file {tmp_output_path}")
        return False
    
    final_path = Path(output_dir, output_name)
    final_path.parent.mkdir(parents=True, exist_ok=True)
    print(f"Copying tmp zip file to {final_path}")
    shutil.copyfile(tmp_output_path, final_path)

    print(f"zip file created at {final_path}")
    return True


def add_git_tag(mod_name, version):
    print(f"Adding git tag v{version}")
    result = subprocess.run([
        "git", "tag",
        "-a",
        "-m", f"{mod_name} v{version}",
        f"v{version}"
    ])

    print("Remember to push up with `git push --tags`")
    return result.returncode == 0


def clean_output(output_dir):
    if not Path.exists(output_dir):
        return True

    print(f"Cleaning directory {output_dir}")
    shutil.rmtree(output_dir)
    return True


if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        prog='build.py',
        description='Helper functions for building dev and release files (synthmod, zip)')
    parser.add_argument(
        "version",
        help="Mod version being built. Ex. 1.3.0"
    )
    parser.add_argument(
        "input_file",
        type=Path,
        help="File containing list of file names to include in copy/zip/synthmod, one per line, relative from the input directory"
    )
    parser.add_argument(
        "-n",
        "--name",
        help="Mod name. Defaults to current directory name."
    )
    parser.add_argument(
        "-c",
        "--configuration",
        default="Release",
        help="Build configuration, Debug or Release. Defaults to Release"
    )
    parser.add_argument(
        "-o",
        "--output-dir",
        type=Path,
        help="Output directory. Defaults to build/<ModName>_<Version>"
    )
    parser.add_argument(
        "-i",
        "--input-dir",
        type=Path,
        help="Input directory (build directory). Defaults to <ModName>/bin/<Configuration>/net6.0/publish"
    )
    parser.add_argument(
        "--tag",
        action="store_true",
        help="If specified, tags the version and pushes tag to repo"
    )
    parser.add_argument(
        "--clean",
        action="store_true",
        help="If specified, removes the output directory for this particular version before building"
    )
    
    args = parser.parse_args()

    configuration = args.configuration

    version = str(args.version)

    mod_name = args.name
    if not mod_name:
        mod_name = Path(".").resolve().stem
        print(f"Mod name not specified; using '{mod_name}'")

    input_dir = args.input_dir
    if not input_dir:
        input_dir = Path(".", mod_name, "bin", configuration, "net6.0", "publish").resolve()
        print(f"Input dir not specified; using '{input_dir}'")

    output_dir = args.output_dir
    if not output_dir:
        output_dir = Path("build", f"{mod_name}_{version}")
        print(f"Output dir not specified; using {output_dir}")
    
    input_file_definitions = args.input_file
    input_files = [line.rstrip() for line in open(input_file_definitions, "r")]

    # Always build the whole solution to resolve dependencies correctly
    # This can become a parameter if it ever differs.
    # Try both the current directory and one directory lower (should be the common configurations)
    solution_file = Path(f"{mod_name}.sln")
    if not solution_file.exists():
        solution_file = Path(mod_name, f"{mod_name}.sln")
    print("Using solution file", solution_file.resolve())

    zip_output_file = f"{mod_name}_{version}.zip"
    synthmod_output_file = f"{mod_name}_{version}.synthmod"

    commands = [
        lambda: clean_output(output_dir) if args.clean else True,
        lambda: dotnet_clean(solution_file.resolve(), configuration),
        lambda: dotnet_publish_solution(solution_file.resolve(), configuration),
        lambda: copy_raw(mod_name, input_dir, input_files, Path(output_dir, "Mods")),
        lambda: create_zip(mod_name, input_dir, input_files, output_dir, zip_output_file),
        lambda: create_synthmod(mod_name, input_dir, input_files, output_dir, synthmod_output_file),
        lambda: add_git_tag(mod_name, version) if args.tag else True,
    ]

    for cmd in commands:
        if not cmd():
            print("Error, stopping")
            exit(-1)
        print()

