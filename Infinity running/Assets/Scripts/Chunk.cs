using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public StartChunkSpawner startChunkSpawner;
    private bool isSpawnNewChunk;

    private void Start()
    {
        startChunkSpawner = GameObject.FindGameObjectWithTag("ChunkSpawner").
            GetComponent<StartChunkSpawner>();
        if(Random.Range(0,2)==0)
        {
            Destroy(transform.parent.GetChild(2).gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player" && isSpawnNewChunk==false)
        {
            Vector3 spawnposition = transform.position;
            spawnposition.z += 100;
            
            Instantiate(startChunkSpawner.chunk[Random.Range(0, startChunkSpawner.chunk.Length)], spawnposition,
                Quaternion.identity, startChunkSpawner.gameObject.transform);
            isSpawnNewChunk = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            Destroy(transform.parent.gameObject, 5f);
        }
    }
}
