using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMachineScript : MonoBehaviour {

    List<Vector3> positions = new List<Vector3>();
    List<float> lookAngles = new List<float>();

    void Start() {
        InvokeRepeating("SpawnEnemy", 5f, 5f);
    }

    public void Save(Rigidbody2D rb2d, float lookAngle) {
        // Save this fixed frame to later replay
        positions.Add(rb2d.transform.position);
        lookAngles.Add(lookAngle);
    }

    void SpawnEnemy() {
        Debug.Log("Enemy");
    }
}
