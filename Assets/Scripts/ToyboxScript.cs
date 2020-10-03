using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyboxScript : MonoBehaviour {
    public GameObject toyPrefab;
    public GameObject toyHolePrefab;

    List<GameObject> toys = new List<GameObject>();

    // TODO these are just set by hand
    Vector3 leftGreenRoomCenter = new Vector3(-23, 0, 0);
    Vector3 rightGreenRoomCenter = new Vector3(23, 0, 0);
    Vector3 downGreenRoomCenter = new Vector3(0, -21, 0);

    Vector3 leftOrangeRoomCenter = new Vector3(-23, 26, 0);
    Vector3 RightOrangeRoomCenter = new Vector3(23, 26, 0);

    Vector3 leftRedRoomCenter = new Vector3(-23, 52, 0);
    Vector3 rightRedRoomCenter = new Vector3(23, 52, 0);

    public List<Sprite> greenShards = new List<Sprite>();
    public Sprite greenShardHole;

    public int totalToys = 7;
    public int toysSet = 0;

    // Start is called before the first frame update
    void Start() {
        // TODO place the toys more randomly
        
        // Easiest, pick up toy, it is near hole
        MakeToy(JitterPos(leftGreenRoomCenter), leftGreenRoomCenter);

        // Simple 'puzzel' down at the bottom
        foreach (Sprite s in greenShards) {
            var t = MakeToy(JitterPos(downGreenRoomCenter), downGreenRoomCenter, new Color(0.23f, 1, 0.45f));
            t.GetComponent<SpriteRenderer>().sprite = s;
            t.GetComponent<ToyScript>().hole.GetComponent<SpriteRenderer>().sprite = greenShardHole;
        }

        // Sign tells you want to do
        MakeToy(JitterPos(rightGreenRoomCenter), rightGreenRoomCenter - new Vector3(-3f, -6f, 0), new Color(0.32f, 0.44f, 0));
        MakeToy(JitterPos(rightGreenRoomCenter), rightGreenRoomCenter - new Vector3(0, -6.5f, 0), new Color(0.69f, 0.85f, 0.15f));
        MakeToy(JitterPos(rightGreenRoomCenter), rightGreenRoomCenter - new Vector3(3f, -6f, 0), new Color(0.90f, 1f, 0.59f));
    }

    GameObject MakeToy(Vector3 pos, Vector3 holePos, Color? c=null) {
        var col = (Color) (c ?? Color.green);
        var t1 = Instantiate(toyPrefab, pos, Quaternion.identity);
        t1.GetComponent<ToyScript>().hole = Instantiate(toyHolePrefab, holePos, Quaternion.identity);
        t1.GetComponent<SpriteRenderer>().color = col;
        toys.Add(t1);   
        return t1;
    }

    Vector3 JitterPos(Vector3 pos) {
        // Just add a bit of randomness to a position

        // Dont have it too close to center
        var x = Random.Range(2f, 5f);
        if (Random.value > 0.5f) {
            x = -x;
        }
        var y = Random.Range(2f, 5f);
        if (Random.value > 0.5f) {
            y = -y;
        }
        return pos + new Vector3(x, y, 0);
    }

    
    public void CheckConditions() {
        // We just put something down, see if a door unlocks

        // TODO
        foreach (GameObject toy in toys) {
            ToyScript ts = toy.GetComponent<ToyScript>();
        }
    }

    public void UnlockDoor(int doorName) {
        // TODO create flash of light, and unlocking sound
        // GameObject.Find(doorName);
    }
}
