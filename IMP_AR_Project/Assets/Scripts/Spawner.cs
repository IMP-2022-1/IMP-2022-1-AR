using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Having Prefabs about Mosquitos
    public GameObject[] MosquitoPrefab;

    // Having Created Mosquitos's GameObject
    public List<GameObject> Mosquitos;

    private bool spawnEnable;
    public float distance = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnMosquito()
    {
        // if spawnEnable true -> Animation can't occur 
        // Don't Need?

        Vector3 MosquitoPosition;
        MosquitoPosition = Random.onUnitSphere * distance;
        if (MosquitoPosition.y < 0)
            MosquitoPosition.y *= -1;

        // !!!!! diversification MosquitoPrefabs when difficulty UP!
        // MUST HAVE MosquitoPrefab has Prefabs
        GameObject MosquitoObject = Instantiate(MosquitoPrefab[0], MosquitoPosition, Quaternion.identity);
        Mosquitos.Add(MosquitoObject);

        spawnEnable = false;
    }


    // if Mosquito many -> 1. hp check / 2. method have argument 
    // if 2 -> Mosquitos is struct or Dictionary (struct : index check / dictionary : key check)
    public void destroyMosquito()
    {
        Destroy(Mosquitos[0]);
    }
}
