using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife.Patches
{
    [HarmonyPatch( typeof( uConsole ), "Start" )]
    public class Patch_uConsole_Start
    {
        static Color OriginalColorBG;
        static Color OriginalColorFG;
        static Color OriginalColorBGInput;
        static Color OriginalColorFGInput;

        static void Postfix( uConsole __instance )
        {
            OriginalColorBG = __instance.m_LogBackGroundColor;
            OriginalColorFG = __instance.m_LogFontColor;
            OriginalColorBGInput = __instance.m_InputFieldBackGroundColor;
            OriginalColorFGInput = __instance.m_InputFieldFontColor;

            UpdateConsoleColor();
        }

        public static void UpdateConsoleColor()
        {
            uConsole Console = uConsole.m_Instance;
            if ( Console != null )
            {
                if ( Settings.Instance.EnableMod && Settings.Instance.ConsoleDarkMode )
                {
                    Console.m_InputFieldBackGroundColor = Console.m_LogBackGroundColor = new Color( 0.2f, 0.2f, 0.2f );
                    Console.m_InputFieldFontColor = Console.m_LogFontColor = Color.green;
                }
                else
                {
                    Console.m_LogBackGroundColor = OriginalColorBG;
                    Console.m_LogFontColor = OriginalColorFG;
                    Console.m_InputFieldBackGroundColor = OriginalColorBGInput;
                    Console.m_InputFieldFontColor = OriginalColorFGInput;
                }
            }
        }
    }
}
