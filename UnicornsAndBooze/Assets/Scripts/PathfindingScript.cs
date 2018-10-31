using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingScript : MonoBehaviour {

	LevelManagerScript levelManager;

    List<Vector3> currentPath;

	public Vector3 goalPoint;

	// Use this for initialization
	void Start () {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManagerScript>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
