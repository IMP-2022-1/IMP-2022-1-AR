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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray;
            RaycastHit hit;

            ray = arCamera.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out hit);

            if (hit.collider != null && hit.transform.gameObject.CompareTag("Mosquito"))
            {
                Destroy(hit.transform.gameObject);
                GameManager.instance.Score++;
                GameManager.instance.TimeLimit = 10f;
            }
        }


        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Rotate(-3f, 0, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Rotate(3f, 0, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(0, -3f, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(0, 3f, 0);
        }

    }
}
