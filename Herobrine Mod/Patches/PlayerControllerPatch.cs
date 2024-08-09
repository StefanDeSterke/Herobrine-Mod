using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace HerobrineMod.Patches;

public static class PlayerControllerPatch
{
    [HarmonyPatch(typeof(PlayerControllerB), "Update")]
    [HarmonyPrefix]
    static void OnPlayerUpdate(PlayerControllerB __instance)
    {
        if((__instance.IsOwner && __instance.isPlayerControlled && (!__instance.IsServer || __instance.isHostPlayerObject)) || __instance.isTestingPlayer)
        {
            float chancePerSecond = 0.01f;

            float frameChance = 1f - Mathf.Pow(1f - chancePerSecond, Time.deltaTime);

            if (Random.Range(0f, 1f) <= frameChance)
            {
                Plugin.Instance.HerobrineSpawner.TrySpawnHerobrine(__instance);
            }
        }
    }
}