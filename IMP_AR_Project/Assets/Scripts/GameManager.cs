using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text Timer;
    public Text ScoreCount;
    public Player Player;
    public Slider TimerBar;
    public int Score = 0;

    public static GameManager instance;

    public GameObject GameOver;

    public float TimeLimit = 10f;

    // Check the gameStatus
    // 0 = Main
    // 1 = Play
    // 2 = Play Checked
    // 3 = Game Over
    public int gamestatus = 0;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // When Game Status is Playing mode
        if (GameManager.instance.gamestatus == 2)
        {
            ///////Timer
            TimeLimit -= Time.deltaTime;
            Timer.text = TimeLimit.ToString("F1");
            TimerBar.value -= Time.deltaTime / 10.0f;

            if (TimerBar.value <= 0)
                TimerBar.transform.Find("Fill Area").gameObject.SetActive(false);
            else
                TimerBar.transform.Find("Fill Area").gameObject.SetActive(true);

            // if Timer were End
            if (TimeLimit <= 0)
            {
                // Player Life -1 and Time Reset
                Player.HP--;
                TimeLimit = 10f;
                TimerBar.value = 1;
            }

            ///////Score
            ScoreCount.text = Score.ToString() + " Kills";

            // if Player were dead
            if (Player.HP <= 0)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }
}
