// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;

// public class PlayerScript : MonoBehaviour {

//     // Player move force
//     private float speed = 50f;

//     // Flashlight drawn for player (based on mouse and surround geometry)
//     private float flashlightThrow = 10f;  // Max distance in units
//     private float flashlightSpread = 40f; // Angle in degrees
//     private Mesh flashlightMesh;
//     private LayerMask opaqueLayer;


//     private Rigidbody2D rb2d;
    
//     private TimeMachineScript tms;

//     void Start() {
//         rb2d = GetComponent<Rigidbody2D>();
//         flashlightMesh = new Mesh();
//         GameObject.Find("Flashlight").GetComponent<MeshFilter>().mesh = flashlightMesh;

//         // The layer of objects that block light
//         opaqueLayer = LayerMask.GetMask("Opaque");

//         tms = GameObject.Find("TimeMachine").GetComponent<TimeMachineScript>();
//     }

//     void FixedUpdate() {
//         Move();
//         Flashlight();
//     }

//     void Move() {
//         // WASD movement, normalized to speed
//         // TODO add arrow keys, controller input, etc
//         var move = new Vector2(0, 0);
//         if (Input.GetKey("w")) {
//             move.y += 1;
//         }
//         if (Input.GetKey("s")) {
//             move.y -= 1;
//         }
//         if (Input.GetKey("a")) {
//             move.x -= 1;
//         }
//         if (Input.GetKey("d")) {
//             move.x += 1;
//         }

//         // Don't have diagnonals be faster
//         move = move.normalized * speed;

//         //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
//         rb2d.AddForce(move);
//     }

//     void Flashlight() {
//         // Render our flashlight

//         // TODO FIGURE OUT MORE
//         // TODO will need an array of points - that is the baby
//         // way to do it at least
//         var flashlightStart = rb2d.transform.position;

//         var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         var mouseAngle = Vector3.Angle(flashlightStart, mousePos);

//         // Get the corners of our camera
//         // TODO could use screen to world point, but why
//         var topLeft = rb2d.transform.position - new Vector3(-10, 10, 0);
//         var topRight = rb2d.transform.position - new Vector3(10, 10, 0);
//         var bottomLeft = rb2d.transform.position - new Vector3(-10, 10, 0);
//         var bottomRight = rb2d.transform.position - new Vector3(10, 10, 0);

//         // Using the mouse, position the center of the flashlight in that direction
//         var flashlightRay = ((Vector2) (mousePos - flashlightStart)).normalized * flashlightThrow;
//         var flashlightCenter = flashlightStart + ((Vector3) flashlightRay);


//         // How many segments for the flashlight?
//         int segmentCount = 10;

//         // Setup the mesh, knowing we will need the flashlight start, the 4 corners of the camera
//         // and a point for each segment tip
//         Vector3[] vertices = new Vector3[segmentCount + 5];
//         vertices[0] = flashlightStart;
//         vertices[1] = topLeft;
//         vertices[2] = topRight;
//         vertices[3] = bottomRight;
//         vertices[4] = bottomLeft;
//         List<int> triangles = new List<int>();

//         // used to figure out far to spread each segment
//         float segmentAngle = flashlightSpread / segmentCount;
//         float leftAngle = -flashlightSpread / 2;

//         // Using trig, get the points just to the left and right of the flashlight's center
//         for (var i=0; i<segmentCount; i++) {
//             var q = Quaternion.AngleAxis(leftAngle + segmentAngle * i, Vector3.forward);
//             var flashlightSegment = flashlightStart + q * flashlightRay * flashlightThrow;
//             // Find where the flashlight hits the wall
//             var flashlightSegmentEnd = Physics2D.Linecast(flashlightStart, flashlightSegment, opaqueLayer);

//             vertices[i+5] = flashlightSegmentEnd.point;

//             // This segment will connect to its neighbor,
//             // and then the nearest corner
//             // The last one goes back to the person, rather than to the next one
//             if (i<segmentCount-1) {
//                 var ClosestCorner = GetClosestCorner(flashlightSegmentEnd.point);
//                 triangles.Add(ClosestCorner);
//                 triangles.Add(i+5);
//                 triangles.Add(i+5+1);
//             }
//         }

//         // Add in the triangles that go to the first segment
//         var firstCorner = GetClosestCorner(vertices[5]);
//         triangles.Add(firstCorner);
//         triangles.Add((firstCorner + 5 - 1) % 4 + 1);
//         triangles.Add(5);

//         // TODO last corner

//         // Add in the corners between the flashlight start
//         triangles.Add(0);
//         triangles.Add((firstCorner + 5 - 2) % 4 + 1);
//         triangles.Add((firstCorner + 5 - 1) % 4 + 1);
//         triangles.Add(0);
//         triangles.Add((firstCorner + 5 - 1) % 4+ 1);
//         triangles.Add(5);

//         foreach (var t in triangles) {
//             Debug.Log("  " + t);
//         }

//         for (var i=0; i<vertices.Length; i++) {
//             Debug.Log("  " + i + ": " + vertices[i]);
//         }

//         flashlightMesh.Clear();
//         flashlightMesh.vertices = vertices;
//         flashlightMesh.triangles = triangles.ToArray();
//     }

//     int GetClosestCorner(Vector3 pos) {
//         // Returns the INDEX of the closet corner, remember:
//         // 0 = flashlight start
//         // 1 = top left
//         // 2 = top right, etc
//         var flashlightStart = rb2d.transform.position;
//         var topLeft = rb2d.transform.position - new Vector3(-10, 10, 0);
//         var topRight = rb2d.transform.position - new Vector3(10, 10, 0);
//         var bottomLeft = rb2d.transform.position - new Vector3(-10, 10, 0);
//         var bottomRight = rb2d.transform.position - new Vector3(10, 10, 0);

//         float[] distances = new float[] {
//             float.PositiveInfinity,
//             Vector3.Distance(pos, topLeft),
//             Vector3.Distance(pos, topRight),
//             Vector3.Distance(pos, bottomRight),
//             Vector3.Distance(pos, bottomLeft),
//         };

//         return Array.IndexOf(distances, Mathf.Min(distances));
//     }
// }
