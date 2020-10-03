using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToyScript : MonoBehaviour {

    public GameObject hole;
    public bool set = false; // if toy has been set in correct hole

    public static float snapDistance =  0.1f;

    string pickupMessage = "Press space to put it down";

    void Start() {
        transform.parent = GameObject.Find("Toys").transform;
        hole.transform.parent = GameObject.Find("Toys").transform;
    }

    void OnTriggerEnter2D(Collider2D other) {
        var colliderName = other.gameObject.name;
        
        if (set) {
            return;
        }

        // Set us in our hole! Yay!
        if (other.gameObject == hole) {
            set = true;
            transform.position = hole.transform.position;
            GetComponent<Rigidbody2D>().velocity = new Vector2();
            Debug.Log(gameObject);
            var tbs = transform.parent.gameObject.GetComponent<ToyboxScript>();
            tbs.toysSet += 1;
            GameObject.Find("ToyText").GetComponent<Text>().text = tbs.toysSet + " / " + tbs.totalToys;
            return;
        }

        // Have the player pick us up when they run into us
        if (!colliderName.Contains("Player")) {
            return;
        }
        GameObject player = other.gameObject;
        PlayerScript ps = player.GetComponent<PlayerScript>();
        if (ps.heldObject != null) {
            return;
        }   
        ps.heldObject = gameObject;

        // To keep us connected
        var joint = gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
        joint.connectedBody = ps.rb2d;
        joint.dampingRatio = 10;
        joint.distance = 0;

        ps.DisplayHint(pickupMessage);
    }
}
