using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private SpawnMessage spawnMsg;
    private bool spawnPrefab;

    // Use this for initialization
    void Start()
    {
        spawnPrefab = false;
    }

    void Update()
    {
        if (spawnPrefab)
        {
            Instantiate(prefabs[spawnMsg.prefabId], spawnMsg.position, Quaternion.Euler(spawnMsg.rotation));
            spawnPrefab = false;
        }
    }

    public void Spawn(SpawnMessage msg)
    {
        Debug.Log(msg.payload);
        spawnMsg = msg;
        spawnPrefab = true;
    }
}
