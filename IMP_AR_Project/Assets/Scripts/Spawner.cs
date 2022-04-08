using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool spawnEnable;

    public GameObject Mosquito;

    //스폰할 모기 레벨 체크할 인스턴스

    // Start is called before the first frame update
    void Start()
    {
        spawnMosquito();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject mosq = GameObject.FindGameObjectWithTag("Mosquito");

        if (mosq == null)
        {
            spawnEnable = true;
        }
        else
            spawnEnable = false;

        if (spawnEnable)
        {
            spawnMosquito();
        }
    }

    void spawnMosquito()
    {
        float randomX = Random.Range(-5f, 5f);
        float randomZ = Random.Range(-5f, 5f);

        GameObject mosq = (GameObject)Instantiate(Mosquito, new Vector3(randomX, 0f, randomZ), Quaternion.identity);
    } 
}
