using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // About UI
    public GameObject mainScreen;
    public GameObject mainBeforeScreen;
    public GameObject playScreen;
    public GameObject tutorialScreen;
    public GameObject optionScreen;
    public GameObject gameoverScreen;
    public GameObject HPBlood;
    public CanvasGroup Main_Cover;
    public CanvasGroup BeginInside_Cover;
    public CanvasGroup Begin_Cover;
    public CanvasGroup Damage_Cover;
    public CanvasGroup Play_Cover;
    public CanvasGroup PlayInside_Cover;

    private int HP; // Player HP replica
    private float tempVolume; // temporary save the volume
    private GameObject life;
    private RectTransform lifeRect;
    List<GameObject> lifeList = new List<GameObject>();

    // Start Button (Show the UI before playing)
    public void mainStart_before()
    {
        mainBeforeScreen.gameObject.SetActive(true);
        StartCoroutine(PlayFade());
    }

    // and Start the game
    public void mainStart()
    {
        mainScreen.gameObject.SetActive(false);
        mainBeforeScreen.gameObject.SetActive(false);
        playScreen.gameObject.SetActive(true);
        GameManager.instance.gamestatus = 1;

        // To Touch & If GamePlay, Mosquito Spawn
        GameObject.Find("Spawner").GetComponent<Spawner>().spawnMosquito();
        // Music -> Play Sound Start
        GameObject.Find("Music").GetComponent<MusicController>().PlaySoundStart();
    }

    // Tutorial button
    public void mainTutorial()
    {
        tutorialScreen.gameObject.SetActive(true);
        mainScreen.gameObject.SetActive(false);
    }

    // Option button
    public void mainOption()
    {
        optionScreen.gameObject.SetActive(true);
        mainScreen.gameObject.SetActive(false);
        GameManager.instance.optionSwitch = true;
    }

    // Go to Main Screen Quit
    public void mainHome()
    {
        tutorialScreen.gameObject.SetActive(false);
        optionScreen.gameObject.SetActive(false);
        gameoverScreen.gameObject.SetActive(false);
        mainBeforeScreen.gameObject.SetActive(false);
        mainScreen.gameObject.SetActive(true);
        GameManager.instance.optionSwitch = false;
        GameManager.instance.gamestatus = 0;
    }



    // Quit Button
    public void mainQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    }
#else
        Application.Quit();
    }
