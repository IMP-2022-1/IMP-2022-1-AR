using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool spawnEnable;

    public GameObject Mosquito;

    // Variable for spawnMosquito();
    GameObject mosq;



    // Variable for using Timelimit in destroyMosquito()
    GameManager gameManager;

    //스폰할 모기 레벨 체크할 인스턴스

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GamerManager").GetComponent<GameManager>();
        spawnMosquito();
    }

    // Update is called once per frame
    void Update()
    {
        mosq = GameObject.FindGameObjectWithTag("Mosquito");

        if (mosq == null)
        {
            Debug.Log("1");
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

        mosq = (GameObject)Instantiate(Mosquito, new Vector3(randomX, 0f, randomZ), Quaternion.identity);
    }
    
    void destroyMousquito()
    {
        // Destroy mosquito if time limit is over
        if(gameManager.TimeLimit == 0)
        {
            Destroy(mosq);
        }

        // Destroy mosquito if mosquito's HP is 0
        // Mosquito_HP in mosquito.cs is gone
    }
}
