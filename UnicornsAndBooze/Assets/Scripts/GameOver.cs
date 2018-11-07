using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    private SceneChanger sceneChanger;

    private void Start()
    {
        sceneChanger = GameObject.Find("SceneManager").GetComponent<SceneChanger>();
    }

    public void Restart()
    {
        sceneChanger.RestartGame();
    }
}
