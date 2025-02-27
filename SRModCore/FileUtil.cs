using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SRModCore
{
    public class FileUtil
    {
        public static string GetSynthRidersUcDir()
        {
            // Check if we're running on Android
            var androidPath = "/sdcard/SynthRidersUC";
            if (Environment.OSVersion.Platform == PlatformID.Unix && Directory.Exists(androidPath))
            {
                return androidPath;
            }

            // Fallback on relative to game directory
            var pcPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "SynthRidersUC"));
            return pcPath;
        }
    }
}
