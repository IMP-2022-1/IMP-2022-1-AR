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

    // About Time
    public int timeMinute;

    // About Result
    public Text resultScore;
    public Text resultTime;
    public Text highScore;
    public Text highTime;
    public int savehighScore;
    public int savehighMinute;
    public float savehighSecond;

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
    public bool vibrateSwitch; // true : Can vibrate
    private int StepEffect;
    private AudioSource Effect;
    private AudioSource Music;
    private GameObject SoundEffectOn;
    private GameObject SoundEffectPart;
    private GameObject SoundEffectOff;
    private GameObject SoundMusicOn;
    private GameObject SoundMusicOff;


    private void Awake()
    {
        instance = this;
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();

        // About Option valuable
        vibrateSwitch = true;
        StepEffect = 0;
        Effect = GameObject.Find("SoundEffect").GetComponent<AudioSource>();
        Music = GameObject.Find("Music").GetComponent<AudioSource>();
        SoundEffectOn = GameObject.Find("SoundEffectOn");
        SoundEffectPart = GameObject.Find("SoundEffectPart");
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
            timeMinute = 0;
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

        if (timeMinute > savehighMinute || TimeCount > savehighSecond && timeMinute >= savehighMinute)
        {
            PlayerPrefs.SetInt("highMinute", timeMinute);
            PlayerPrefs.SetFloat("highSecond", TimeCount);
            savehighMinute = PlayerPrefs.GetInt("highMinute", 0);
            savehighSecond = PlayerPrefs.GetFloat("highSecond", 0);
            PlayerPrefs.Save();
        }

    }

    private void OptionCheck()
    {
        Effect.volume = soundEffect.value;
        Music.volume = soundMusic.value;

        // Volume On/Off UI
        if (Effect.volume == 0)
        {
            if (vibrateSwitch)
            {
                SoundEffectOn.SetActive(false);
                SoundEffectPart.SetActive(true);
                SoundEffectOff.SetActive(false);
            }
            else
            {
                SoundEffectOn.SetActive(false);
                SoundEffectPart.SetActive(false);
                SoundEffectOff.SetActive(true);
            }

        }
        else
        {
            SoundEffectOn.SetActive(true);
            SoundEffectPart.SetActive(false);
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

        // 1. Mute only vibrate
        if (StepEffect == 0)
        {
            vibrateSwitch = true;
            Handheld.Vibrate();
            soundEffect.value = 0;
            StepEffect++;
        }
        // 2. Mute All Effects
        else if (StepEffect == 1)
        {
            vibrateSwitch = false;
            soundEffect.value = 0;
            StepEffect++;
        }
        // 3. Turn on Volume
        else if (StepEffect == 2)
        {
            GameObject.Find("SoundEffect").GetComponent<SoundEffectController>().ReadyToPlay_Beep();
            vibrateSwitch = true;
            soundEffect.value = 1;
            StepEffect = 0;
        }

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

        if (TimeCount >= 60.0f - Time.deltaTime)
        {
            TimeCount = 0;
            timeMinute++;
        }

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
                if (spawner.Mosquitos[i].activeInHierarchy)
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
            if (spawner.Mosquitos[i].activeInHierarchy)
                spawner.Mosquitos[i].GetComponent<MosquitoController>().MosquitoAttack();
        }

        RTA = true;

        GameManager.instance.gamestatus = 4;

        // if Player were dead
        if (Player.HP <= 0)
        {
            GameManager.instance.gamestatus = 3;
            spawner.destroyMosquito();
            Player.isPressed = false;

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
