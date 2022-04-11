using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject playScreen;
    public GameObject HPBlood;
    public CanvasGroup Main_Cover;
    public CanvasGroup BeginInside_Cover;
    public CanvasGroup Begin_Cover;
    private int HP;
    private GameObject life;
    private RectTransform lifeRect;
    List<GameObject> lifeList = new List<GameObject>();

    // Start Button
    public void mainStart()
    {
        mainScreen.gameObject.SetActive(false);
        playScreen.gameObject.SetActive(true);
        GameManager.instance.gamestatus = 1;
    }

    //Quit Button
    public void mainQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    }
#else
        Application.Quit();
    }
#endif

    //UI about HP
    private void UI_HP()
    {
        HP = GameObject.Find("AR Camera").GetComponent<Player>().HP;
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
        HP = GameObject.Find("AR Camera").GetComponent<Player>().HP;

        // if HP is changed by Mosquitos
        lifeList[HP].SetActive(false);
    }

    public void Start()
    {
        Begin_Cover.alpha = 1;
        BeginInside_Cover.alpha = 0;
        Main_Cover.alpha = 0;
    }

    // Show the notice before start the game
    IEnumerator DoFade()
    {
        CanvasGroup canvasGroup = BeginInside_Cover;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * 0.2f;
            yield return new WaitForSeconds(.2f);
        }
        canvasGroup.interactable = false;
        yield return new WaitForSeconds(3f);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 0.2f;
            yield return new WaitForSeconds(.2f);
        }
        canvasGroup.interactable = false;
        yield return new WaitForSeconds(2f);

        Begin_Cover.alpha = 0;

        CanvasGroup canvasGroup2 = Main_Cover;
        while (canvasGroup2.alpha < 1)
        {
            canvasGroup2.alpha += Time.deltaTime * 0.4f;
            yield return new WaitForSeconds(.2f);
        }

        yield return new WaitForSeconds(3f);
        yield return null;
    }

    public void Update()
    {
        // Start status
        if (GameManager.instance.gamestatus == 0)
        {
            StartCoroutine(DoFade());
        }

        // Turn the Game Mode UI on (like Awake status)
        if (GameManager.instance.gamestatus == 1)
        {
            // Show HP Icon
            UI_HP();
        }

        // If Player still play the game
        if (GameManager.instance.gamestatus == 2)
        {
            // Control HP Icon
            UI_HPControl();
        }

        // Game Over!
        if (GameManager.instance.gamestatus == 3)
        {
            // show the Game over
            GameManager.instance.GameOver.gameObject.SetActive(true);
        }
    }
}



