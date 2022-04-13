using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text Timer;
    public Text ScoreCount;
    public Player Player;

    public int Score = 0;

    public static GameManager instance;

    public GameObject GameOver;

    //!!!!!!!!!!!!! To Test, Time Limit Change 100
    public float TimeLimit = 100f;

    // Check the gameStatus
    // 0 = Main
    // 1 = Play
    public int gamestatus = 0;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {

        // When Game Status is Play mode
        if (GameManager.instance.gamestatus == 1)
        {
            TimeLimit -= Time.deltaTime;

            Timer.text = string.Format("{0:N1}", TimeLimit);

            ScoreCount.text = Score.ToString() + " Kills";

            // if Player were dead
            if (Player.HP <= 0)
            {
                GameOver.gameObject.SetActive(true);
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }
}
