using GameNetcodeStuff;
using UnityEngine;
using System.Collections;

namespace HerobrineMod;

public class HerobrineSpawner
{
    private GameObject herobrinePrefab, caveNoisePlayerPrefab;
    private AudioClip[] caveNoises;

    private HerobrineEntity herobrineEntity;

    public HerobrineSpawner()
    {
        herobrinePrefab = Plugin.Instance.ModAssets.LoadAsset<GameObject>("Assets/Prefabs/Herobrine.prefab");
        caveNoises = Plugin.Instance.ModAssets.LoadAllAssets<AudioClip>();
        caveNoisePlayerPrefab = Plugin.Instance.ModAssets.LoadAsset<GameObject>("Assets/Prefabs/Cave Noise Sound.prefab");
    }

    public void TrySpawnHerobrine(PlayerControllerB player)
    {
        if (herobrineEntity != null)
            return;

        Vector3 spawnPos = FindHerobrineSpawnPoint(player);

        if (spawnPos == Vector3.zero)
            return;

        GameObject herobrineGameObject = GameObject.Instantiate(herobrinePrefab, spawnPos, Quaternion.identity);
        herobrineEntity = herobrineGameObject.AddComponent<HerobrineEntity>();
        herobrineEntity.player = player;
    }

    private Vector3 FindHerobrineSpawnPoint(PlayerControllerB player)
    {
        GameObject[] aiNodes = player.isInsideFactory ? RoundManager.Instance.insideAINodes : RoundManager.Instance.outsideAINodes;

        foreach(GameObject aiNode in aiNodes)
        {
            if(!Physics.Linecast(player.gameplayCamera.transform.position, aiNode.transform.position, StartOfRound.Instance.collidersAndRoomMaskAndDefault) && !player.HasLineOfSightToPosition(aiNode.transform.position, 80f, 100, 8f))
            {
                return aiNode.transform.position;
            }
        }

        return Vector3.zero;
    }

    public void PlayCaveNoise(PlayerControllerB player)
    {
        AudioClip randomCaveNoise = caveNoises[Random.Range(0, caveNoises.Length)];
        GameObject caveNoisePlayer = GameObject.Instantiate(caveNoisePlayerPrefab, player.transform.position, Quaternion.identity);
        AudioSource audioSource = caveNoisePlayer.GetComponent<AudioSource>();
        audioSource.clip = randomCaveNoise;
        audioSource.Play();
    }
}
