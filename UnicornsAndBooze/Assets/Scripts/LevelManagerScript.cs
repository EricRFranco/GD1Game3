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

	public void Clear(){
		nodeToNodesGraph.Clear ();
	}
}


public struct Tile {
	public bool isWall;
	public Vector3 position;


}

public class LevelManagerScript : MonoBehaviour {
	
	[System.NonSerialized]
	public Graph graph;

	[System.NonSerialized]
	public Tile[][] levelGrid;

	// Use this for initialization
	void Start () {
		graph = new Graph ();
        UpdateGraph();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	Vector3 GetClosestTilePositionFromGrid(Vector3 position) {

		Vector3 closestCoord = Vector3.zero;
		float closestDistance = float.MaxValue;

		foreach (Tile[] tileRow in levelGrid) {
			foreach (Tile tile in tileRow) {
				float distance = Vector3.Distance (position, tile.position);
				if (distance < closestDistance) {
					closestCoord = tile.position;
					closestDistance = distance;
				}
			}
		}
		return closestCoord;

	}

	public void UpdateGraph(){
        graph.Clear();
		for (int i = 0; i < levelGrid.Length; ++i) {
			for (int j = 0; j < levelGrid [i].Length; ++j) {
				List<Vector3> connectedPositions = new List<Vector3> ();
				bool up = false;
				bool down = false;
				bool right = false;
				bool left = false;
				if (i > 0 && !levelGrid [i - 1] [j].isWall) {
					connectedPositions.Add (levelGrid[i - 1][j].position);
					up = true;
				}
				if (i < levelGrid.Length - 1 && !levelGrid [i + 1] [j].isWall) {
					connectedPositions.Add (levelGrid[i + 1][j].position);
					down = true;
				}
				if (j > 0 && !levelGrid [i] [j - 1].isWall) {
					connectedPositions.Add (levelGrid[i][j - 1].position);
					left = true;
				}
				if (j < levelGrid[i].Length - 1 && !levelGrid [i] [j + 1].isWall) {
					connectedPositions.Add (levelGrid[i][j + 1].position);
					right = true;
				}

				if (up && left && !levelGrid [i - 1] [j - 1].isWall) {
					connectedPositions.Add (levelGrid[i - 1][j - 1].position);
				}

				if (up && right && !levelGrid [i - 1] [j + 1].isWall) {
					connectedPositions.Add (levelGrid[i - 1][j + 1].position);
				}

				if (down && left && !levelGrid [i + 1] [j - 1].isWall) {
					connectedPositions.Add (levelGrid[i + 1][j - 1].position);
				}

				if (down && right && !levelGrid [i + 1] [j + 1].isWall) {
					connectedPositions.Add (levelGrid[i + 1][j + 1].position);
				}

				graph.AddNode (levelGrid [i] [j].position, connectedPositions);
			}
		}
	}

    public List<Vector3> PathFind(Vector3 startPoint, Vector3 endPoint) {
       

        Vector3 startTilePosition = GetClosestTilePositionFromGrid(startPoint);
        Vector3 endTilePosition = GetClosestTilePositionFromGrid(endPoint);

		List<Vector3> path = BFS(graph, startTilePosition, endTilePosition);

		path [0] = startPoint;
		path [path.Count - 1] = endPoint;

        return path;
    }


    public static List<Vector3> BFS(Graph graph, Vector3 startNode, Vector3 endNode) {
        Queue<List<Vector3>> pathQueue = new Queue<List<Vector3>>();
        HashSet<Vector3> nodesVisited = new HashSet<Vector3>();
        List<Vector3> initPath = new List<Vector3>();
        initPath.Add(startNode);
        pathQueue.Enqueue(initPath);
		nodesVisited.Add (startNode);
        while(pathQueue.Count != 0) {
            
			List<Vector3> currentPath = pathQueue.Dequeue ();

			Vector3 currentNode = currentPath [currentPath.Count - 1];

			if (currentNode == endNode) {
				return currentPath;
			}

			List<Vector3> neighbors = graph.GetConnectedNodes (currentNode);

			foreach (Vector3 neighbor in neighbors) {
				if (!nodesVisited.Contains (neighbor)) {
					List<Vector3> newPath = new List<Vector3> (currentPath);
					newPath.Add (neighbor);
					pathQueue.Enqueue (newPath);
					nodesVisited.Add (neighbor);
				}
			}

        }

		return null;

    }
}
