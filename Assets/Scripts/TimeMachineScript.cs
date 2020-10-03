using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMachineScript : MonoBehaviour {

    public List<Vector3> positions = new List<Vector3>();
    public List<float> lookAngles = new List<float>();

    public GameObject enemyPrefab;
    GameObject timerText;

    // How long between spawining?
    float delay = 20f;
    float nextDelay;

    int maxEnemies = 10;
    int enemies = 0;

    public AudioClip slowSong;
    public AudioClip fastSong;

    void Start() {
        timerText = GameObject.Find("TimerText");
        InvokeRepeating("SpawnEnemy", delay, delay);
        InvokeRepeating("SetTimer", 0f, 1f);
        nextDelay = delay;

        // Start with slow song, but after that, loop fast versionb
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = slowSong;
        audioSource.Play();
        // Invoke("PlayFastSong", audioSource.clip.length);
    }

    public void PlayFastSong() {
        var audioSource = GetComponent<AudioSource>();
        audioSource.Stop(); // just in case
        audioSource.clip = fastSong;
        audioSource.Play();
        audioSource.loop = true;
    }

    public void Save(Rigidbody2D rb2d, float lookAngle) {
        // Save this fixed frame to later replay
        positions.Add(rb2d.transform.position);
        lookAngles.Add(lookAngle);
    }

    void SpawnEnemy() {
        if (enemies >= maxEnemies) {
            return;
        }
        Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        nextDelay = delay;
        enemies += 1;
    }

    void SetTimer() {
        var t = timerText.GetComponent<Text>();
        if (nextDelay < 5) {
            t.color = Color.red;
        } else {
            t.color = Color.white;
        }
        t.text = "" + nextDelay;
        nextDelay -= 1;
    }
}
