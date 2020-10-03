using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyboxScript : MonoBehaviour {
    public GameObject toyPrefab;
    public GameObject toyHolePrefab;

    List<GameObject> toys;

    // Start is called before the first frame update
    void Start() {
        // TODO place the toys more randomly
        
        // Easiest, pick up toy, it is near hole
        t1 = Instantiate(toyPrefab)
        t1.hol
        toys.Add(t1);
    }

    
    public void CheckConditions() {
        // We just put something down, see if a door unlocks

        foreach (GameObject toy in toys) {
            ToyScript ts = toy.GetComponent<ToyScript>()
        }

        
    }

    public void UnlockDoor(int doorName) {
        // TODO create flash of light, and unlocking sound
        GameObject.Find(doorName);
    }
}
