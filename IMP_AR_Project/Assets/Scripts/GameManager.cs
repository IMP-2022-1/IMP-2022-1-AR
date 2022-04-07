using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text Timer;
    public Text ScoreCount;
    public Text HP;
    public Player Player;

    public int Score = 0;

    public static GameManager instance;

    public GameObject GameOver;

    public float TimeLimit = 10f;

    // Check the gameStatus
    // 0 = Main
    // 1 = Play
    public int gamestatus = 0;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // When Game Status is Play mode
        if (GameManager.instance.gamestatus == 1)
        {
            TimeLimit -= Time.deltaTime;

            Timer.text = string.Format("{0:N1}", TimeLimit);

            HP.text = Player.HP.ToString();

            ScoreCount.text = Score.ToString();

            if (Player.HP <= 0 || TimeLimit <= 0)
            {
                GameOver.gameObject.SetActive(true);
                Application.Quit();
            }
        }
    }
}
