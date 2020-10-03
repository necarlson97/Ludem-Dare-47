using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    // Player move force
    private float speed = 50f;

    // Flashlight drawn for player (based on mouse and surround geometry)
    private float flashlightThrow = 10f;  // Max distance in units
    private float flashlightSpread = 40f; // Angle in degrees
    private GameObject lightingObject;  // TODO DOC just lightin g source, not detector mesh
    private Mesh flashlightMesh;
    private LayerMask opaqueLayer;


    private Rigidbody2D rb2d;
    
    private TimeMachineScript tms;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        flashlightMesh = new Mesh();
        GetComponentInChildren<MeshFilter>().mesh = flashlightMesh;

        lightingObject = GameObject.Find("PlayerLighting");

        // The layer of objects that block light
        opaqueLayer = LayerMask.GetMask("Opaque");

        tms = GameObject.Find("TimeMachine").GetComponent<TimeMachineScript>();
    }

    void FixedUpdate() {
        Move();
        Flashlight();

        tms.Save(rb2d, GetLookAngle());
    }

    void Move() {
        // WASD movement, normalized to speed
        // TODO add arrow keys, controller input, etc
        var move = new Vector2(0, 0);
        if (Input.GetKey("w")) {
            move.y += 1;
        }
        if (Input.GetKey("s")) {
            move.y -= 1;
        }
        if (Input.GetKey("a")) {
            move.x -= 1;
        }
        if (Input.GetKey("d")) {
            move.x += 1;
        }

        // Don't have diagnonals be faster
        move = move.normalized * speed;

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        rb2d.AddForce(move);
    }

    float GetLookAngle() {
        // Get the angle we are looking at based on mouse
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mousePos -= rb2d.transform.position;
        // Twist light to point twoards mouse
        return Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
    }

    void Flashlight() {
        // Render our players flashlight
        lightingObject.transform.eulerAngles = new Vector3(0, 0, GetLookAngle() - 90);
    }

    void FlashlightMesh() {
        // TODO this will linkely need to be a collider or something
        var flashlightStart = rb2d.transform.position;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mousePos -= flashlightStart;

        // Using the mouse, position the center of the flashlight in that direction
        var flashlightRay = ((Vector2) mousePos).normalized * flashlightThrow;
        var flashlightCenter = flashlightStart + ((Vector3) flashlightRay);

        // How many segments for the flashlight?
        int segmentCount = 10;

        // Setup the mesh, knowing we will need the flashlight start, and a point
        // for each segment tip
        Vector3[] vertices = new Vector3[segmentCount + 1];
        vertices[0] = new Vector3(); // Center player
        int[] triangles = new int[segmentCount * 3];

        // used to figure out far to spread each segment
        float segmentAngle = flashlightSpread / segmentCount;
        float leftAngle = -flashlightSpread / 2;

        // Using trig, get the points just to the left and right of the flashlight's center
        for (var i=0; i<segmentCount; i++) {
            var q = Quaternion.AngleAxis(leftAngle + segmentAngle * i, Vector3.forward);
            var flashlightSegment = flashlightStart + q * flashlightRay * flashlightThrow;
            // Find where the flashlight hits the wall
            var flashlightSegmentEnd = Physics2D.Linecast(flashlightStart, flashlightSegment, opaqueLayer);

            // example for 'triangles' with 2 line segments:
            // 0, 1, 2, 0, 2, 3
            // Also, for now, flashlight is parented to player, so sutract their location
            // could optimize 
            vertices[i+1] = flashlightSegmentEnd.point - ((Vector2) rb2d.transform.position);
            triangles[i*3 + 1] = i;
            triangles[i*3 + 2] = i + 1;
        }
        flashlightMesh.Clear();
        flashlightMesh.vertices = vertices;
        flashlightMesh.triangles = triangles;
    }
}
