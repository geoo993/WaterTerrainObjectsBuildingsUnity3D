using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllShapes : MonoBehaviour {
	

	void Start ()
	{
		//CreateCubeOrCuboid ();

		//Tetrahedron ();
	}

	GameObject CreateCubeOrCuboid ()
	{
		
		GameObject cuboid = new GameObject ();
		cuboid.name = "cuboid";
		cuboid.layer = 0;
		cuboid.tag = "Player";

		MeshFilter filter = cuboid.AddComponent< MeshFilter >();
		Mesh mesh = filter.mesh;
		mesh.Clear();
		
		float lengthX = 1f;  
		float lengthY = 1f;
		float lengthZ = 1f;  

		//cube
		float additionalLength = 1f;
		//cuboid
		//float additionalLength = 1.5f;
		
		//Add Vertices region
		Vector3 p0 = new Vector3( 0, 0, lengthZ * additionalLength );
		Vector3 p1 = new Vector3( lengthX, 0, lengthZ * additionalLength );
		Vector3 p2 = new Vector3( lengthX, 0, 0 );
		Vector3 p3 = new Vector3( 0, 0, 0 );	
		
		Vector3 p4 = new Vector3( 0,	lengthY,  lengthZ * additionalLength );
		Vector3 p5 = new Vector3( lengthX, 	lengthY,  lengthZ * additionalLength);
		Vector3 p6 = new Vector3( lengthX, 	lengthY,  0 );
		Vector3 p7 = new Vector3( 0,	lengthY,  0 );

		Vector3[] vertices = new Vector3[]
		{
			// Bottom
			p0, p1, p2, p3,
			
			// Left
			p7, p4, p0, p3,
			
			// Front
			p4, p5, p1, p0,
			
			// Back
			p6, p7, p3, p2,
			
			// Right
			p5, p6, p2, p1,
			
			// Top
			p7, p6, p5, p4
		};
		//End Vertices region
		
		//Add Normales region
		Vector3 up 	= Vector3.up;
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
	

		MeshRenderer meshRenderer = cuboid.AddComponent<MeshRenderer> ();
		//Material material = new Material(Shader.Find("Sprites/Default"));
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

	  	Rigidbody meshRigidbody = cuboid.AddComponent<Rigidbody> ();
		meshRigidbody.useGravity = false;

		BoxCollider collider = cuboid.AddComponent<BoxCollider> ();
		collider.center = new Vector3 (0.5f, 0.5f, 0.5f);
		collider.isTrigger = false;

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.normals = normales;
		meshRenderer.material = material;
		mesh.RecalculateBounds();
		mesh.Optimize();


		return cuboid as GameObject;
	}
		
	GameObject Tetrahedron ()
	{
		GameObject tetrahedron = new GameObject ();
		tetrahedron.name = "triangle";
		tetrahedron.layer = 0;
		tetrahedron.tag = "Player";


		MeshFilter meshFilter = tetrahedron.AddComponent< MeshFilter >();

		if (meshFilter == null){
			
			Debug.LogError("MeshFilter not found!");
			return null; 
		}

		Vector3 p0 = new Vector3(0,0,0);
		Vector3 p1 = new Vector3(1,0,0);
		Vector3 p2 = new Vector3(0.5f,0,Mathf.Sqrt(0.75f));
		Vector3 p3 = new Vector3(0.5f,Mathf.Sqrt(0.75f),Mathf.Sqrt(0.75f)/3f);

		Mesh mesh = meshFilter.mesh;
		mesh.Clear();

		mesh.vertices = new Vector3[]{
			p0,p1,p2,
			p0,p2,p3,
			p2,p1,p3,
			p0,p3,p1
		};
		mesh.triangles = new int[]{
			0,1,2,
			3,4,5,
			6,7,8,
			9,10,11
		};

		Vector2 uv0 = new Vector2(0,0);
		Vector2 uv1 = new Vector2(1,0);
		Vector2 uv2 = new Vector2(0.5f,1);

		mesh.uv = new Vector2[]{
			uv0,uv1,uv2,
			uv0,uv1,uv2,
			uv0,uv1,uv2,
			uv0,uv1,uv2
		};

		MeshRenderer meshRenderer = tetrahedron.AddComponent<MeshRenderer> ();
		Material material = new Material (Shader.Find ("Standard"));
		Color color = Color.Lerp(Color.white, new Color( Random.value, Random.value, Random.value, 1.0f), Random.Range(0.0f, 1.0f));
		material.color = color;

		//MeshCollider meshCollider = tetrahedron.AddComponent<MeshCollider> ();
		//meshCollider.isTrigger = false;


		Rigidbody meshRigidbody = tetrahedron.AddComponent<Rigidbody> ();
		meshRigidbody.useGravity = false; 
		meshRigidbody.isKinematic = false;

		meshRenderer.material = material;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();

		return tetrahedron as GameObject;
	}
}


