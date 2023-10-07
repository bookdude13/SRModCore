set MOD_NAME="SRModCore"
set SYNTHRIDERS_MODS_DIR="C:\Program Files (x86)\Steam\steamapps\common\SynthRiders\Mods"

echo "Building dev configuration"
python.exe build.py --clean -n %MOD_NAME% -o build/localdev localdev

echo "Copying to SR directory..."
@REM Building spits out raw file structure in build/localdev/raw
copy build\localdev\raw\* %SYNTHRIDERS_MODS_DIR%

echo "Done"
