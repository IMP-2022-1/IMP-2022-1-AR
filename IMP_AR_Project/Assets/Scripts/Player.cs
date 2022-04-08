using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class Player : MonoBehaviour
{
    public int HP = 3;


    [SerializeField] private Camera arCamera;

    void Start()
    {

    }


    void Update()
    {


        if (Input.touchCount == 0) return;

        Ray ray;
        RaycastHit hit;

        ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);

        Physics.Raycast(ray, out hit);

        if (hit.collider != null && hit.transform.gameObject.CompareTag("Mosquito"))
        {
            Destroy(hit.transform.gameObject);
            GameManager.instance.Score++;
            GameManager.instance.TimeLimit = 10f;
        }
    }

}
