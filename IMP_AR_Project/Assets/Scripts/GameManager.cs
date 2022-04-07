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
    void Start()
    {

    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        TimeLimit -= Time.deltaTime;

        Timer.text = string.Format("{0:N1}", TimeLimit);

        HP.text = Player.HP.ToString();

        ScoreCount.text = Score.ToString();

        if (Player.HP <= 0)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            GameOver.gameObject.SetActive(true);
        }

        if (TimeLimit <= 0)
            UnityEditor.EditorApplication.isPlaying = false;
    }
}
