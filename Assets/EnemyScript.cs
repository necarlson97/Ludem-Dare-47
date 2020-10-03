using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemyScript : PersonScript {

    private int age = 0; // Number of fixed updates alive

    // Start is called before the first frame update
    void Start() {
        SetFlashlight();
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.position = tms.positions[age];
        lightingObject.transform.eulerAngles = new Vector3(0, 0, tms.lookAngles[age] - 90);
        age += 1;

        UpdateFlashlight();
    }
}
