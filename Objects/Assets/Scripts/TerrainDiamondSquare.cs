using UnityEngine;
using System.Collections;

public class TerrainDiamondSquare : MonoBehaviour {

	// http://stackoverflow.com/questions/2755750/diamond-square-algorithm

	private Terrain terrain;
	public int size = 513;

	float[,] dataArray;


	void Start() 
	{
		terrain = Ground ();
		DiamondSquareDataArray();
	}

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


		//GameObject _Terrain = Terrain.CreateTerrainGameObject(_TerrainData);

		return _Terrain as Terrain;
	}

	void DiamondSquareDataArray() 
	{
		// declare the data array
		dataArray = new float[ size, size ];

		// set the 4 corners
		dataArray[ 0, 0 ] = 1;
		dataArray[ size - 1, 0 ] = 1;
		dataArray[ 0, size - 1 ] = 1;
		dataArray[ size - 1, size - 1 ] = 1;


		float val ;
		float rnd ;

		float h = 0.5f;

		int sideLength ;
		int x ;
		int y ;

		int halfSide ;


		for ( sideLength = size - 1; sideLength >= 2; sideLength /= 2 )
		{
			halfSide = sideLength / 2;

			// square values
			for ( x = 0; x < size - 1; x += sideLength )
			{
				for ( y = 0; y < size - 1; y += sideLength )
				{
					val = dataArray[ x, y ];
					val += dataArray[ x + sideLength, y ];
					val += dataArray[ x, y + sideLength ];
					val += dataArray[ x + sideLength, y + sideLength ];

					val /= 4.0f;

					// add random
					rnd = ( Random.value * 2.0f * h ) - h;
					val = Mathf.Clamp01( val + rnd );

					dataArray[ x + halfSide, y + halfSide ] = val;
				}
			}

			// diamond values
			for ( x = 0; x < size - 1; x += halfSide )
			{
				for ( y = ( x + halfSide ) % sideLength; y < size - 1; y += sideLength )
				{
					val = dataArray[ ( x - halfSide + size - 1 ) % ( size - 1 ), y ];
					val += dataArray[ ( x + halfSide ) % ( size - 1 ), y ];
					val += dataArray[ x, ( y + halfSide ) % ( size - 1 ) ];
					val += dataArray[ x, ( y - halfSide + size - 1 ) % ( size - 1 ) ];

					val /= 4.0f;

					// add random
					rnd = ( Random.value * 2.0f * h ) - h;
					val = Mathf.Clamp01( val + rnd );

					dataArray[ x, y ] = val;

					if ( x == 0 ) dataArray[ size - 1, y ] = val;
					if ( y == 0 ) dataArray[ x, size - 1 ] = val;
				}
			}


			h /= 2.0f; // cannot include this in for loop (dont know how in uJS)
		}


		Debug.Log( "DiamondSquareDataArray completed" );

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