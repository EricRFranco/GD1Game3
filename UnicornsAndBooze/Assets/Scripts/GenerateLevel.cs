using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour {
	public TextAsset level;
	public GameObject wallPrefab;

	public GameObject floorPrefab;

	public Vector3 levelCenter;

	string[] levelRows;

	LevelManagerScript levelManager;
	// Use this for initialization
	void Awake () {
		levelManager = GetComponent<LevelManagerScript> ();
		levelRows = level.text.Split(new char[]{'\n'});
		Bounds wallBounds = wallPrefab.GetComponent<MeshFilter> ().sharedMesh.bounds;
		float wallLengthInMeters = wallBounds.extents.x * 2f;
		float wallHalfHeightInMeters = wallBounds.extents.y;
		float wallWidthInMeters = wallBounds.extents.z * 2f;
		Bounds floorBounds = floorPrefab.GetComponent<MeshFilter> ().sharedMesh.bounds;
		float floorLengthInMeters = floorBounds.extents.x * 2f;
		float floorWidthInMeters = floorBounds.extents.z * 2f;

		GameObject floor = Instantiate (floorPrefab);
		floor.transform.position = levelCenter;

		float zOffset = floorWidthInMeters / (float)(levelRows.Length - 1);

		levelManager.levelLayout = new bool[levelRows.Length - 1][];

		Vector3 topLeftCorner = new Vector3 (levelCenter.x - (floorLengthInMeters / 2f), levelCenter.y + wallHalfHeightInMeters, levelCenter.z - (floorWidthInMeters / 2));

		for (int i = 0; i < levelRows.Length - 1; ++i) {
			levelRows [i] = levelRows [i].Trim ();
			float xOffset = floorLengthInMeters / (float)(levelRows [i].Length);

			levelManager.levelLayout [i] = new bool[levelRows [i].Length];
			for(int j = 0; j < levelRows[i].Length; ++j) {

				Vector3 placePosition = new Vector3 (topLeftCorner.x + j * xOffset + wallLengthInMeters / 2f, 
					                        topLeftCorner.y, topLeftCorner.z + i * zOffset + wallWidthInMeters / 2f);

				if (levelRows [i] [j] == 'W') {
					GameObject wall = Instantiate (wallPrefab, floor.transform);
					wall.transform.position = placePosition;
					levelManager.levelLayout [i] [j] = true;
				} else {
					
					levelManager.levelLayout [i] [j] = false;
				}
					
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
