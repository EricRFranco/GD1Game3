using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PathNode{
	public int tileX;
	public int tileY;
}

[System.Serializable]
public class Path{
	public List<PathNode> pathNodes;

	public Path(){
		pathNodes = new List<PathNode> ();
	}
}

public class PathsListScript : MonoBehaviour {


	public List<Path> paths;
	public GameObject pathfindingNPCPrefab;
    public float pudgyManHeight = -0.5f;
    LevelManagerScript levelManager;
	// Use this for initialization
	void Start () {
        levelManager = GetComponent<LevelManagerScript>();

		foreach (Path path in paths) {
            GameObject pudgy = Instantiate(pathfindingNPCPrefab);
            PathNode firstPoint = path.pathNodes[0];
            pudgy.transform.position = levelManager.levelGrid[firstPoint.tileY][firstPoint.tileX].position;
            pudgy.transform.position = new Vector3(pudgy.transform.position.x, pudgy.transform.position.y - pudgyManHeight, pudgy.transform.position.z);
			PathfindingScript pathfinder = pudgy.GetComponent<PathfindingScript> ();
            pathfinder.enabled = true;
			pathfinder.setOriginalPath (path);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
