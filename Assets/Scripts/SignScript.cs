using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignScript : MonoBehaviour {
    public string message;

    void OnTriggerEnter2D(Collider2D other) {
        // Display message to player
        Debug.Log(other);
        var colliderName = other.gameObject.name;
        if (colliderName.Contains("Player")) {
            GameObject.Find("MessageText").GetComponent<Text>().text = message;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        // Remove message
        var colliderName = other.gameObject.name;
        if (colliderName.Contains("Player")) {
            GameObject.Find("MessageText").GetComponent<Text>().text = "";
        }
    }
}
