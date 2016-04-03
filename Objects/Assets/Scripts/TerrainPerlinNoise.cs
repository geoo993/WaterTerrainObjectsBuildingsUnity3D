using UnityEngine;
using System.Collections;


public class TerrainPerlinNoise : MonoBehaviour {


	Terrain Ground ()
	{
		GameObject t = new GameObject("TerrainObject");
		TerrainData _TerrainData = new TerrainData();

		this.name = "terrain";
		//this.tag = "Player";
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

		return _Terrain as Terrain;
	}

	public Terrain terrain ;
	public int size = 513;

	public Vector2 offset = Vector2.zero; // where does the sample start?
	public int octaves = 8; // how many iteration for fractal?
	public float frq = 100.0f; // how smooth/rough? (low = rough, high = smooth)
	public float amp = 0.5f; // how strong? (0.5 is ideal)

	float[,] dataArray ;

	void Start() 
	{
		terrain = Ground ();
		PerlinFractalDataArray();
	}


	void PerlinFractalDataArray() 
	{
		// declare the data array
		dataArray = new float[ size, size ];

		// variables used in calculations
		float noise ;
		float gain ;

		int x ;
		int y ;
		int i ;

		Vector2 sample;

		// generate noise
		for ( y = 0; y < size; y ++ )
		{
			for ( x = 0; x < size; x ++ )
			{
				noise = 0.0f;
				gain = 1.0f;

				for ( i = 0; i < octaves; i ++ )
				{
					sample.x = offset.x + ( ( x ) * ( gain / frq ) );
					sample.y = offset.y + ( ( y ) * ( gain / frq ) );

					noise += Mathf.PerlinNoise( sample.x, sample.y ) * ( amp / gain );
					gain *= 2.0f;
				}

				dataArray[ x, y ] = noise;
			}
		}


		//Debug.Log( "PerlinFractalDataArray completed" );

		// Generate Terrain using dataArray as height values
		GenerateTerrain();
	}


	void GenerateTerrain() 
	{
		if ( !terrain )
			return;

		if ( terrain.terrainData.heightmapResolution != size )
			terrain.terrainData.heightmapResolution = size;
	
		terrain.terrainData.SetHeights( 0, 0, dataArray );
	}
}
