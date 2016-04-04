using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

// adapdted from Aaron Hibberd tutorial https://www.youtube.com/watch?v=IYMQ2ErFz0s

public class DynamicShape : MonoBehaviour {

	public float length = 0.5f;

	public Vector3 v0 = new Vector3 (-0.5f, -0.5f, 0.5f ); //left (bottom, front, left)
	public Vector3 v1 = new Vector3 (0.5f, -0.5f, 0.5f ); //right (bottom, front, right)
	public Vector3 v2 = new Vector3 (0.5f , -0.5f, -0.5f); //right (bottom, back, right)
	public Vector3 v3 = new Vector3 (-0.5f , -0.5f , -0.5f); //left (bottom, back, left)
	public Vector3 v4 = new Vector3 (-0.5f, 0.5f, 0.5f ); //left (top, front, left)
	public Vector3 v5 = new Vector3 (0.5f, 0.5f, 0.5f ); //right (top, front, right)
	public Vector3 v6 = new Vector3 (0.5f , 0.5f, -0.5f); //right (top, back, right)
	public Vector3 v7 = new Vector3 (-0.5f , 0.5f, -0.5f); //left (top, back, left)


	void Start () {

		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if (meshFilter==null){
			Debug.LogError("MeshFilter not found!");
			return;
		}

		Mesh mesh = meshFilter.sharedMesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}

		mesh.Clear();

//		Vector3 v0 = new Vector3 (-length, -length, length ); //left (bottom, front, left)
//		Vector3 v1 = new Vector3 (length, -length, length ); //right (bottom, front, right)
//		Vector3 v2 = new Vector3 (length , -length, -length); //right (bottom, back, right)
//		Vector3 v3 = new Vector3 (-length , -length , -length); //left (bottom, back, left)
//		Vector3 v4 = new Vector3 (-length, length, length ); //left (top, front, left)
//		Vector3 v5 = new Vector3 (length, length, length ); //right (top, front, right)
//		Vector3 v6 = new Vector3 (length , length, -length); //right (top, back, right)
//		Vector3 v7 = new Vector3 (-length , length, -length); //left (top, back, left)


		mesh.vertices = new Vector3[]{

			// Front face 
			v4, v5, v0, v1,

			// Back face 
			v6, v7, v2, v3,

			// Left face 
			v7, v4, v3, v0,

			// Right face
			v5, v6, v1, v2,

			// Top face 
			v7, v6, v4, v5,

			// Bottom face 
			v0, v1, v3, v2


		};

		//Add Triangles region 
		//these are three point, and work clockwise to determine what side is visible
		mesh.triangles = new int[]{


			//front face
			0,2,3, // first triangle
			3,1,0, // second triangle

			//back face
			4,6,7, // first triangle
			7,5,4, // second triangle

			//left face
			8,10,11, // first triangle
			11,9,8, // second triangle

			//right face
			12,14,15, // first triangle
			15,13,12, // second triangle

			//top face
			16,18,19, // first triangle
			19,17,16, // second triangle

			//bottom face
			20,22,23, // first triangle
			23,21,20, // second triangle

		};

		//Add Normales region
		Vector3 front 	= Vector3.forward;
		Vector3 back 	= Vector3.back;
		Vector3 left 	= Vector3.left;
		Vector3 right 	= Vector3.right;
		Vector3 up 		= Vector3.up;
		Vector3 down 	= Vector3.down;

		mesh.normals = new Vector3[]
		{
			// Front face
			front, front, front, front,

			// Back face
			back, back, back, back,

			// Left face
			left, left, left, left,

			// Right face
			right, right, right, right,

			// Top face
			up, up, up, up,

			// Bottom face
			down, down, down, down

		};
		//end Normales region

		//Add UVs region 
		Vector2 u00 = new Vector2( 0f, 0f );
		Vector2 u10 = new Vector2( 1f, 0f );
		Vector2 u01 = new Vector2( 0f, 1f );
		Vector2 u11 = new Vector2( 1f, 1f );

		Vector2[] uvs = new Vector2[]
		{
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


		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();


		MeshRenderer renderer = GetComponent<MeshRenderer> ();
		Material material = new Material (Shader.Find ("Standard"));
		Color color = new Color( Random.value, Random.value, Random.value, 1.0f);
		renderer.material.color = color;

//		GetComponent<MeshCollider>().sharedMesh = null;
//		MeshCollider mCollider = GetComponent<MeshCollider> ();
//		mCollider.sharedMesh = mesh;

	}


}
