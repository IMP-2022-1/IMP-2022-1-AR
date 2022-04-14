using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class Player : MonoBehaviour
{
    public int HP = 3;


    private Vector3 touchPosition;
    bool touched;

    [SerializeField] 
    public GameObject Spawner;

    void Start()
    { 
        touched = false;
    }


    void Update()
    {
        touched = TryGetTouchPosition(out touchPosition);
    }

    private void FixedUpdate()
    {
        if (touched)
        {
            Ray ray;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider != null && hit.collider.CompareTag("Mosquito"))
                {
                    Debug.Log("Heating!!");
                    MosquitoController raycastedMosquito = hit.collider.gameObject.GetComponent<MosquitoController>();
                    raycastedMosquito.MosquitoHP -= 1;
                    if (raycastedMosquito.MosquitoHP <= 0)
                    {
                        // Mosquitos[0] reduplication ? 

                        Spawner.GetComponent<Spawner>().playerDestroyMosquito();
                        GameManager.instance.Score++;
                        GameManager.instance.TimeLimit = 10f;

                        Destroy(hit.transform.gameObject);

                    }
                }
            }
        }
    }

    private bool TryGetTouchPosition(out Vector3 touchPosition)
    {
        // get only one touch (holding down the finger will be ignored)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("Touched!");
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        else
        {
            touchPosition = default;
            return false;
        }
    }

}
