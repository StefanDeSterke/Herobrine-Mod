using GameNetcodeStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HerobrineMod;

public class HerobrineEntity : MonoBehaviour
{
    public PlayerControllerB player;

    private bool coroutineRunning = false;
    private float time = 0f;

    private void Awake()
    {
        Plugin.Instance.GetLogger().LogInfo("Herobrine Added");
    }

    private void Update()
    {
        transform.LookAt(player.transform);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

        time += Time.deltaTime;

        if(IsInSight() && !coroutineRunning)
        {
            StartCoroutine(HerobrineSeen());
            coroutineRunning = true;
        }

        if (time > 20f)
        {
            Plugin.Instance.GetLogger().LogInfo("Herobrine Removed");
            Destroy(gameObject);
        }
    }

    private IEnumerator HerobrineSeen()
    {
        for(int i = 0; i < 6; i++)
        {
            if (!IsInSight())
            {
                StopAllCoroutines();
                coroutineRunning = false;
            }
            yield return new WaitForSeconds(0.1f);
        }

        Plugin.Instance.GetLogger().LogInfo("Herobrine Seen");
        Plugin.Instance.HerobrineSpawner.PlayCaveNoise(player);
        Destroy(gameObject);
    }

    private bool IsInSight()
    {
        return player.HasLineOfSightToPosition(transform.position + Vector3.up * 0.4f, 60f, 100, 5f) && Vector3.Distance(transform.position, player.transform.position) < 35f;
    }
}
