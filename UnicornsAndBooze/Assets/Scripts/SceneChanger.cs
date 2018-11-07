using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    [SerializeField]    //Serialized for debugging purposes
    private int sceneIndex = 2;

    private void Update()
    {
        Debug.Log(sceneIndex);
            
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameOver();
        }
        else if(Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("GameOverScreen");
        }
    }

    public void StartGame()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("LevelOne");
    }

	public void RunIntro(){
		DontDestroyOnLoad (this.gameObject);
		SceneManager.LoadScene ("IntroductionScene");
	}

    public void RestartGame()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(sceneIndex);
    }

    public void NextLevel()
    {
        DontDestroyOnLoad(this.gameObject);
        sceneIndex++;
        SceneManager.LoadScene(sceneIndex);
    }

    public void GameOver()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("GameOverScreen");
    }

	public void ReturnToStart(){
		
		SceneManager.LoadScene ("StartScreen");
	}

	public void RunCredits(){
		DontDestroyOnLoad (this.gameObject);
		SceneManager.LoadScene ("Credits");
	}
}
