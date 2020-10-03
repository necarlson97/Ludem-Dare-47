using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PersonScript : MonoBehaviour {
    // Flashlight drawn for player (based on mouse and surround geometry)
    protected float flashlightThrow = 15f;  // Max distance in units
    protected float flashlightSpread = 40f; // Angle in degrees
    protected GameObject lightingObject;  // Just lighting source, not detector
    protected LayerMask opaqueLayer;  // The ojects that block the collider
    protected PolygonCollider2D flashlightCollider; // The actualy detector for what I can see
    
    protected TimeMachineScript tms;

    // Start is called before the first frame update
    protected void SetFlashlight(){
        lightingObject = transform.Find("Flashlight/SprayLight").gameObject;
        Light2D pointLight = lightingObject.GetComponent<Light2D>();
        pointLight.pointLightOuterRadius  = flashlightThrow;
        pointLight.pointLightOuterAngle = flashlightSpread;

        flashlightCollider = GetComponentInChildren<PolygonCollider2D>();
        opaqueLayer = LayerMask.GetMask("Opaque");

        tms = GameObject.Find("TimeMachine").GetComponent<TimeMachineScript>();
    }
    protected void UpdateFlashlight() {
        // Calculate the 'i see you' mesh
        UpdateFlashlightCollider();
    }

    protected void UpdateFlashlightCollider() {
        // Use a collider to find what this person can
        var flashlightStart = (Vector2) transform.position;
        var lookAngle = lightingObject.transform.rotation.eulerAngles.z;
        var lookVector = Quaternion.AngleAxis(lookAngle, Vector3.forward) * Vector3.up;

        // Using the mouse, position the center of the flashlight in that direction
        var flashlightRay = ((Vector2) lookVector).normalized * flashlightThrow;
        var flashlightCenter = flashlightStart + flashlightRay;

        // How many segments for the flashlight?
        int segmentCount = 10;

        // Setup the mesh, knowing we will need the flashlight start, and a point
        // for each segment tip
        List<Vector2> vertices = new List<Vector2>();
        vertices.Add(new Vector2()); // Center player

        // used to figure out far to spread each segment
        float segmentAngle = flashlightSpread / segmentCount;
        float leftAngle = -flashlightSpread / 2;

        // Using trig, get the points just to the left and right of the flashlight's center
        for (var i=0; i<segmentCount; i++) {
            var q = Quaternion.AngleAxis(leftAngle + segmentAngle * i, Vector3.forward);
            var flashlightSegment = (Vector2) ((Vector3) flashlightStart + q * flashlightRay);
            // Find where the flashlight hits the wall
            var flashlightSegmentHit = Physics2D.Linecast(flashlightStart, flashlightSegment, opaqueLayer);
            var flashlightSegmentEnd = flashlightSegmentHit.point;

            // TODO why do I have to do this?
            if (flashlightSegmentHit.distance <= 0) {
                flashlightSegmentEnd = flashlightSegment;
            }
            // Also, for now, flashlight is parented to player,
            // so sutract their location - could optimize 
            Vector2 flashlightSegmentRay = (Vector2) (flashlightSegmentEnd - ((Vector2) transform.position));

            // Debug.DrawLine(flashlightStart, flashlightStart + flashlightSegmentRay);
            
            vertices.Add(flashlightSegmentRay);
        }
        flashlightCollider.SetPath(0, vertices);
    }
}
