using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour {
    void Start () {
        var deadMenu = transform.root.Find("DeadMenu/TryAgain").gameObject;
        var deadButton = deadMenu.GetComponent<Button>();
        deadButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick(){
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
}
