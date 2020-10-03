using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour {
    // Flashlight drawn for player (based on mouse and surround geometry)
    private float flashlightThrow = 15f;  // Max distance in units
    private float flashlightSpread = 40f; // Angle in degrees
    private GameObject lightingObject;  // TODO DOC just lighting source, not detector mesh
    
    private TimeMachineScript tms;

    // Start is called before the first frame update
    void SetFlashlight(){
        lightingObject = transform.Find("Flashlight/PlayerLighting").gameObject;
        Light2D pointLight = lightingObject.GetComponent<Light2D>();
        pointLight.pointLightOuterRadius  = flashlightThrow;
        pointLight.pointLightOuterAngle = flashlightSpread;
        tms = GameObject.Find("TimeMachine").GetComponent<TimeMachineScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
