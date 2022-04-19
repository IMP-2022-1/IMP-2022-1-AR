using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Having Prefabs about Mosquitos
    public GameObject[] MosquitoPrefab;

    // Having Created Mosquitos's GameObject
    public List<GameObject> Mosquitos = new List<GameObject>();

    // Varialbe for dicide limition of mosquito's spawn locaiton
    public float distance = 1;

    public void spawnMosquito()
    {
        // if spawnEnable true -> Animation can't occur 
        // Don't Need?

        /* Case1
        Vector3 MosquitoPosition;
        MosquitoPosition = Random.onUnitSphere * distance;
        if (MosquitoPosition.y < 0)
            MosquitoPosition.y *= -1;

        // !!!!! diversification MosquitoPrefabs when difficulty UP!
        // MUST HAVE MosquitoPrefab has Prefabs
        GameObject MosquitoObject = Instantiate(MosquitoPrefab[0], MosquitoPosition, Quaternion.identity);
        Mosquitos.Add(MosquitoObject);
        */

        /* Case2 */
        Transform ARCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Vector3 MosquitoTempPosition = Random.insideUnitCircle.normalized;
        Vector3 MosquitoPosition = ARCameraTransform.position + new Vector3(MosquitoTempPosition.x, Random.Range(-0.3f,0.3f), MosquitoTempPosition.y);
        GameObject MosquitoObject;
        if (GameManager.instance.Score < 6)
            MosquitoObject = Instantiate(MosquitoPrefab[0], MosquitoPosition, Quaternion.identity);
        else if (GameManager.instance.Score < 11)
        {
            MosquitoObject = Instantiate(MosquitoPrefab[Random.Range(0, (int) MosquitoPrefab.Length / 2)], MosquitoPosition, Quaternion.identity);
        } else
        {
            MosquitoObject = Instantiate(MosquitoPrefab[Random.Range((int) MosquitoPrefab.Length/2, MosquitoPrefab.Length)], MosquitoPosition, Quaternion.identity);
        }
        Mosquitos.Add(MosquitoObject);
    }


    // RemoveAllMosquitos
    public void destroyMosquito()
    {
        for (int i = Mosquitos.Count - 1; i >= 0; i--)
        {
            Destroy(Mosquitos[i]);
            Mosquitos.Remove(Mosquitos[i]);
        }
    }


    // Player Touch -> Destroy Mosquito
    public void playerDestroyMosquito()
    {
        Mosquitos.Remove(Mosquitos[0]);

        // Player -> Mosquito Destroy, Spawner -> Mosquito Destroy : Clear? Intergrated?

    }
}