#endif

    //UI about HP - Instantiate
    private void UI_HP()
    {
        // Please Comment like this code : AR Camera -> Player
        HP = GameManager.instance.Player.HP;
        // Interval of image
        int interval = 20;

        for (int i = 0; i < HP + 1; i++)
        {
            life = Instantiate(HPBlood, new Vector2(0, 0), Quaternion.Euler(0, 0, 0), GameObject.Find("Screen").transform) as GameObject;
            lifeRect = life.GetComponent<RectTransform>();
            lifeRect.anchoredPosition = new Vector2(105 - i * interval, -215);
            life.transform.parent = playScreen.transform;
            lifeList.Add(life);
        }

        // Play Check
        GameManager.instance.gamestatus = 2;
    }

    // UI HP Reset
    private void UI_HPReset()
    {
        GameObject HPs = GameObject.FindGameObjectWithTag("HPBlood");
        for (int i = lifeList.Count - 1; i >= 0; i--)
        {
            Destroy(lifeList[i]);
            lifeList.Remove(lifeList[i]);
        }

    }

    private void UI_HPControl()
    {
        HP = GameManager.instance.Player.HP;

        // if HP is changed by Mosquitos
        lifeList[HP].SetActive(false);

        // Damage Effects
        if (GameManager.instance.gamestatus == 4)
        {
            StartCoroutine(DamageFade());
            GameManager.instance.gamestatus = 2;
        }
    }

    public void Start()
    {
        Begin_Cover.alpha = 1;
        BeginInside_Cover.alpha = 0;
        Main_Cover.alpha = 0;
        Play_Cover.alpha = 0;
        PlayInside_Cover.alpha = 0;
        Damage_Cover.alpha = 0;

        optionScreen.gameObject.SetActive(false);
        StartCoroutine(StartFade());
    }

    // Show the notice before start the game
    IEnumerator StartFade()
    {
        CanvasGroup canvasGroup = BeginInside_Cover;
        yield return new WaitForSeconds(.5f);

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * 0.75f;
            yield return new WaitForSeconds(.0005f);
        }
        canvasGroup.interactable = false;
        yield return new WaitForSeconds(3f);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * .75f;
            yield return new WaitForSeconds(.0005f);
        }
        canvasGroup.interactable = false;
        yield return new WaitForSeconds(2f);

        Begin_Cover.alpha = 0;

        CanvasGroup canvasGroup2 = Main_Cover;
        while (canvasGroup2.alpha < 1)
        {
            canvasGroup2.alpha += Time.deltaTime * 2f;
            yield return new WaitForSeconds(.0005f);
        }

        yield return new WaitForSeconds(3f);
        yield return null;
    }

    // When Player damaged, show the effect.
    IEnumerator DamageFade()
    {
        CanvasGroup canvasGroup = Damage_Cover;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * 5f;
            yield return new WaitForSeconds(.005f);
        }
        yield return new WaitForSeconds(.01f);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 5f;
            yield return new WaitForSeconds(.005f);
        }
        yield return null;
    }

    // Before the play, We will show the some UI
    IEnumerator PlayFade()
    {
        CanvasGroup canvasGroup = Play_Cover;
        CanvasGroup canvasGroup2 = PlayInside_Cover;
        CanvasGroup canvasGroup3 = Main_Cover;
        AudioSource music = GameObject.Find("Music").GetComponent<AudioSource>();
        tempVolume = music.volume;
        GameManager.instance.playBeforeCount.gameObject.SetActive(false);
        canvasGroup.alpha = 0;
        canvasGroup2.alpha = 0;

        yield return new WaitForSeconds(.75f);

        // 0. Fade out Main Screen
        while (canvasGroup3.alpha > 0)
        {
            canvasGroup3.alpha -= Time.deltaTime * 0.75f;
            yield return new WaitForSeconds(.0005f);
        }

        // 1. Music Fade out and Show the BG
        while (canvasGroup.alpha < 1 || music.volume > 0)
        {
            music.volume -= Time.deltaTime * 0.50f;
            canvasGroup.alpha += Time.deltaTime * 0.75f;
            yield return new WaitForSeconds(.0005f);
        }

        // 2. Play the Sigh
        GameObject.Find("SoundEffect").GetComponent<SoundEffectController>().ReadyToPlay_Sigh();
        yield return new WaitForSeconds(.25f);

        // 3. Show the Message
        while (canvasGroup2.alpha < 1)
        {
            canvasGroup2.alpha += Time.deltaTime * 0.75f;
            yield return new WaitForSeconds(.0005f);
        }

        yield return new WaitForSeconds(.5f);

        // 4. and Clean up BG
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 0.75f;
            yield return new WaitForSeconds(.0005f);
        }

        yield return new WaitForSeconds(.5f);

        // 5. and Clean up Message
        while (canvasGroup2.alpha > 0)
        {
            canvasGroup2.alpha -= Time.deltaTime * 0.75f;
            yield return new WaitForSeconds(.0005f);
        }

        yield return new WaitForSeconds(.9f);

        // 6. Show the Count Down
        GameManager.instance.playBeforeCount.gameObject.SetActive(true);
        GameManager.instance.playBeforeCount.fontSize = 72;
        GameManager.instance.playBeforeCount.text = "3";
        GameObject.Find("SoundEffect").GetComponent<SoundEffectController>().ReadyToPlay_Beep();

        yield return new WaitForSeconds(.93f);

        GameManager.instance.playBeforeCount.text = "2";
        GameObject.Find("SoundEffect").GetComponent<SoundEffectController>().ReadyToPlay_Beep();

        yield return new WaitForSeconds(.93f);

        GameManager.instance.playBeforeCount.text = "1";
        GameObject.Find("SoundEffect").GetComponent<SoundEffectController>().ReadyToPlay_Beep();

        yield return new WaitForSeconds(.93f);

        GameManager.instance.playBeforeCount.text = "Start!";
        GameObject.Find("SoundEffect").GetComponent<SoundEffectController>().ReadyToPlay_Alarm();

        yield return new WaitForSeconds(1.85f);

        // 7. Setting value
        canvasGroup.alpha = 0;
        canvasGroup2.alpha = 0;
        canvasGroup3.alpha = 1;
        music.volume = tempVolume;
        GameManager.instance.playBeforeCount.gameObject.SetActive(false);

        // 8. Game Start
        mainStart();
        yield return null;
    }

    public void Update()
    {
        // Start status
        if (GameManager.instance.gamestatus == 0)
        {
            // Main Music Start
            if (Begin_Cover.alpha == 0)
                GameObject.Find("Music").GetComponent<MusicController>().MainMusicStart();
        }

        // Turn the Game Mode UI on (like Awake status)
        if (GameManager.instance.gamestatus == 1)
        {
            // Show HP Icon
            UI_HP();
        }

        // If Player still play the game Or Player was attacked by Mosquitos
        if (GameManager.instance.gamestatus == 2 || GameManager.instance.gamestatus == 4)
        {
            // Control HP Icon
            UI_HPControl();
        }

        // Game Over!
        if (GameManager.instance.gamestatus == 3)
        {
            // HP UI Reset
            UI_HPReset();

            // show the Game over
            gameoverScreen.gameObject.SetActive(true);
            playScreen.gameObject.SetActive(false);

            // Pause Spawner
            GameObject.Find("Spawner").GetComponent<Spawner>().destroyMosquito();

            // Show the Result
            GameManager.instance.resultScore.text = GameManager.instance.Score.ToString() + " Kills";
            GameManager.instance.resultTime.text = GameManager.instance.timeMinute.ToString("F0") + ":" + GameManager.instance.TimeCount.ToString("F0");

            GameManager.instance.highScore.text = GameManager.instance.savehighScore.ToString() + " Kills";
            GameManager.instance.highTime.text = GameManager.instance.savehighMinute.ToString("F0") + ":" + GameManager.instance.savehighSecond.ToString("F0");
        }
    }
}



