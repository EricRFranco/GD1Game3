using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour {

	SceneChanger sceneChanger;

	// Use this for initialization
	void Start () {
		sceneChanger = GameObject.Find("SceneManager").GetComponent<SceneChanger>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoBackToStart(){
		sceneChanger.ReturnToStart ();
	}
}
