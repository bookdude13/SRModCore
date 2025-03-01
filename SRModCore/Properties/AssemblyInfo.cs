using MelonLoader;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("abcb1a59-660e-41b7-bcd8-f7f3ce0e45b5")]

[assembly: MelonInfo(typeof(SRModCore.MainMod), "SR Mod Core", "2.1.1", "bookdude13", "https://github.com/bookdude13/SRModCore")]
#if QUEST
[assembly: MelonGame("kluge", "SynthRiders")]
#else
[assembly: MelonGame("Kluge Interactive", "SynthRiders")]
#endif
