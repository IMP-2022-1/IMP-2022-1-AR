using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Use Singleton Pattern

    // About GamePlay
    public Player Player;
    public int Score = 0;
    public float TimeCount = 0f;
    public float TimeLimit = 10f;
    /* Check the gameStatus
     * 0 = Main
     * 1 = When GamePlay Start
     * 2 = Playing (Play Checked)
     * 3 = Game Over 
     * 4 = Player was Damaged */
    public int gamestatus = 0;
    Spawner spawner;

    // About UI
    public Text resultScore;
    public Text resultTime;
    public Text Timer;
    public Text ScoreCount;
    public Slider TimerBar;
    public Slider PlayerSprayGauge;



    private void Awake()
    {
        instance = this;
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }

    void Update()
    {
        // Game Start (Main Menu)
        if (GameManager.instance.gamestatus == 0)
        {
            // setting initial variable
            Score = 0;
            TimeCount = 0;
            Player.HP = 3;
        }

        // When Game Status is Playing mode
        if (GameManager.instance.gamestatus == 2)
        {
            // Timer
            TimerOperation();

            // Score
            ScoreCount.text = Score.ToString() + " Kills";

            PlayerSprayGauge.value = Player.SprayGauge;
        }
    }

    public void controlSound(float vol)
    {

    }

    private void TimerOperation()
    {
        TimeCount += Time.deltaTime;
        TimeLimit -= Time.deltaTime;
        Timer.text = TimeLimit.ToString("F1");
        TimerBar.value -= Time.deltaTime / 10.0f;

        if (TimerBar.value <= 0)
            TimerBar.transform.Find("Fill Area").gameObject.SetActive(false);
        else
            TimerBar.transform.Find("Fill Area").gameObject.SetActive(true);

        // When TimeOver
        if (TimeLimit <= 0)
            TimeOver();
    }

    private void TimeOver()
    {
        // Mosquitos Attack
        for (int i = spawner.Mosquitos.Count - 1; i >= 0; i--)
        {
            spawner.Mosquitos[i].GetComponent<MosquitoController>().MosquitoAttack();
        }

        GameManager.instance.gamestatus = 4;

        // if Player were dead
        if (Player.HP <= 0)
        {
            GameManager.instance.gamestatus = 3;
            spawner.destroyMosquito();

        }

        // Time Recover
        TimeLimit = 10f;
        TimerBar.value = 1;

        // Mosquito Destroy And Spawn
        spawner.destroyMosquito();
        spawner.spawnMosquito();
    }
}
