using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    // Player Health Point
    public int HP = 3;

    // About RayCast
    // private Vector3 touchPosition;
    // bool touched;
    private int MosquitoLayerMask;
    [SerializeField] 
    public GameObject Spawner; // When Mosquito Catching, Used

    [SerializeField] private Button sprayButton;

    [SerializeField] private GameObject Smoke;

    private bool isPressed = false;

    public float SprayGauge = 10.02f;

    private float SprayRadius;

    [SerializeField] private GameObject Crosshair;

    void Start()
    { 
        // touched = false;
        MosquitoLayerMask = 1 << LayerMask.NameToLayer("Mosquito");

    }


    void Update()
    {
        // Debug.Log(SprayRadius);

        if (isPressed)
        {
            Smoke.SetActive(true);
            SprayGauge -= Time.deltaTime;

            if(SprayGauge <= 0)
            {
                Smoke.SetActive(false);
                isPressed = false;
            }
        }
        else
        {
            Smoke.SetActive(false);
        }

        if (!isPressed && SprayGauge <= 10f)
        {
            SprayGauge += Time.deltaTime;
        }

        SprayRadius = 0.08f * ((SprayGauge / 10.02f) * 0.5f) + 0.04f;
        Crosshair.transform.localScale = new Vector3(SprayRadius / 0.08f - 0.12f, SprayRadius / 0.08f - 0.12f, SprayRadius / 0.08f - 0.12f);

        //touched = TryGetTouchPosition(out touchPosition);

    }

    private void FixedUpdate()
    {

        if (isPressed)
        {
            if (SprayGauge >= 0)
            {
                Spray();
            }
        }

        /*if (touched)
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
        }*/
    }


    public void Spray()
    {
        RaycastHit hit;
        
        if (GameManager.instance.gamestatus == 1 || GameManager.instance.gamestatus == 2)
        {

            if (Physics.SphereCast(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z) , SprayRadius, transform.forward, out hit, 10f, MosquitoLayerMask))
            {
                Debug.Log("Raycast Occur");
                // Debug.DrawRay(Camera.main.transform.position, transform.forward * 10f, Color.blue, 1f);

                if (hit.collider != null && hit.collider.CompareTag("Mosquito"))
                {
                    Debug.Log("Heating!!");
                    MosquitoController raycastedMosquito = hit.collider.gameObject.GetComponent<MosquitoController>();
                    raycastedMosquito.MosquitoHP -= 5 * Time.deltaTime;

                    // Sparying Sound Start
                    GameObject.Find("SoundEffect").GetComponent<SoundEffectController>().SprayingStart();

                    raycastedMosquito.MosquitoHeated();
                    if (raycastedMosquito.MosquitoHP <= 0)
                    {
                        // KilledMosquito Sound Start
                        GameObject.Find("SoundEffect").GetComponent<SoundEffectController>().KilledMosquitoStart();

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

    public void ButtonDown()
    {
        Debug.Log("Button pressed");
        isPressed = true;
        

    }

    public void ButtonUp()
    {
        // Debug.Log("Button not pressed");
        isPressed = false;
    }
}
