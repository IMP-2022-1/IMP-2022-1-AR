using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mosquito : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (GameManager.instance.TimeLimit <= 0)
        {
            attack();
            GameManager.instance.TimeLimit = 10f;
        }
    }

    public void attack()
    {
        GameManager.instance.Player.HP--;

        Destroy(this.transform.gameObject);
    }
}
