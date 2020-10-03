using System; // TODO I fucking hate that I can't just import System.Func
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using UnityEngine.UI;

public class PlayerScript : PersonScript {

    // Player move force
    private float speed = 50f;
    bool alive = true;
    public Rigidbody2D rb2d;

    // What object are we currently carring?
    public GameObject heldObject;

    public List<string> heardMessages = new List<string>();

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        SetFlashlight();
    }

    void FixedUpdate() {
        if (!alive) {
            return;
        }

        Move();
        UpdateFlashlight();

        tms.Save(rb2d, GetLookAngle());

        // TODO space bar
        if (Input.GetKey("space")) {
            PutDown();
        }
    }

    public float GetLookAngle() {
        // Get the angle we are looking at based on mouse
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mousePos -= transform.position;
        // Twist light to point twoards mouse
        return Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
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

        // Rotate to look at mouse
        lightingObject.transform.eulerAngles = new Vector3(0, 0, GetLookAngle() - 90);
    }

    public void Kill() {
        // TODO render menu
        alive = false;
        GameObject.Find("Canvas").transform.Find("DeadMenu").gameObject.SetActive(true);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        var colliderName = other.gameObject.name;
        if (colliderName.Contains("Enemy") || colliderName.Contains("Flashlight")) {
            Kill();
        }
    }

    public void PutDown() {
        // Put whatever down if you are holding it
        if (heldObject == null) {
            return;
        }

        heldObject.GetComponent<SpringJoint2D>().breakForce = 0;
        // Toss away, was wrong
        var lookVector = Quaternion.AngleAxis(GetLookAngle(), Vector3.forward) * Vector3.right;
        var toss = lookVector * Random.Range(0, 0.2f);
        heldObject.transform.position = transform.position + lookVector * 2f;
        heldObject.GetComponent<Rigidbody2D>().AddForce(toss);
        heldObject = null;
    }

    public void DisplayHint(string msg) {
        // Display a hint at the bottom - unless we already heard it
        if (!heardMessages.Contains(msg)) {
            GameObject.Find("MessageText").GetComponent<Text>().text = msg;
            heardMessages.Add(msg);
            Invoke("ClearMessage", 3f);
        }
    }

    public void ClearMessage() {
        var t = GameObject.Find("MessageText").GetComponent<Text>();
        // If what was displayed is a hint (not a sign)
        if (heardMessages.Contains(t.text)) {
            t.text = "";
        }
    }
}
