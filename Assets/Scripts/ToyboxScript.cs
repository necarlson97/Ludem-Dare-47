using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToyboxScript : MonoBehaviour {
    public GameObject toyPrefab;
    public GameObject toyHolePrefab;

    List<GameObject> toys = new List<GameObject>();

    // TODO these are just set by hand
    Vector3 leftGreenRoomCenter = new Vector3(-28, 0, 0);
    Vector3 rightGreenRoomCenter = new Vector3(28, 0, 0);
    Vector3 downGreenRoomCenter = new Vector3(0, -28, 0);

    Vector3 leftOrangeRoomCenter = new Vector3(-28, 25, 0);
    Vector3 RightOrangeRoomCenter = new Vector3(28, 25, 0);

    Vector3 leftRedRoomCenter = new Vector3(-28, 50, 0);
    Vector3 rightRedRoomCenter = new Vector3(28, 50, 0);

    public List<Sprite> toyShapes = new List<Sprite>();

    // Start is called before the first frame update
    void Start() {
        // TODO could shuffle toy shapes
        
        // Easiest, pick up toy, it is near hole
        var t = MakeToy(JitterPos(leftGreenRoomCenter, true), leftGreenRoomCenter);

        // Simple 'puzzel' down at the bottom
        for (int i=0; i<3; i++) {
            Sprite s = toyShapes[i];
            Vector3 pos = downGreenRoomCenter + new Vector3(-3f + (i * 2), -7, 0);
            t = MakeToy(JitterPos(downGreenRoomCenter), pos, new Color(0.23f, 1, 0.45f));
            t.GetComponent<SpriteRenderer>().sprite = s;
            var hole = t.GetComponent<ToyScript>().hole;
            hole.GetComponent<SpriteRenderer>().sprite = s;
        }

        // Sign tells you want to do
        MakeToy(JitterPos(rightGreenRoomCenter), rightGreenRoomCenter + new Vector3(-3f, 6f, 0), new Color(0.32f, 0.44f, 0));
        MakeToy(JitterPos(rightGreenRoomCenter), rightGreenRoomCenter + new Vector3(0, 6.5f, 0), new Color(0.69f, 0.85f, 0.15f));
        MakeToy(JitterPos(rightGreenRoomCenter), rightGreenRoomCenter + new Vector3(3f, 6f, 0), new Color(0.90f, 1f, 0.59f));


        // All of the different shapes in orange
        for (int i=0; i<toyShapes.Count; i++) {
            Sprite s = toyShapes[i];
            Vector3 pos = leftOrangeRoomCenter + new Vector3(-7f, -toyShapes.Count + (i * 2), 0);
            t = MakeToy(JitterPos(RightOrangeRoomCenter), pos, Color.yellow);
            t.GetComponent<SpriteRenderer>().sprite = s;
            var hole = t.GetComponent<ToyScript>().hole;
            hole.GetComponent<SpriteRenderer>().sprite = s;
        }

        // Super easy - but the peice is in an orange room!
        t = MakeToy(JitterPos(RightOrangeRoomCenter), leftRedRoomCenter, Color.red);

        // Sign tells you want to do #2
        // TODO RED COLORS
        MakeToy(JitterPos(rightRedRoomCenter), rightRedRoomCenter + new Vector3(-3f, 6f, 0), new Color(1, 0.54f, 87));
        MakeToy(JitterPos(rightRedRoomCenter), rightRedRoomCenter + new Vector3(0, 6.5f, 0), new Color(0.62f, 0.11f, 0.47f));
        MakeToy(JitterPos(rightRedRoomCenter), rightRedRoomCenter + new Vector3(3f, 6f, 0), new Color(0.25f, 0, 0.18f));
    }

    GameObject MakeToy(Vector3 pos, Vector3 holePos, Color? c=null) {
        var col = (Color) (c ?? Color.green);
        var t1 = Instantiate(toyPrefab, pos, Quaternion.identity);
        t1.GetComponent<ToyScript>().hole = Instantiate(toyHolePrefab, holePos, Quaternion.identity);
        t1.GetComponent<SpriteRenderer>().color = col;
        toys.Add(t1);   
        return t1;
    }

    Vector3 JitterPos(Vector3 pos, bool avoidCenter=false) {
        // Just add a bit of randomness to a position

        var x = Random.Range(-5f, 5f);
        var y = Random.Range(-5f, 5f);

        // Dont have it too close to center
        if (avoidCenter) {
            x = Random.Range(4f, 8f);
            if (Random.value > 0.5f) {
                x = -x;
            }
            y = Random.Range(4f, 8f);
            if (Random.value > 0.5f) {
                y = -y;
            }
        }
        
        return pos + new Vector3(x, y, 0);
    }

    
    public void CheckToys() {
        // We just put something down, update text and
        // see if we won

        int set = 0;
        foreach (GameObject toy in toys) {
            ToyScript ts = toy.GetComponent<ToyScript>();
            if (ts.set) {
                set += 1;
            }
        }
        var t = GameObject.Find("ToyText").GetComponent<Text>();
        t.text = set + " / " + toys.Count;

        if (set == toys.Count) {
            t.color = Color.green;
        }
    }

    public void UnlockDoor(int doorName) {
        // TODO create flash of light, and unlocking sound
        // GameObject.Find(doorName);
    }
}
