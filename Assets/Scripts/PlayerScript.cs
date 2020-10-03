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

        // Don't want my flashlight to get me killed
        flashlightThrow = 0f;
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

    public void Kill() {
        // Little kill screen to try again, go to menu
        // TODO MENU
        if (!alive) {
            return;
        }
        alive = false;
        var canvas = GameObject.Find("Canvas");
        canvas.transform.Find("DeadMenu").gameObject.SetActive(true);
        var t = GameObject.Find("DeadText").GetComponent<Text>();
        var tops = new List<string> {
            "Spacetime was torn asunder",
            "The universe imploded",
            "Everybody died",
            "You saw yourself",
            "The world went 'poof!'",
            "You killed time",
            "The cosmos did not like that",
            "You made god cry",
            "Reality was destroyed",
            "Everything became nothing",
            "You became your own grandpa",
            "The earth disentigrated"
        };

        var quotes = new List<string> {
            "“People like us, who believe in physics, know that the distinction between past, present and future is only a stubbornly persistent illusion.” -Albert Einstein.",
            "“Nothing is as far away as one minute ago.” -Jim Bishop.",
            "“Time is precious – spend it wisely”. -Anonymous.",
            "“The past is gone, the future is not come, and the present becomes the past even while we attempt to define it” -Charles Caleb Colton.",
            "“History will be kind to me for I intend to write it.” -Winston Churchill",
            "“The best way to predict the future is to create it” -Peter Drucker",
            "“The bad news is time flies. The good news is you’re the pilot.” -Michael Althsuler",
            "“Memory is the diary we all carry about with us.” -Oscar Wilde",
            "“Choices create circumstances; decisions determine your future” -John Croyle",
            "“The past is a foreign country: they do things differently there.” -L. P. Hartley.",
            "“Time, like an ever-rolling stream, Bears all it’s sons away; they fly forgotten as a dream dies at the opening day.” -Isaac Watts.",
            "“Time is too slow for those who wait, too swift for those who fear, too long for those who grieve” -Henry Van Dyke.",
            "“Time is an illusion. Lunch time doubly so.” -Douglas Adams.",
            "The only reason for time is so that everything doesn’t happen at once.” -Albert Einstein.",
            "“Time is the only thief we can’t get justice against.” -Astrid Alauda.",
        };

        var tips = new List<string> {
            "Avoid the red sightlines",
            "Staring at a wall can buy yourself time later",
            "Flow like water - in one direction",
            "Plan the important parts of your route",
            "Explore for little places to hide",
            "Be swift - but predictable",
            "Try not to make rash, sudden swivels"
        };

        t.text = (
            tops[Random.Range(0, tops.Count)] + "\n\n" +
            quotes[Random.Range(0, quotes.Count)] + "\n\n" +
            "Tip: " + tips[Random.Range(0, tips.Count)]
        );
    }

}
