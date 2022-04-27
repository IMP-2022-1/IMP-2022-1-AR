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

    public bool RTA = true;


    // About Result
    public Text resultScore;
    public Text resultTime;
    public Text highScore;
    public Text highTime;
    public int savehighScore;
    public float savehighTime;

    // About UI
    public Text Timer;
    public Text playBeforeCount;
    public Text ScoreCount;
    public Slider TimerBar;
    public Slider PlayerSprayGauge;
    public Slider soundEffect;
    public Slider soundMusic;

    // About Option
    public bool optionSwitch;
    private AudioSource Effect;
    private AudioSource Music;
    private GameObject SoundEffectOn;
    private GameObject SoundEffectOff;
    private GameObject SoundMusicOn;
    private GameObject SoundMusicOff;


    private void Awake()
    {
        instance = this;
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();

        // About Option valuable
        Effect = GameObject.Find("SoundEffect").GetComponent<AudioSource>();
        Music = GameObject.Find("Music").GetComponent<AudioSource>();
        SoundEffectOn = GameObject.Find("SoundEffectOn");
        SoundEffectOff = GameObject.Find("SoundEffectOff");
        SoundMusicOn = GameObject.Find("SoundMusicOn");
        SoundMusicOff = GameObject.Find("SoundMusicOff");
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

            // option control Swtich
            if (optionSwitch)
                OptionCheck();
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

        // when Game is over, save the result for high score
        if (GameManager.instance.gamestatus == 3)
        {
            saveResult();
        }
    }

    private void saveResult()
    {
        if (Score > savehighScore)
        {
            PlayerPrefs.SetInt("highScore", Score);
            savehighScore = PlayerPrefs.GetInt("highScore", 0);
            PlayerPrefs.Save();
        }

        if (TimeCount > savehighTime)
        {
            PlayerPrefs.SetFloat("highTime", TimeCount);
            savehighTime = PlayerPrefs.GetFloat("highTime", 0);
            PlayerPrefs.Save();
        }

    }

    public void controlSound(float vol)
    {

    }

    private void OptionCheck()
    {
        Effect.volume = soundEffect.value;
        Music.volume = soundMusic.value;

        // Volume On/Off UI
        if (Effect.volume == 0)
        {
            SoundEffectOn.SetActive(false);
            SoundEffectOff.SetActive(true);
        }
        else
        {
            SoundEffectOn.SetActive(true);
            SoundEffectOff.SetActive(false);
        }

        if (Music.volume == 0)
        {
            SoundMusicOn.SetActive(false);
            SoundMusicOff.SetActive(true);
        }
        else
        {
            SoundMusicOn.SetActive(true);
            SoundMusicOff.SetActive(false);
        }
    }

    public void OptionEffectMute()
    {
        if (Effect.volume > 0)
            soundEffect.value = 0;
        else
            soundEffect.value = 1;
    }

    public void OptionMusicMute()
    {
        if (Music.volume > 0)
            soundMusic.value = 0;
        else
            soundMusic.value = 1;
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

        if (TimeLimit <= 0.4f && RTA == true)
        {
            for (int i = spawner.Mosquitos.Count - 1; i >= 0; i--)
            {
                spawner.Mosquitos[i].GetComponent<MosquitoController>().ReadyToAttack();
            }
            RTA = false;
        }

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

        RTA = true;

        GameManager.instance.gamestatus = 4;

        // if Player were dead
        if (Player.HP <= 0)
        {
            GameManager.instance.gamestatus = 3;
            spawner.destroyMosquito();

            // GameOverMusic Start
            GameObject.Find("Music").GetComponent<MusicController>().GameOverMusicStart();
        }

        // Time Recover
        TimeLimit = 10f;
        TimerBar.value = 1;

        // Mosquito Destroy And Spawn
        spawner.destroyMosquito();
        spawner.spawnMosquito();
    }
}
