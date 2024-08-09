using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace HerobrineMod.Patches;

public static class StartOfRoundPatch
{
    [HarmonyPatch(typeof(StartOfRound), "Awake")]
    [HarmonyPostfix]
    static void OnStartOfRoundAwakePatch()
    {
        
    }
}
