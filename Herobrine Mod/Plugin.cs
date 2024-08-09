using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HerobrineMod.Patches;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace HerobrineMod;

[BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string MOD_GUID = "StefanDeSterke.herobrinemod",
                         MOD_NAME = "Herobrine Mod",
                         MOD_VERSION = "1.0.0";

    private readonly Harmony harmony = new Harmony(MOD_GUID);

    public static Plugin Instance { get; private set; }

    public AssetBundle ModAssets { get; private set; }

    public HerobrineSpawner HerobrineSpawner { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Logger.LogError("Plugin instance already exists! Overriding singleton..");
        }

        Instance = this;

        LoadAssets();
        LoadPatches();

        HerobrineSpawner = new HerobrineSpawner();
    }

    private void LoadAssets()
    {
        string assetBundleFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "modassets");
        ModAssets = AssetBundle.LoadFromFile(assetBundleFilePath);
    }

    private void LoadPatches()
    {
        harmony.PatchAll(typeof(PlayerControllerPatch));
    }

    public ManualLogSource GetLogger()
    {
        return Logger;
    }
}