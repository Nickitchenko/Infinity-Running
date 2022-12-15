using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartChunkSpawner : MonoBehaviour
{
    public GameObject[] chunk;
    private void Start()
    {
        const int coordinate_spawn = 10;
        int first_zikl;
        int second_zikl;
        for (first_zikl=0; first_zikl < 3; first_zikl++)
        {
            Vector3 spawnposition = new Vector3(0, 0, coordinate_spawn * first_zikl);
            Instantiate(chunk[0], spawnposition, Quaternion.identity, transform);
        }
        for (second_zikl = 0; second_zikl < 7; second_zikl++)
        {
            Vector3 spawnposition = new Vector3(0, 0, coordinate_spawn*(second_zikl+first_zikl));
            Instantiate(chunk[Random.Range(0, chunk.Length)], spawnposition, Quaternion.identity, transform);
        }
    }
}
