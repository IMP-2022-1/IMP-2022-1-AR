using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class Player : MonoBehaviour
{
    // Player Health Point
    public int HP = 3;

    // About RayCast
    private Vector3 touchPosition;
    bool touched;
    private int MosquitoLayerMask;
    [SerializeField] 
    public GameObject Spawner; // When Mosquito Catching, Used


    void Start()
    { 
        touched = false;
        MosquitoLayerMask = 1 << LayerMask.NameToLayer("Mosquito");
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


            // When GamePlaying & Player Touch, Raycast Occur 
            if (GameManager.instance.gamestatus == 1 || GameManager.instance.gamestatus == 2)
            {
                if (Physics.Raycast(ray, out hit, 100f, MosquitoLayerMask))
                {
                    Debug.Log("Raycast Occur");
                    if (hit.collider != null && hit.collider.CompareTag("Mosquito"))
                    {
                        Debug.Log("Heating!!");
                        MosquitoController raycastedMosquito = hit.collider.gameObject.GetComponent<MosquitoController>();
                        raycastedMosquito.MosquitoHP -= 1;
                        if (raycastedMosquito.MosquitoHP <= 0)
                        {

                            GameManager.instance.Score++;
                            GameManager.instance.TimeLimit = 10f;
                            GameManager.instance.TimerBar.value = 1;

                            Spawner.GetComponent<Spawner>().playerDestroyMosquito();
                            Destroy(hit.transform.gameObject);
                            Spawner.GetComponent<Spawner>().spawnMosquito();
                        }
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
