using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (BoxCollider))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (Rigidbody))]


public class Cube : MonoBehaviour {
	
	private Vector3[] vertices;

	void Awake ()
	{
		Cubee ();
	}

	private void OnDrawGizmos () {

		if (vertices == null) {
			return;
		}

		Gizmos.color = Color.black;
		for (int i = 0; i < vertices.Length; i++) {
			Gizmos.DrawSphere(vertices[i] + this.transform.position, 0.1f);
		}

	}

	public void Cubee ()
	{
		
		this.name = "cube";
		this.tag = "Player";

		MeshFilter filter = GetComponent< MeshFilter >();
		Mesh mesh = filter.mesh;
		mesh.Clear();
		
		float length = 0.5f;   

		//Add Vertices region
		Vector3 vertice_0 = new Vector3 (-length, -length, length );
		Vector3 vertice_1 = new Vector3 (length, -length, length );
		Vector3 vertice_2 = new Vector3 (length , -length, -length);
		Vector3 vertice_3 = new Vector3 (-length , -length , -length );    
		Vector3 vertice_4 = new Vector3 (-length, length, length );
		Vector3 vertice_5 = new Vector3 (length, length, length );
		Vector3 vertice_6 = new Vector3 (length , length, -length);
		Vector3 vertice_7 = new Vector3 (-length , length, -length);

		Vector3[] vertices = new Vector3[]
		{
			// Bottom Polygon
			vertice_0, vertice_1, vertice_2, vertice_0,
			// Left Polygon
			vertice_7, vertice_4, vertice_0, vertice_3,
			// Front Polygon
			vertice_4, vertice_5, vertice_1, vertice_0,
			// Back Polygon
			vertice_6, vertice_7, vertice_3, vertice_2,
			// Right Polygon
			vertice_5, vertice_6, vertice_2, vertice_1,
			// Top Polygon
			vertice_7, vertice_6, vertice_5, vertice_4
		} ;
		//End Vertices region

		//Add Normales region
		Vector3 up 	    = Vector3.up;
		Vector3 down 	= Vector3.down;
		Vector3 front 	= Vector3.forward;
		Vector3 back 	= Vector3.back;
		Vector3 left 	= Vector3.left;
		Vector3 right 	= Vector3.right;
		
		Vector3[] normales = new Vector3[]
		{
			// Bottom
			down, down, down, down,
			
			// Left
			left, left, left, left,
			
			// Front
			front, front, front, front,
			
			// Back
			back, back, back, back,
			
			// Right
			right, right, right, right,
			
			// Top
			up, up, up, up
		};
		//end Normales region	
		
		//Add UVs region 
		Vector2 _00 = new Vector2( 0f, 0f );
		Vector2 _10 = new Vector2( 1f, 0f );
		Vector2 _01 = new Vector2( 0f, 1f );
		Vector2 _11 = new Vector2( 1f, 1f );
		
		Vector2[] uvs = new Vector2[]
		{
			// Bottom
			_11, _01, _00, _10,
			
			// Left
			_11, _01, _00, _10,
			
			// Front
			_11, _01, _00, _10,
			
			// Back
			_11, _01, _00, _10,
			
			// Right
			_11, _01, _00, _10,
			
			// Top
			_11, _01, _00, _10,
		};
		//End UVs region
		
		//Add Triangles region 
		int[] triangles = new int[]
		{
			// Bottom
			3, 1, 0,
			3, 2, 1,			
			
			// Left
			3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
			3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
			
			// Front
			3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
			3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
			
			// Back
			3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
			3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
			
			// Right
			3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
			3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
			
			// Top
			3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
			3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
			
		};
		//End Triangles region
	

		MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
		Material material = new Material (Shader.Find ("Standard"));

		////Add Color
		//Color color = Color.Lerp(Color.white, new Color( Random.value, Random.value, Random.value, 1.0f), Random.Range(0.0f, 1.0f));
		//material.color = color;



		//Add Texture
		Texture2D texture2d;
		float height = 512f;
		float width = 512f;
		texture2d = new Texture2D( 512, 512);
		for( int y = 2; y < height; y += 2 ){
			for( int x = 0; x < width; x += 2 ){
				
				//Color color = new Color((x + 0.5f) * (1f/width), (y + 0.5f) * (1f/height), 0f);
				Color color = new Color(x * (1f/width), y * (1f/height), 0f);
				
				texture2d.SetPixel(x, y, color);
			}
		}
		texture2d.Apply();

		//Texture texture = Resources.Load ("StripesTexture") as Texture;
		material.mainTexture = texture2d; //texture
		meshRenderer.material = material;

	  	Rigidbody meshRigidbody = GetComponent<Rigidbody> ();
		meshRigidbody.useGravity = false; 
		BoxCollider collider = GetComponent<BoxCollider> ();
		collider.center = Vector3.zero;

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.normals = normales;
		mesh.RecalculateBounds();
		mesh.Optimize();

	}

}

