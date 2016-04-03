using UnityEngine;
using System.Collections;

public class CreateTerrain : MonoBehaviour {

	void Start ()
	{
		Ground ();
	}


	Terrain Ground ()
	{
		GameObject t = new GameObject("TerrainObject");
		TerrainData _TerrainData = new TerrainData();

		this.name = "terrain";
		t.transform.parent = this.transform;

		_TerrainData.size = new Vector3(10, 600, 10);
		_TerrainData.heightmapResolution = 512;
		_TerrainData.baseMapResolution = 1024;
		_TerrainData.SetDetailResolution(1024, 16);

		int _heightmapWidth = _TerrainData.heightmapWidth;
		int _heightmapHeight = _TerrainData.heightmapHeight;

		TerrainCollider _TerrainCollider = t.AddComponent<TerrainCollider>();
		Terrain _Terrain = t.AddComponent<Terrain>();

		_TerrainCollider.terrainData = _TerrainData;
		_Terrain.terrainData = _TerrainData;


		//GameObject _Terrain = Terrain.CreateTerrainGameObject(_TerrainData);

		return _Terrain as Terrain;
	}

}
