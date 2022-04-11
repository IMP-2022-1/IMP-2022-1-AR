using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject playScreen;
    public GameObject HPBlood;
    private int HP;
    private GameObject life;
    private RectTransform lifeRect;

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

        for (int i = 0; i < HP; i++)
        {
            life = Instantiate(HPBlood, new Vector2(0, 0), Quaternion.Euler(0, 0, 0), GameObject.Find("Screen").transform);
            lifeRect = life.GetComponent<RectTransform>();
            lifeRect.anchoredPosition = new Vector2(105 - i * interval, -215);
            life.transform.parent = playScreen.transform;
        }

        // Play Check
        GameManager.instance.gamestatus = 2;
    }

    public void Update()
    {
        // Turn the Game Mode UI on
        if (GameManager.instance.gamestatus == 1)
        {
            UI_HP();
        }
    }
}



