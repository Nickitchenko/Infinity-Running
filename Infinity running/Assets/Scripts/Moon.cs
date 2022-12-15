using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    private void OnTriggerEnter(Collider collsiion)
    {
        if(collsiion.gameObject.tag=="Player")
        {
            collsiion.gameObject.GetComponent<Player>().moons++;
            Destroy(gameObject);
        }
    }
}
