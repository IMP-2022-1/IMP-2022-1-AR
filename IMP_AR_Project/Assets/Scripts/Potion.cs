using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class Potion : MonoBehaviour
{
    private ARPlaneManager planeManager;

    // variable for prefab
    [SerializeField]
    private GameObject unrealPotionPrefab;
    [SerializeField]
    private GameObject realPotionPrefab;

    // potions used in script
    private GameObject unrealPotion;
    private GameObject realPotion;

    // check if unrealPotion, realPotion are generated
    private bool unrealPotionCheck;
    private bool realPotionCheck;

    private Vector3 unrealPotionPosition;
    private Vector3 ARCameraPosition;

    // About RayCast
    private Vector3 touchPosition;
    private int MosquitoLayerMask;

    // Start is called before the first frame update
    void Awake()
    {
        ARCameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        planeManager = GetComponent<ARPlaneManager>();        
        unrealPotionCheck = true;
        realPotionCheck = false;
        MosquitoLayerMask = 1 << LayerMask.NameToLayer("Mosquito"); 
    }

    // subscribe
    private void OnEnable()
    {
        planeManager.planesChanged += planesChanged;        
    }

    // unsubscribe
    private void OnDisable()
    {

    }
    private void planesChanged(ARPlanesChangedEventArgs args)
    {
        // execute here any code that you want to execute when planes have been detected or changed
        foreach (ARPlane plane in args.added)
        {
            if (GameManager.instance.Score >= 3 && unrealPotionCheck)
            {
                spawnUnRealPotion(plane);
            }
        }
    }   

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.Score >= 3 && realPotionCheck)
        {
            spawnRealPotion();
        }
    }

    // spawn unreal potion based on plane
    private void spawnUnRealPotion(ARPlane plane)
    {
        Debug.Log("unrealPotion Spawn!!!");
        unrealPotion = Instantiate(unrealPotionPrefab, plane.transform.position, Quaternion.identity);
        unrealPotionPosition = unrealPotion.transform.position;
        unrealPotionCheck = false;
        realPotionCheck = true;
    }

    // Spawn real potion when player get closed to unreal potion
    private void spawnRealPotion()
    {
        // Spawn in unrealPotion's loacation        
        Debug.Log("Distance between MainCamera and unrealPotion" + Vector3.Distance(GameObject.FindGameObjectWithTag("MainCamera").transform.position, unrealPotionPosition));
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("MainCamera").transform.position, unrealPotionPosition) < 2)
        {
            Debug.Log("realPotion Spawn!!!");
            realPotion = Instantiate(realPotionPrefab, unrealPotionPosition, Quaternion.identity);
            Destroy(unrealPotion);
            realPotionCheck = false;
        }
    }
}