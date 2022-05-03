using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Rendering;

public class LightEstimation : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager cameraManager;
    private Light light;

    // Start is called before the first frame update
    void Start()
    {
        //cameraManager = FindObjectOfType<ARCameraManager>();
        light = GetComponent<Light>();
    }

    private void OnEnable()
    {
        cameraManager.frameReceived += ProcessFrame;
    }

    private void OnDisable()
    {
        cameraManager.frameReceived -= ProcessFrame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ProcessFrame(ARCameraFrameEventArgs args)
    {
        ARLightEstimationData data = args.lightEstimation;

        //birghtness
        if (data.averageBrightness.HasValue)
        {
            // change the virtual light brightness
            light.intensity = data.averageBrightness.Value;
        }
        // color temperature
        if (data.averageColorTemperature.HasValue)
        {
            light.colorTemperature = data.averageColorTemperature.Value;
        }
        // color correction
        if (data.colorCorrection.HasValue)
        {
            light.color = data.colorCorrection.Value;
        }
        // direction
        if (data.mainLightDirection.HasValue)
        {
            light.transform.rotation = Quaternion.LookRotation(data.mainLightDirection.Value) ;
        }
        // intensity in Lumens
        if (data.mainLightIntensityLumens.HasValue)
        {
            light.intensity = data.mainLightIntensityLumens.Value;
        }
        if (data.ambientSphericalHarmonics.HasValue) {
            // spherical harmonics
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientProbe = data.ambientSphericalHarmonics.Value;
        }
    }
}
