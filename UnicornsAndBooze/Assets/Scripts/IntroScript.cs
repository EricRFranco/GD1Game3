using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct StringList{
	public List<string> stringList;
}

public class IntroScript : MonoBehaviour {


	public List<StringList> framesText;
	public List<GameObject> frameImages;

	int frameIndex;
	int textInFrameIndex;

	SceneChanger sceneChanger;

	GameObject currentBackground;
	Text text;


	// Use this for initialization
	void Start () {
		frameIndex = 0;
		textInFrameIndex = 0;
		text = GetComponentInChildren<Text> ();
		sceneChanger = GameObject.Find("SceneManager").GetComponent<SceneChanger>();
		currentBackground = Instantiate(frameImages [0]);
		text.text = framesText [0].stringList [0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void IncrementTextInFrame(){

		++textInFrameIndex;
		if (textInFrameIndex == framesText [frameIndex].stringList.Count) {
			++frameIndex;
			textInFrameIndex = 0;
			if (frameIndex == framesText.Count) {
				
				sceneChanger.StartGame ();
				return;
			} else {
				Destroy (currentBackground);
				currentBackground = Instantiate (frameImages [frameIndex]);
			}
		}
	
		text.text = framesText [frameIndex].stringList [textInFrameIndex];
	
	}
}
