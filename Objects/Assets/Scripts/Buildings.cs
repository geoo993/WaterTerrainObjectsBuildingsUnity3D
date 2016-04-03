using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Buildings : MonoBehaviour {

	private Mesh mesh;
	private Vector3[] vertices;

	private MeshFilter filter;
	//private Rigidbody meshRigidbody;
	private MeshRenderer meshRenderer;

	void Start ()
	{
		CreateBuildings();

	}
	GameObject CreateBuildings ()
	{
		GameObject city = new GameObject ();
		
		for (var i = 0; i < 2000; i ++) {
			
			GameObject buildingMesh = CreateCube();
			buildingMesh.transform.parent = city.transform;
			buildingMesh.name = "cuboiddObject" + i;
			
			float xPos = Random.Range (-200.0f, 200.0f);
			float zPos = Random.Range (-200.0f, 200.0f);
			
			float rotationY = Random.Range (0, Mathf.PI * 2);
			float scaleWidth = (((Random.Range (0.0f, 1.0f) * Random.Range (0.0f, 1.0f) * Random.Range (0.0f, 1.0f) * Random.Range (0.0f, 1.0f)) * 50.0f) + 10);
			float scaleHeight = (((Random.Range (0.0f, 1.0f) * Random.Range (0.0f, 1.0f) * Random.Range (0.0f, 1.0f) * scaleWidth) * 8) + 8);

			buildingMesh.transform.position = new Vector3 (xPos, 0, zPos);
			buildingMesh.transform.Rotate (0, rotationY, 0);
			buildingMesh.transform.localScale = new Vector3 (scaleWidth, scaleHeight, scaleWidth);

		}
		
		city.transform.parent = this.transform;
		return city;
	}

	GameObject CreateCube()
	{
		GameObject cube = new GameObject ();
	
		filter = cube.AddComponent< MeshFilter >();
		mesh = filter.mesh;
		mesh.Clear();
		
		float lengthX = 1f; 
		float lengthY = 1f; 
		float lengthZ = 1f;  

		
		//region Vertices
		Vector3 p0 = new Vector3( 0, 0, lengthZ );
		Vector3 p1 = new Vector3( lengthX, 0, lengthZ );
		Vector3 p2 = new Vector3( lengthX, 0, 0 );
		Vector3 p3 = new Vector3( 0, 0, 0 );	
		
		Vector3 p4 = new Vector3( 0,	lengthY,  lengthZ );
		Vector3 p5 = new Vector3( lengthX, 	lengthY,  lengthZ );
		Vector3 p6 = new Vector3( lengthX, 	lengthY,  0 );
		Vector3 p7 = new Vector3( 0,	lengthY,  0 );

		vertices = new Vector3[]
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
		//endregion
		
		//		//region Normales
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
		//endregion	

		//top and bottom
		Vector2 tbp1 = new Vector2( 0.68f, 0f);
		Vector2 tbp2 = new Vector2( 1f, 0f );
		Vector2 tbp3 = new Vector2( 1f, 1f );
		Vector2 tbp4 = new Vector2( 0.68f, 1f );
		
		//rest
		Vector2 rp1 = new Vector2( 0f, 0f );
		Vector2 rp2 = new Vector2( 0.68f, 0f );
		Vector2 rp3 = new Vector2( 0.68f, 1f );
		Vector2 rp4 = new Vector2( 0f, 1f );


		Vector2[] uvs = new Vector2[]
		{
			// Bottom
			//_11, _01, _00, _10,
			tbp1,tbp2,tbp3,tbp4,

			// Left
			//_11, _01, _00, _10,
			rp1,rp2,rp3,rp4,

			// Front
			//_11, _01, _00, _10,
			rp1,rp2,rp3,rp4,

			// Back
			//_11, _01, _00, _10,
			rp1,rp2,rp3,rp4,
			
			// Right
			//_11, _01, _00, _10,
			rp1,rp2,rp3,rp4,
			
			// Top
			//_11, _01, _00, _10,
			tbp1,tbp2,tbp3,tbp4
		};
		//		//endregion
		
		//region Triangles
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
		//endregion
		
		
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.normals = normales;
		meshRenderer = cube.AddComponent<MeshRenderer> ();

		//Material defaultMaterial = GetComponent<SpriteRenderer>().material;
		Material material = new Material (Shader.Find ("Standard"));
		material.color = Color.Lerp(Color.white, new Color((48.0f/255.0f),(48.0f/255.0f),(80.0f/255.0f)), Random.Range(0.0f, 1.0f));

//		Texture texture1 = Resources.Load ("TextureComplete1") as Texture;
//		Texture texture2 = Resources.Load ("TextureComplete2") as Texture;
//		Texture texture3 = Resources.Load ("TextureComplete3") as Texture;
//		Texture texture4 = Resources.Load ("TextureComplete4") as Texture;
//		Texture texture5 = Resources.Load ("TextureComplete5") as Texture;
//		Texture texture6 = Resources.Load ("TextureComplete6") as Texture;
//		Texture texture7 = Resources.Load ("TextureComplete7") as Texture;
//		Texture texture8 = Resources.Load ("TextureComplete8") as Texture;
//		Texture texture9 = Resources.Load ("TextureComplete9") as Texture;
//		Texture texture10 = Resources.Load ("TextureComplete10") as Texture;

//		Texture[] texture = new Texture[] {
//			texture1,texture2,texture3,texture4,texture5,
//			texture6,texture7,texture8,texture9,texture10
//		};
//		material.mainTexture = texture[ Random.Range (0,10) ];

		meshRenderer.material = material;

		MeshCollider meshCollider = cube.AddComponent<MeshCollider> ();
		meshCollider.isTrigger = false;

		cube.layer = 0;
		cube.tag = "Player";

		mesh.RecalculateBounds();
		mesh.Optimize();


		return cube as GameObject;
	}


}
