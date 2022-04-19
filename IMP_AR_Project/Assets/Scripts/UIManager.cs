using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // About UI
    public GameObject mainScreen;
    public GameObject playScreen;
    public GameObject tutorialScreen;
    public GameObject optionScreen;
    public GameObject gameoverScreen;
    public GameObject HPBlood;
    public CanvasGroup Main_Cover;
    public CanvasGroup BeginInside_Cover;
    public CanvasGroup Begin_Cover;
    public CanvasGroup Damage_Cover;

    private int HP; // Player HP replica
    private GameObject life;
    private RectTransform lifeRect;
    List<GameObject> lifeList = new List<GameObject>();

    // Start Button
    public void mainStart()
    {
        mainScreen.gameObject.SetActive(false);
        playScreen.gameObject.SetActive(true);
        GameManager.instance.gamestatus = 1;

        // To Touch & If GamePlay, Mosquito Spawn
        GameObject.Find("Spawner").GetComponent<Spawner>().spawnMosquito();
        //GameObject.Find("UI").GetComponent<CanvasGroup>().blocksRaycasts = false;
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
    }

    // Go to Main Screen Quit
    public void mainHome()
    {
        tutorialScreen.gameObject.SetActive(false);
        optionScreen.gameObject.SetActive(false);
        gameoverScreen.gameObject.SetActive(false);
        mainScreen.gameObject.SetActive(true);
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
        HP = GameObject.Find("Player").GetComponent<Player>().HP;
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

    private void UI_HPControl()
    {
        HP = GameObject.Find("Player").GetComponent<Player>().HP;

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
        Damage_Cover.alpha = 0;

        StartCoroutine(DoFade());
    }

    // Show the notice before start the game
    IEnumerator DoFade()
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
            canvasGroup.alpha += Time.deltaTime * 20f;
            yield return new WaitForSeconds(.005f);
        }
        yield return new WaitForSeconds(.01f);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 20f;
            yield return new WaitForSeconds(.005f);
        }
        yield return null;
    }

    public void Update()
    {
        // Start status
        if (GameManager.instance.gamestatus == 0)
        {

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
            // show the Game over
            gameoverScreen.gameObject.SetActive(true);
            playScreen.gameObject.SetActive(false);
            GameObject.Find("Spawner").GetComponent<Spawner>().destroyMosquito();
        }
    }
}



