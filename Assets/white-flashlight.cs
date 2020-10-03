// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;

// // public class PlayerScript : MonoBehaviour {

// //     // Player move force
// //     private float speed = 50f;

// //     // Flashlight drawn for player (based on mouse and surround geometry)
// //     private float flashlightThrow = 10f;  // Max distance in units
// //     private float flashlightSpread = 40f; // Angle in degrees
// //     private Mesh flashlightMesh;
// //     private LayerMask opaqueLayer;


// //     private Rigidbody2D rb2d;
    
// //     private TimeMachineScript tms;

// //     void Start() {
// //         rb2d = GetComponent<Rigidbody2D>();
// //         flashlightMesh = new Mesh();
// //         GameObject.Find("Flashlight").GetComponent<MeshFilter>().mesh = flashlightMesh;

// //         // The layer of objects that block light
// //         opaqueLayer = LayerMask.GetMask("Opaque");

// //         tms = GameObject.Find("TimeMachine").GetComponent<TimeMachineScript>();
// //     }

// //     void FixedUpdate() {
// //         Move();
// //         Flashlight();
// //     }

// //     void Move() {
// //         // WASD movement, normalized to speed
// //         // TODO add arrow keys, controller input, etc
// //         var move = new Vector2(0, 0);
// //         if (Input.GetKey("w")) {
// //             move.y += 1;
// //         }
// //         if (Input.GetKey("s")) {
// //             move.y -= 1;
// //         }
// //         if (Input.GetKey("a")) {
// //             move.x -= 1;
// //         }
// //         if (Input.GetKey("d")) {
// //             move.x += 1;
// //         }

// //         // Don't have diagnonals be faster
// //         move = move.normalized * speed;

// //         //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
// //         rb2d.AddForce(move);
// //     }

//     void Flashlight() {
//         // Render our flashlight

//         // TODO FIGURE OUT MORE
//         // TODO will need an array of points - that is the baby
//         // way to do it at least
//         var flashlightStart = rb2d.transform.position;

//         var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         var mouseAngle = Vector3.Angle(flashlightStart, mousePos);

//         // Using the mouse, position the center of the flashlight in that direction
//         var flashlightRay = ((Vector2) (mousePos - flashlightStart)).normalized * flashlightThrow;
//         var flashlightCenter = flashlightStart + ((Vector3) flashlightRay);


//         // How many segments for the flashlight?
//         int segmentCount = 10;

//         // Setup the mesh, knowing we will need the flashlight start, and a point
//         // for each segment tip
//         Vector3[] vertices = new Vector3[segmentCount + 1];
//         vertices[0] = flashlightStart;
//         int[] triangles = new int[segmentCount * 3];

//         // used to figure out far to spread each segment
//         float segmentAngle = flashlightSpread / segmentCount;
//         float leftAngle = -flashlightSpread / 2;

//         // Using trig, get the points just to the left and right of the flashlight's center
//         for (var i=0; i<segmentCount; i++) {
//             var q = Quaternion.AngleAxis(leftAngle + segmentAngle * i, Vector3.forward);
//             var flashlightSegment = flashlightStart + q * flashlightRay * flashlightThrow;
//             // Find where the flashlight hits the wall
//             var flashlightSegmentEnd = Physics2D.Linecast(flashlightStart, flashlightSegment, opaqueLayer);

//             // example for 'triangles' with 2 line segments:
//             // 0, 1, 2, 0, 2, 3
//             vertices[i+1] = flashlightSegmentEnd.point;
//             triangles[i*3 + 1] = i;
//             triangles[i*3 + 2] = i + 1;
//         }
//         flashlightMesh.Clear();
//         flashlightMesh.vertices = vertices;
//         flashlightMesh.triangles = triangles;
//     }
// }
