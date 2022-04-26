using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent (typeof(ARRaycastManager))]
public class PlanePotion : MonoBehaviour
{
    private ARPlaneManager planeManager;
    private ARAnchorManager anchorManager;
    
    [SerializeField]
    private GameObject unrealPotionToInstantiate;
    [SerializeField]
    private GameObject realPotionToInstantiate;

    public List<GameObject> ARPlanes = new List<GameObject>();
    private GameObject unrealPotion;
    private GameObject realPotion;
    
    private bool potionCheck; // check if potion is generated


    Vector3 ARCameraPosition;

    // Start is called before the first frame update
    void Awake()
    {

        ARCameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        planeManager = GetComponent<ARPlaneManager>();
        anchorManager = GetComponent<ARAnchorManager>();
        potionCheck = true;
    }

    // subscribe
    private void OnEnable()
    {
        planeManager.planesChanged += planesChanged;
        anchorManager.anchorsChanged += AnchorsChanged;
    }

    // unsubscribe
    private void OnDisable()
    {

    }

    private void planesChanged(ARPlanesChangedEventArgs args)
    {
        // execute here any code that you want to execute when planes have been detected or changed
        foreach(ARPlane plane in args.added)
        {
            spawnUnRealPotion(plane);
        }
    }

    private void AnchorsChanged(ARAnchorsChangedEventArgs args)
    {
        foreach (ARAnchor anchor in args.added)
        {
            //Debug.Log("Anchor added at: " + anchor.transform);
        }
        foreach (ARAnchor anchor in args.updated) {
        }
        foreach (ARAnchor anchor in args.removed) {
        }
    }

    private bool TryGetTouchPosition(out Vector3 touchPosition)
    {
        // get only one touch(holding finger down will be ignored)
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if(GameManager.instance.Score >= 3 && potionCheck)
        {
            spawnRealPotion();
        }
            
    }

    // spawn unreal potion based on plane
    private void spawnUnRealPotion(ARPlane plane)
    {
        /*// Random spawn in world space
        if (GameManager.instance.Score >= 3 && potionCheck)
        {
            Debug.Log("unrealPotion Spawn!!!");
            Vector3 potionPosition = ARCameraPosition + new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f));
            unrealPotion = Instantiate(unrealPotionToInstantiate, new Vector3(potionPosition.x, 0, potionPosition.z), Quaternion.identity);
            potionCheck = false;

        }*/

        // Spawn in plane's center
        if (GameManager.instance.Score >= 3 && potionCheck)
        {
            Debug.Log("unrealPotion Spawn!!!");
            unrealPotion = Instantiate(unrealPotionToInstantiate, plane.transform.position, Quaternion.identity);
            potionCheck = false;
        }
    }

    // Spawn real potion when player get closed to unreal potion
    private void spawnRealPotion()
    {
        // Spawn in plane's center
        Vector3 unrealPotionPosition = unrealPotion.transform.position;
        if (Vector3.Distance(ARCameraPosition, unrealPotionPosition)<1f)
        {
            Debug.Log("realPotion Spawn!!!");
            realPotion = Instantiate(realPotionToInstantiate, unrealPotionPosition, Quaternion.identity);
            Destroy(unrealPotion);
        }
    }
}
