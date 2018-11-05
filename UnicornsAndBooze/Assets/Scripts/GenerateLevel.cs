using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour {
	public TextAsset level;
	public GameObject wallPrefab;

	public GameObject floorPrefab;

	public GameObject shirtBinPrefab;

	public GameObject boomboxPrefab;

	public Vector3 levelCenter;

	string[] levelRows;

	LevelManagerScript levelManager;
	// Use this for initialization
	void Awake () {
        
		levelManager = GetComponent<LevelManagerScript> ();
		levelRows = level.text.Split(new char[]{'\n'});
		Bounds wallBounds = wallPrefab.GetComponent<MeshFilter> ().sharedMesh.bounds;
		float wallLengthInMeters = wallBounds.extents.x * 2f * wallPrefab.transform.localScale.x;
		float wallHalfHeightInMeters = wallBounds.extents.y * wallPrefab.transform.localScale.y;
		float wallWidthInMeters = wallBounds.extents.z * 2f * wallPrefab.transform.localScale.z;
		Bounds floorBounds = floorPrefab.GetComponent<MeshFilter> ().sharedMesh.bounds;
		float floorLengthInMeters = floorBounds.extents.x * 2f * floorPrefab.transform.localScale.x;
		float floorWidthInMeters = floorBounds.extents.z * 2f * floorPrefab.transform.localScale.z;

		GameObject floor = Instantiate (floorPrefab);
		floor.transform.position = levelCenter;

		float zOffset = floorWidthInMeters / (float)(levelRows.Length - 1);

		levelManager.levelGrid = new Tile[levelRows.Length - 1][];

		Vector3 topLeftCorner = new Vector3 (levelCenter.x - (floorLengthInMeters / 2f), levelCenter.y + wallHalfHeightInMeters, levelCenter.z - (floorWidthInMeters / 2));

		for (int i = 0; i < levelRows.Length - 1; ++i) {
			levelRows [i] = levelRows [i].Trim ();
			float xOffset = floorLengthInMeters / (float)(levelRows [i].Length);

			levelManager.levelGrid [i] = new Tile[levelRows [i].Length];

			for(int j = 0; j < levelRows[i].Length; ++j) {

				Vector3 placePosition = new Vector3 (topLeftCorner.x + j * xOffset + wallLengthInMeters / 2f, 
					                        topLeftCorner.y, topLeftCorner.z + i * zOffset + wallWidthInMeters / 2f);

				Tile tile = new Tile ();
				tile.position = placePosition;
				tile.position.y = 0.5f;
				if (levelRows [i] [j] == 'W') {
					GameObject wall = Instantiate (wallPrefab);
					wall.transform.position = placePosition;
					tile.isWall = true;
				} else {
					
					tile.isWall = false;
				}

				if (levelRows [i] [j] == 'S') {
					GameObject shirtBin = Instantiate (shirtBinPrefab);
					shirtBin.transform.position = placePosition;
				} else if (levelRows [i] [j] == 'B') {
					GameObject boombox = Instantiate (boomboxPrefab);
					boombox.transform.position = placePosition;
				}
			

				levelManager.levelGrid [i] [j] = tile;
			}
		}
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
