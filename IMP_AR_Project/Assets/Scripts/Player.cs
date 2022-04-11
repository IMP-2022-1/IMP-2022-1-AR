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


        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {

                Ray ray;
                RaycastHit hit;

                ray = arCamera.ScreenPointToRay(touch.position);

                Physics.Raycast(ray, out hit);

                if (hit.collider != null && hit.transform.gameObject.CompareTag("Mosquito"))
                {
                    MosquitoController raycastedMosquito = hit.collider.gameObject.GetComponent<MosquitoController>();
                    raycastedMosquito.MosquitoHP -= 1;
                    if (raycastedMosquito.MosquitoHP <= 0)
                    {
                        Destroy(hit.transform.gameObject);
                        GameManager.instance.Score++;
                        GameManager.instance.TimeLimit = 10f;
                    }


                }
            }
        }
    }

}
