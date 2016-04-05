using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Buildings : MonoBehaviour {

	public bool resetToCube = false;
	public bool stop = false;

	[Range(-5.0f,5.0f)] public float bottomlengthX = 1f; 
	[Range(-5.0f,5.0f)] public float bottomlengthY = 1f; 
	[Range(-5.0f,5.0f)] public float bottomlengthZ = 1f;  
	[Range(-5.0f,5.0f)] public float midlengthX = 1f; 
	[Range(-5.0f,5.0f)] public float midlengthY = 1f; 
	[Range(-5.0f,5.0f)] public float midlengthZ = 1f; 
	[Range(-5.0f,5.0f)] public float toplengthX = 1f; 
	[Range(-5.0f,5.0f)] public float toplengthY = 1f; 
	[Range(-5.0f,5.0f)] public float toplengthZ = 1f; 

	Vector3 v0 = Vector3.zero; //left (bottom, front, left)
	Vector3 v1 = Vector3.zero; //right (bottom, front, right)
	Vector3 v2 = Vector3.zero; //right (bottom, back, right)
	Vector3 v3 = Vector3.zero; //left (bottom, back, left)
	Vector3 v4 = Vector3.zero; //left (top, front, left)
	Vector3 v5 = Vector3.zero; //right (top, front, right)
	Vector3 v6 = Vector3.zero; //right (top, back, right)
	Vector3 v7 = Vector3.zero; //left (top, back, left)
	Vector3 v8 = Vector3.zero; //left (mid, front, left)
	Vector3 v9 = Vector3.zero; //right (mid, front, right)
	Vector3 v10 = Vector3.zero; //right (mid, back, right)
	Vector3 v11 = Vector3.zero; //left (mid, back, left)

	float topTargetX = 1.0f;
	float topTargetY = 1.0f;
	float topTargetZ = 1.0f;
	float topPreTargetX = 1.0f;
	float topPreTargetY = 1.0f;
	float topPreTargetZ = 1.0f;

	float midTargetX = 1.0f;
	float midTargetY = 0.0f;
	float midTargetZ = 1.0f;
	float midPreTargetX = 1.0f;
	float midPreTargetY = 0.0f;
	float midPreTargetZ = 1.0f;

	float bottomTargetX = 1.0f;
	float bottomTargetY = 1.0f;
	float bottomTargetZ = 1.0f;
	float bottomPreTargetX = 1.0f;
	float bottomPreTargetY = 1.0f;
	float bottomPreTargetZ = 1.0f;

	private Color color;
	private Color PreviousColor;
	private Color TargetColor;

	bool changeState = true;
	float smoothTime = 0.0f;
	float smoother = 0.1f;

	[Range(0.001f, 0.05f)] public float transitionSpeed = 0.005f;

	void Start ()
	{
		this.name = "building Block";
		//CreateCube ();	

	}

	void Update()
	{
		if (!stop){
			smoothTime += transitionSpeed;	
		}


		if (smoothTime > 10) {

			smoothTime = 0;
			changeState = false;
		}

		if (changeState == false)
		{
			transitionSpeed =  Random.Range (0.01f, 0.05f);

			if (!resetToCube) {
				
				topTargetX = Random.Range (-5.0f, 5.0f);
				topTargetY = Random.Range (1.0f, 5.0f);
				topTargetZ = Random.Range (-5.0f, 5.0f);
				midTargetX = Random.Range (-5.0f, 5.0f);
				midTargetY = Random.Range (0.0f, 4.0f);
				midTargetZ = Random.Range (-5.0f, 5.0f);

				bottomTargetX = Random.Range (-5.0f, 5.0f);
				bottomTargetY = 1.0f;
				bottomTargetZ = Random.Range (-5.0f, 5.0f);
				TargetColor = new Color( Random.value, Random.value, Random.value, 1.0f);

			} else {

				topTargetX = 1.0f;
				topTargetY = 1.0f;
				topTargetZ = 1.0f;
				midTargetX = 1.0f;
				midTargetY = 0.0f;
				midTargetZ = 1.0f;
				bottomTargetX = 1.0f;
				bottomTargetY = 1.0f;
				bottomTargetZ = 1.0f;


				TargetColor = Color.black;
			}
			topPreTargetX = toplengthX;
			topPreTargetY = toplengthY;
			topPreTargetZ = toplengthZ;
			midPreTargetX = midlengthX;
			midPreTargetY = midlengthY;
			midPreTargetZ = midlengthZ;
			bottomPreTargetX = bottomlengthX;
			bottomPreTargetY = bottomlengthY;
			bottomPreTargetZ = bottomlengthZ;
			PreviousColor = color;

			changeState = true;
		}

		toplengthX = Mathf.Lerp (topPreTargetX, topTargetX, smoother * smoothTime);			
		toplengthY = Mathf.Lerp (topPreTargetY, topTargetY+midTargetY, smoother * smoothTime);	
		toplengthZ = Mathf.Lerp (topPreTargetZ, topTargetZ, smoother * smoothTime);

		midlengthX = Mathf.Lerp (midPreTargetX, midTargetX, smoother * smoothTime);			
		midlengthY = Mathf.Lerp (midPreTargetY, midTargetY, smoother * smoothTime);
		midlengthZ = Mathf.Lerp (midPreTargetZ, midTargetZ, smoother * smoothTime);

		bottomlengthX = Mathf.Lerp (bottomPreTargetX, bottomTargetX, smoother * smoothTime);			
		bottomlengthY = Mathf.Lerp (bottomPreTargetY, bottomTargetY, smoother * smoothTime);
		bottomlengthZ = Mathf.Lerp (bottomPreTargetZ, bottomTargetZ, smoother * smoothTime);
		color = Color.Lerp(PreviousColor, TargetColor, smoother * smoothTime);

		CreateCube ();

		//Debug.Log ("smoothtime:  "+smoothTime);

	}

	private void CreateCube()
	{

		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if (meshFilter==null){
			Debug.LogError("MeshFilter not found!");
			return;
		}

		Mesh mesh = meshFilter.sharedMesh;
		//mesh = meshFilter.mesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
			//mesh = meshFilter.mesh;
		}

		mesh.Clear();
		
		//cube details
		v0 = new Vector3 (-toplengthX, toplengthY, toplengthZ); //left (top, front, left)
		v1 = new Vector3 (toplengthX, toplengthY, toplengthZ); //right (top, front, right)
		v2 = new Vector3 (toplengthX , toplengthY, -toplengthZ); //right (top, back, right)
		v3 = new Vector3 (-toplengthX , toplengthY, -toplengthZ); //left (top, back, left)

		v4 = new Vector3 (-midlengthX, midlengthY, midlengthZ ); //left (mid, front, left)
		v5 = new Vector3 (midlengthX, midlengthY, midlengthZ ); //right (mid, front, right)
		v6 = new Vector3 (midlengthX , midlengthY, -midlengthZ); //right (mid, back, right)
		v7 = new Vector3 (-midlengthX , midlengthY , -midlengthZ); //left (mid, back, left)
		

		v8 = new Vector3 (-bottomlengthX, -bottomlengthY, bottomlengthZ ); //left (bottom, front, left)
		v9 = new Vector3 (bottomlengthX, -bottomlengthY, bottomlengthZ ); //right (bottom, front, right)
		v10 = new Vector3 (bottomlengthX , -bottomlengthY, -bottomlengthZ); //right (bottom, back, right)
		v11 = new Vector3 (-bottomlengthX , -bottomlengthY , -bottomlengthZ); //left (bottom, back, left)


		//Add region Vertices
		mesh.vertices = new Vector3[]{


			// top Front face 
			v0, v1, v4, v5,

			// top Back face 
			v2, v3, v6, v7,

			// top Left face 
			v3, v0, v7, v4,

			// top Right face
			v1, v2, v5, v6,



			// bottom Front face 
			v4, v5, v8, v9,


			// bottom Back face 
			v6, v7, v10, v11,

			// bottom Left face 
			v7, v4, v11, v8,

			// bottom Right face
			v5, v6, v9, v10,




			// Top face 
			v3, v2, v0, v1,

			// Bottom face 
			v8, v9, v11, v10




		};
		//end vertices region

		//Add Triangles region 
		//these are three point, and work clockwise to determine what side is visible
		mesh.triangles = new int[]{

			// top front face
			0,2,3, // first triangle
			3,1,0, // second triangle

			// top back face
			4,6,7, // first triangle
			7,5,4, // second triangle

			// top left face
			8,10,11, // first triangle
			11,9,8, // second triangle

			// top right face
			12,14,15, // first triangle
			15,13,12, // second triangle


			//bottom front face
			16,18,19, // first triangle
			19,17,16, // second triangle

			//bottom back face
			20,22,23, // first triangle
			23,21,20, // second triangle

			//bottom left face 
			24,26,27, // first triangle
			27,25,24, // second triangle

			//bottom right face 
			28,30,31, // first triangle
			31,29,28, // second triangle



			//top face 
			32,34,35, // first triangle
			35,33,32, // second triangle

			//bottom face 
			36,38,39, // first triangle
			39,37,36 // second triangle

		};
		//end triangles region


		//Normales vector3 region
		Vector3 front 	= Vector3.forward;
		Vector3 back 	= Vector3.back;
		Vector3 left 	= Vector3.left;
		Vector3 right 	= Vector3.right;
		Vector3 up 		= Vector3.up;
		Vector3 down 	= Vector3.down;

		//Add Normales region
		mesh.normals = new Vector3[]
		{
			// top Front face
			front, front, front, front,

			// top Back face
			back, back, back, back,

			// top Left face
			left, left, left, left,

			// top Right face
			right, right, right, right,


			// bottom Front face
			front, front, front, front,

			// bottom Back face
			back, back, back, back,

			// bottom Left face
			left, left, left, left,

			// bottom Right face
			right, right, right, right,



			// Top face
			up, up, up, up,

			// Bottom face
			down, down, down, down

		};
		//end Normales region

		//Add vector2 regions 
		Vector2 u00 = new Vector2( 0f, 0f );
		Vector2 u10 = new Vector2( 1f, 0f );
		Vector2 u01 = new Vector2( 0f, 1f );
		Vector2 u11 = new Vector2( 1f, 1f );

		//Add UVs region 
		mesh.uv = new Vector2[]
		{
			// Front face uv
			u01, u00, u11, u10,

			// Back face uv
			u01, u00, u11, u10,

			// Left face uv
			u01, u00, u11, u10,

			// Right face uv
			u01, u00, u11, u10,



			// Front face uv
			u01, u00, u11, u10,

			// Back face uv
			u01, u00, u11, u10,

			// Left face uv
			u01, u00, u11, u10,

			// Right face uv
			u01, u00, u11, u10,


			// Top face uv
			u01, u00, u11, u10,

			// Bottom face uv
			u01, u00, u11, u10
		};
		//End UVs region
		

//		Texture texture = Resources.Load ("TextureComplete1") as Texture;
//		material.mainTexture = texture;


		//MeshCollider meshCollider = cube.AddComponent<MeshCollider> ();
		//meshCollider.isTrigger = false;


		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();


		MeshRenderer renderer = GetComponent<MeshRenderer> ();
		renderer.material.color = color;
	}


}
