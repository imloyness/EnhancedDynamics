using System;
using UnityEngine;

namespace EnhancedDynamics
{
    public static class GameDetector
    {
        public static string GameName { get; private set; } = "Unknown";
        public static string GameVersion { get; private set; } = "Unknown";
        public static string CompanyName { get; private set; } = "Unknown";
        public static string DataPath { get; private set; } = string.Empty;
        public static bool IsDetected { get; private set; } = false;

        public static string whatbbgame_ED = "idk";

        public static void DetectGame()
        {
            try
            {
                GameName = Application.productName ?? "Unknown";
                GameVersion = Application.version ?? "Unknown";
                CompanyName = Application.companyName ?? "Unknown";
                DataPath = Application.dataPath ?? string.Empty;
                IsDetected = true;

                if (!string.IsNullOrEmpty(GameName))
                {
                    if (GameName.IndexOf("Plus", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        whatbbgame_ED = "bbp";
                    }
                    else if (GameName.IndexOf("Classic Remastered", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        whatbbgame_ED = "bbieal";
                    }
                    else if (GameName.IndexOf("In Education and Learning", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        whatbbgame_ED = "bbc";
                    }
                    else
                    {
                        whatbbgame_ED = "idk";
                    }
                }
                else
                {
                    whatbbgame_ED = "idk";
                }

                BasePlugin.logsblablabla?.LogInfo($"Enhanced Dynamics | {GameName} v{GameVersion} (whatbbgame_ED={whatbbgame_ED})");
            }
            catch (System.Exception e)
            {
                BasePlugin.logsblablabla?.LogWarning("Enhanced Dynamics | Game detection failed: " + e.Message);
                GameName = "Unknown";
                GameVersion = "Unknown";
                CompanyName = "Unknown";
                DataPath = string.Empty;
                IsDetected = false;
            }
        }
    }
}
