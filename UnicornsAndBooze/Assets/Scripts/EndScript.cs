﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScript : MonoBehaviour {

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

				sceneChanger.RunCredits ();
				return;
			} else {
				Destroy (currentBackground);
				currentBackground = Instantiate (frameImages [frameIndex]);
			}
		}

		text.text = framesText [frameIndex].stringList [textInFrameIndex];

	}
}
