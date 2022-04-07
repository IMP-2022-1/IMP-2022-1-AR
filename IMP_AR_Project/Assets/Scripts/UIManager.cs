using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject playScreen;

    // Button Feature
    public void mainStart()
    {
        mainScreen.gameObject.SetActive(false);
        playScreen.gameObject.SetActive(true);
    }
}
