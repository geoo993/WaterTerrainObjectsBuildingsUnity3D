using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]


public class Plane : MonoBehaviour {


 	public float height = 10f;
	public float width = 10f;

	public void Planee () {

		this.name = "plane";

		MeshFilter meshFilter = GetComponent<MeshFilter>();
		Mesh mesh = new Mesh ();

		meshFilter.mesh = mesh;
		mesh.Clear();

		Vector3[] vertices = new Vector3[4] {
			new Vector3 (0, 0, 0), 
			new Vector3 (width, 0, 1), 
			new Vector3 (0, height, 0),
			new Vector3 (width, height, 0)
		};

		int[] tri = new int[6]; 
		///tri A
		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;

		//tri B
		tri[3] = 2;
		tri[4] = 3;
		tri[5] = 1;

		Vector3[] normals = new Vector3[4];
		normals [0] = -Vector3.forward;
		normals [1] = -Vector3.forward;
		normals [2] = -Vector3.forward;
		normals [3] = -Vector3.forward;

		Vector2[] uvs = new Vector2[4];
		uvs [0] = new Vector2 (0,0);
		uvs [0] = new Vector2 (1,0);
		uvs [0] = new Vector2 (0,1);
		uvs [0] = new Vector2 (1,1);

		MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
		Material material = new Material (Shader.Find ("Standard"));
		Color color = Color.Lerp(Color.white, new Color( Random.value, Random.value, Random.value, 1.0f), Random.Range(0.0f, 1.0f));
		material.color = color;
		meshRenderer.material = material;

		mesh.vertices = vertices;
		mesh.triangles = tri;
		mesh.normals = normals;
		mesh.uv = uvs;

	}

	void CreateMesh(float width, float height)
	{
		this.name = "plane";

		Mesh m = new Mesh();
		m.Clear();
		m.name = "planeMesh";
		m.vertices = new Vector3[] {
			new Vector3(-width, -height, 0.01f),
			new Vector3(width, -height, 0.01f),
			new Vector3(width, height, 0.01f),
			new Vector3(-width, height, 0.01f)
		};
		m.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2(1, 1),
			new Vector2 (1, 0)
		};
		m.triangles = new int[] { 0, 1, 2, 0, 2, 3};
		m.RecalculateNormals();
	

		//GameObject plane = new GameObject("Plane");
		MeshFilter meshFilter = GetComponent<MeshFilter>();//(MeshFilter)plane.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = m;

		MeshRenderer renderer = GetComponent<MeshRenderer>(); //plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material.shader = Shader.Find ("Particles/Additive");
		Texture2D tex = new Texture2D(1, 1);
		tex.SetPixel(0, 0, Color.green);
		tex.Apply();
		renderer.material.mainTexture = tex;
		renderer.material.color = Color.green;

		//plane.transform.parent = this.transform;
		//return plane;
	}

	void Awake() {
		

		CreateMesh (width,height);
		//Planee ();

	}

}
