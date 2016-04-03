using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class  Delaunay : MonoBehaviour
{
	public int numPoints = 50;
	public float rangeFromPos = 100f;	// Distance that points may appear from this object’s position

	private List<Triangle> level = new List<Triangle>();

	private List<Vector3> pointToVec = new List<Vector3>(); // So that we work with point numbers instead of Vectors (just easier to debug)

	private List<int> outsiders;	// Points of surrounding tetrahedron, so cells can be easily ignored if they are just scaffolding
	public Material lineMaterial;

	public struct Triangle
	{
		public int[] vertices;

		public bool isSurrounding;

		public Vector3 circCentre;

		public float circSqRad;

		public Triangle( bool surround, int[] bounds, Vector3 theCircCentre, float theCircSqRad )
		{
			if ( bounds.Length > 4 )
				Debug.LogError( "Triangle created with more than 4 vertices" );

			isSurrounding = surround;

			vertices = bounds;

			circSqRad = theCircSqRad;

			circCentre = theCircCentre;
		}
	}

	void Awake()
	{
		level = new List<Triangle>();
		pointToVec = new List<Vector3>();
		outsiders = new List<int>();

		// Find enclosing tetrahedron
		pointToVec.Add( new Vector3( - rangeFromPos * 2f, -rangeFromPos * 2f, -rangeFromPos * 2f ) + transform.position );
		pointToVec.Add( new Vector3( rangeFromPos * 2f, -rangeFromPos * 2f, - rangeFromPos * 2f ) + transform.position );
		pointToVec.Add( new Vector3( 0, -rangeFromPos * 2f, rangeFromPos * 2f ) + transform.position );
		pointToVec.Add( new Vector3( 0, rangeFromPos * 2f, 0 ) + transform.position );

		int[] pointArray = new int[]{ 0, 1, 2, 3 };
		outsiders.AddRange( pointArray );
		level.Add( new Triangle( true, pointArray, FindCentre( pointArray ), FindRad( pointArray ) ) );

		// Generate list of random points to triangulate
		for ( int i = 0; i < numPoints; i++ )
			pointToVec.Add( Random.insideUnitSphere * rangeFromPos );

		for ( int i = outsiders.Count; i < pointToVec.Count - 1; i++ )
			AddPoint( i );

		// DEBUG
		foreach( Triangle t in level )
		{
			Debug.DrawLine( pointToVec[ t.vertices[0] ], pointToVec[ t.vertices[1] ] );
			Debug.DrawLine( pointToVec[ t.vertices[1] ], pointToVec[ t.vertices[2] ] );
			Debug.DrawLine( pointToVec[ t.vertices[2] ], pointToVec[ t.vertices[0] ] );

			Debug.Log (pointToVec[ t.vertices[0] ]);
		}


		Debug.Break();




	}
	private void OnDrawGizmos () 
	{


		//Gizmos.color = Color.black;

		Gizmos.color = new Color(1, 0, 0, 0.5F);
		//Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
		//yield return wait;


		for (int i = 0; i < pointToVec.Count; i++) {
			Gizmos.DrawSphere(pointToVec[i], 1f);
		}
		foreach( Triangle t in level )
		{
			
			Vector3 pointPos1 = pointToVec[ t.vertices[0]];
			Vector3 pointPos2 = pointToVec[ t.vertices[1]];
			Vector3 pointPos3 = pointToVec[ t.vertices[2]];

			lineMaterial.SetPass (0);
			GL.Begin (GL.LINES);
			GL.Color (Color.blue);
			GL.Vertex3 (pointPos1.x, pointPos1.y, pointPos1.z);
			GL.Vertex3 (pointPos2.x, pointPos2.y, pointPos2.z);
			GL.Vertex3 (pointPos2.x, pointPos2.y, pointPos2.z);
			GL.Vertex3 (pointPos3.x, pointPos3.y, pointPos3.z);
			GL.Vertex3 (pointPos3.x, pointPos3.y, pointPos3.z);
			GL.Vertex3 (pointPos1.x, pointPos1.y, pointPos1.z);
			GL.End ();


			//Debug.Log (pointToVec[ t.vertices[0] ]);
		}


	}

	// Call for each vertex (any order)
	void AddPoint( int point )
	{
		List< List< int > > faces = new List< List<int> >();

		// Find what cell(s) point is inside
		foreach ( Triangle triangleToDel in level.FindAll( delegate( Triangle c )
			{
				return ( ( pointToVec[point] - c.circCentre ).magnitude < c.circSqRad );
			}	)	)
		{
			// Delete this cell from level
			level.Remove( triangleToDel );
			Debug.Log( "Deleted Triangle: " + triangleToDel.vertices[0] + " " + triangleToDel.vertices[1] + " " + triangleToDel.vertices[2] + " " + triangleToDel.vertices[3] );

			// Add faces from deleted cell, delete any that are duplicates
			foreach ( List<int> face in FindFaces( triangleToDel.vertices ) )
			{
				List<int> faceToDel = faces.Find( delegate( List<int> f )
					{
						return ( f[0] == face[0] && f[1] == face[1] && f[2] == face[2] );
					} );

				if ( faceToDel != null )
					faces.Remove( faceToDel );
				else
					faces.Add( face );
			}
		}

		// Create cells from each of the faces surrounding the space to the new point
		foreach ( List< int > face in faces )
		{
			face.Add( point );
			bool isSurround = false;
			if ( face.Exists( delegate( int i )
				{
					return outsiders.Contains( i );
				} ) )
				isSurround = true;

			// Make cell
			int[] iFace = face.ToArray();;
			level.Add( new Triangle( isSurround, iFace, FindCentre( iFace ), FindRad( iFace ) ) );
			//	Debug.Log( "Triangle: " + newTriangle.vertices[0] + " " + newTriangle.vertices[1] + " " + newTriangle.vertices[2] + " " + newTriangle.vertices[3] );
			//	level.Add( new Triangle( isSurround, true, iFace, FindCentre( iFace ), FindRad( iFace ) ) );
		}
	}

	List<int>[] FindFaces( int[] vertices)
	{
		if ( vertices.Length > 4 )
			Debug.LogError( "Combine given more than 4 vertices" );

		List< int >[] retList = new List<int>[ 4 ];

		List< int > face1 = new List<int>( 3 );
		face1.Add( vertices[0] );
		face1.Add( vertices[1] );
		face1.Add( vertices[2] );
		face1.Sort( );
		retList[0] = face1;

		List< int > face2 = new List<int>( 3 );
		face2.Add( vertices[0] );
		face2.Add( vertices[1] );
		face2.Add( vertices[3] );
		face2.Sort( );
		retList[1] = face2;

		List< int > face3 = new List<int>( 3 );
		face3.Add( vertices[0] );
		face3.Add( vertices[2] );
		face3.Add( vertices[3] );
		face3.Sort( );
		retList[2] = face3;

		List< int > face4 = new List<int>( 3 );
		face4.Add( vertices[1] );
		face4.Add( vertices[2] );
		face4.Add( vertices[3] );
		face4.Sort( );
		retList[3] = face4;

		return retList;
	}

	// Calc a tetra/cell’s centre and radius
	private Vector3 FindCentre( int[] vertInd )
	{
		if ( vertInd.Length != 4 )
			Debug.LogError( "Not valid cell" );

		Vector3[] vertices = GetVectors( vertInd );
		return vertices[0] + ( ( vertices[3] - vertices[0]).sqrMagnitude * Vector3.Cross( vertices[1] - vertices[0], vertices[2] - vertices[0] )
			+ ( vertices[2] - vertices[0] ).sqrMagnitude * Vector3.Cross( vertices[3] - vertices[0], vertices[1] - vertices[0] )
			+ ( vertices[1] - vertices[0] ).sqrMagnitude * Vector3.Cross( vertices[2] - vertices[0], vertices[3] - vertices[0] ) )
			/ ( 2f * Vector3.Dot( vertices[1] - vertices[0], Vector3.Cross( vertices[2] - vertices[0], vertices[3] - vertices[0] ) ) );
	}

	private float FindRad( int[] vertInd )
	{
		if ( vertInd.Length != 4 )
			Debug.LogError( "Not valid cell" );

		Vector3[] vertices = GetVectors( vertInd );

		return ( ( vertices[3] - vertices[0] ).sqrMagnitude * Vector3.Cross( vertices[1] - vertices[0], vertices[2] - vertices[0] )
			+ ( vertices[2] - vertices[0] ).sqrMagnitude * Vector3.Cross( vertices[3] - vertices[0], vertices[1] - vertices[0] )
			+ ( vertices[1] - vertices[0] ).sqrMagnitude * Vector3.Cross( vertices[2] - vertices[0], vertices[3] - vertices[0] ) ).magnitude / ( 2f * Mathf.Abs( Vector3.Dot( vertices[1] - vertices[0], Vector3.Cross( vertices[2] - vertices[0], vertices[3] - vertices[0] ) ) ) );
	}

	private Vector3[] GetVectors( int[] verts )
	{
		Vector3[] toReturn = new Vector3[verts.Length];

		for( int i = 0; i < verts.Length; i++ )
			toReturn[i] = pointToVec[ verts[i] ];

		return toReturn;
	}
}