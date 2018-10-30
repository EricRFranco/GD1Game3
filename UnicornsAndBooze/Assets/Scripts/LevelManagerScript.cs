using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {
	Hashtable nodeToNodesGraph;
	public Graph() {
		nodeToNodesGraph = new Hashtable ();
	}

	public void AddNode(Vector3 nodePosition, List<Vector3> connectedNodes){
		nodeToNodesGraph.Add (nodePosition, connectedNodes);
	}

	public List<Vector3> GetConnectedNodes(Vector3 nodePosition){
		return (List<Vector3>)nodeToNodesGraph[nodePosition];
	}
}

public class LevelManagerScript : MonoBehaviour {

	[System.NonSerialized]
	public Graph graph;

	[System.NonSerialized]
	public bool[][] levelLayout;

	// Use this for initialization
	void Start () {
		print (levelLayout);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void UpdateGraph(){
		/*List<Vector3> connectedPositions = new List<Vector3> ();
		bool up = false;
		bool down = false;
		bool right = false;
		bool left = true;
		if (i > 0 && levelRows [i - 1] [j] != 'W') {
			connectedPositions.Add (new Vector3 (placePosition.x, placePosition.y, placePosition.z - zOffset));
			up = true;
		}
		if (i < levelRows.Length - 2 && levelRows [i + 1] [j] != 'W') {
			connectedPositions.Add (new Vector3 (placePosition.x, placePosition.y, placePosition.z + zOffset));
			down = true;
		}
		if (j > 0 && levelRows [i] [j - 1] != 'W') {
			connectedPositions.Add (new Vector3 (placePosition.x - xOffset, placePosition.y, placePosition.z));
			left = true;
		}
		if (j < levelRows[i].Length - 1 && levelRows [i] [j + 1] != 'W') {
			connectedPositions.Add (new Vector3 (placePosition.x + xOffset, placePosition.y, placePosition.z));
			right = true;
		}

		if (up && left && levelRows [i - 1] [j - 1] != 'W') {

		}*/
	}
}
