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

    private void Start()
    {
        for (int i = 0; i < MosquitoPrefab.Length; i++)
        {
            var TempMosquito = Instantiate(MosquitoPrefab[i]);
            Mosquitos.Add(TempMosquito);
            TempMosquito.SetActive(false);
            TempMosquito.transform.SetParent(transform);
        }
    }

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
        // GameObject MosquitoObject;
        if (GameManager.instance.Score < 6)
        {
            Mosquitos[0].transform.position = MosquitoPosition; 
            Mosquitos[0].SetActive(true);
            // MosquitoObject = Instantiate(MosquitoPrefab[0], MosquitoPosition, Quaternion.identity);
        }
        else if (GameManager.instance.Score < 11)
        {
            int NoM = Random.Range(0, (int)(Mosquitos.Count / 2));
            Mosquitos[NoM].transform.position = MosquitoPosition;
            Mosquitos[NoM].SetActive(true);
            // MosquitoObject = Instantiate(MosquitoPrefab[Random.Range(0, (int)MosquitoPrefab.Length / 2)], MosquitoPosition, Quaternion.identity);
        }
        else
        {
            int NoM = Random.Range((int)(Mosquitos.Count / 2), Mosquitos.Count);
            Mosquitos[NoM].transform.position = MosquitoPosition;
            Mosquitos[NoM].SetActive(true);
            // MosquitoObject = Instantiate(MosquitoPrefab[Random.Range((int)MosquitoPrefab.Length / 2, MosquitoPrefab.Length)], MosquitoPosition, Quaternion.identity);
        }
        //Mosquitos.Add(MosquitoObject);
    }


    // RemoveAllMosquitos
    public void destroyMosquito()
    {
        for (int i = Mosquitos.Count - 1; i >= 0; i--)
        {
            Mosquitos[i].SetActive(false);
            /*Destroy(Mosquitos[i]);
            Mosquitos.Remove(Mosquitos[i]);*/
        }
    }


    // Player Touch -> Destroy Mosquito
    public void playerDestroyMosquito()
    {
        Mosquitos.Remove(Mosquitos[0]);

        // Player -> Mosquito Destroy, Spawner -> Mosquito Destroy : Clear? Intergrated?

    }
}

