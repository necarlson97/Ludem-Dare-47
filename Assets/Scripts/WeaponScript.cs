using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        var colliderName = other.gameObject.name;
        // Have the player pick us up when they run into us
        if (!colliderName.Contains("Player")) {
            return;
        }
        
        // For now, they just win - hooray!
        SceneManager.LoadScene("EndScene");
    }
}
