using Microsoft.Win32;

namespace PawnIO
{
    public static class CpuInfo
    {
        public static readonly bool IsAMD = DetectAMD();

        private static bool DetectAMD()
        {
            // .NET Framework 4.8 has no System.Runtime.Intrinsics.X86 API.
            // The processor name is already available from the standard Windows registry.
            return Name.IndexOf("AMD", StringComparison.OrdinalIgnoreCase) >= 0;
        }


        private const string CpuRegKey = @"HARDWARE\DESCRIPTION\System\CentralProcessor\0";

        private static readonly Lazy<(string Name, string Caption)> _data =
            new Lazy<(string, string)>(Load, LazyThreadSafetyMode.ExecutionAndPublication);

        public static string Name    => _data.Value.Name;
        public static string Caption => _data.Value.Caption;

        // CPU undervolting / temperature limits.
        // Defaults match the original RyzenControl values; can be overridden via AppConfig.
        public static int MinCPUUV   => AppConfig.Get("min_uv",      -40);
        public static int MaxCPUUV   => AppConfig.Get("max_uv",        0);
        public static int MinIGPUUV  => AppConfig.Get("min_igpu_uv", -30);
        public static int MaxIGPUUV  => AppConfig.Get("max_igpu_uv",   0);
        public static int MinTemp    => AppConfig.Get("min_temp",     75);
        public static int DefaultTemp=> AppConfig.Get("max_temp",     96);

        public static bool IsSupportedUV()
            => Name.Contains("RYZEN AI MAX") ||
               Name.Contains("Ryzen AI 9")   ||
               Name.Contains("Ryzen 9")      ||
               Name.Contains("4900H")        ||
               Name.Contains("4800H")        ||
               Name.Contains("4600H");

        public static bool IsSupportedUViGPU() => Name.Contains("6900H");

        private static (string Name, string Caption) Load()
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(CpuRegKey);
                if (key is not null)
                {
                    var name = key.GetValue("ProcessorNameString")?.ToString()?.Trim() ?? string.Empty;
                    var caption = key.GetValue("Identifier")?.ToString()?.Trim() ?? string.Empty;
                    return (name, caption);
                }
            }
            catch { }

            return (string.Empty, string.Empty);
        }
    }
}
