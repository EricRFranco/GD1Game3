using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    private string sceneName;

    private void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (Input.GetKeyDown(KeyCode.Y))
        {
            RestartGame();
        }
        else if(Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("GameOverScreen");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("LevelOne");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
