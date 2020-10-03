using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerScript : MonoBehaviour {

    // Player move force
    private float speed = 50f;
    bool alive = true;
    private Rigidbody2D rb2d;


    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if (!alive) {
            return;
        }

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

    public void Kill() {
        // TODO render menu
        alive = false;
    }
}
