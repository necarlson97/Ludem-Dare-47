using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemyScript : MonoBehaviour {

    private TimeMachineScript tms;
    private int age = 0; // Number of fixed updates alive
    private GameObject lightingObject;
    private float flashlightThrow = 15f;  // Max distance in units
    private float flashlightSpread = 40f; // Angle in degrees

    private LayerMask opaqueLayer;

    private PolygonCollider2D flashlightCollider;

    // Start is called before the first frame update
    void Start() {
        tms = GameObject.Find("TimeMachine").GetComponent<TimeMachineScript>();
        lightingObject = transform.Find("Flashlight/EnemyLighting").gameObject;
        Light2D pointLight = lightingObject.GetComponent<Light2D>();
        pointLight.pointLightOuterRadius  = flashlightThrow;
        pointLight.pointLightOuterAngle = flashlightSpread;

        flashlightCollider = GetComponentInChildren<PolygonCollider2D>();
        // The layer of objects that block light
        opaqueLayer = LayerMask.GetMask("Opaque");
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.position = tms.positions[age];
        lightingObject.transform.eulerAngles = new Vector3(0, 0, tms.lookAngles[age] - 90);
        age += 1;

        SetFlashlightCollider();
    }

    void SetFlashlightCollider() {
        // TODO this will likely need to be a collider or something
        var flashlightStart = transform.position;
        var lookVector = Quaternion.AngleAxis(tms.lookAngles[age], Vector3.forward) * Vector3.right;

        // Using the mouse, position the center of the flashlight in that direction
        var flashlightRay = ((Vector2) lookVector).normalized * flashlightThrow;
        var flashlightCenter = flashlightStart + ((Vector3) flashlightRay);

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
            var flashlightSegment = flashlightStart + q * flashlightRay;
            // Find where the flashlight hits the wall
            var flashlightSegmentEnd = Physics2D.Linecast(flashlightStart, flashlightSegment, opaqueLayer);

            // Also, for now, flashlight is parented to player, so sutract their location
            // could optimize 
            Vector2 flashlightPoint = flashlightSegmentEnd.point - ((Vector2) transform.position);
            vertices.Add(flashlightPoint);
        }
        flashlightCollider.SetPath(0, vertices);
    }
}
