using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer),typeof(MeshCollider))]

public class Ground : MonoBehaviour {

	[Range(10,500)] public int xSize = 200;
	[Range(10,500)] public int ySize = 200;

	private Vector3[] vertices;

	private Color color;
	private Color PreviousColor = Color.white;
	private Color TargetColor = Color.black;


	private bool changeState = false;
	private float smoothTime = 0.0f;
	private float smoother = 0.1f;

	MeshFilter meshFilter;
	MeshRenderer renderer;
	MeshCollider mCollider;
	Mesh mesh;

	private void Awake () {

		this.name = "ground";
		Vector3 rotation =  new Vector3(90,0,0);
		this.transform.localEulerAngles = rotation;

	}


	void Update () {


		CreateMesh (xSize,ySize);
		UpdateColor ();

	}

	void UpdateColor()
	{
		smoothTime += Time.deltaTime * (1.0f / 10f);
		if (smoothTime > 1f) {

			smoothTime = 0;
			changeState = false;
		}

		if (changeState == false)
		{
			float r = Random.Range (0.5f, 0.1f);
			float g = Random.Range (0.5f, 0.1f);
			float b = Random.Range (0.5f, 0.1f);

			TargetColor = new Color( r, g, b, 1.0f);

			PreviousColor = color;

			changeState = true;
		}

		color = Color.Lerp(PreviousColor, TargetColor, smoothTime);


	}
	void CreateMesh(int width, int height)
	{

		mesh = new Mesh();
		mesh.Clear ();
		mesh.name = "waterMesh";

		vertices = new Vector3[(width + 1) * (height + 1)];

		Vector2[] uv = new Vector2[vertices.Length];
		for (int i = 0, y = 0; y <= height; y++) 
		{
			for (int x = 0; x <= width; x++, i++) 
			{
				vertices[i] = new Vector3(x, y);
				uv[i] = new Vector2(x / width, y / height);
				uv[i] = new Vector2((float)x / width, (float)y / height);
			}
		}
	
		mesh.vertices = vertices;
		mesh.uv = uv;

		int[] triangles = new int[width * height * 6];
		for (int ti = 0, vi = 0, y = 0; y < height; y++, vi++) 
		{
			for (int x = 0; x < width; x++, ti += 6, vi++) 
			{
				triangles [ti] = vi;
				triangles [ti + 3] = triangles [ti + 2] = vi + 1;
				triangles [ti + 4] = triangles [ti + 1] = vi + width + 1;
				triangles [ti + 5] = vi + width + 2;
			}
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		meshFilter = GetComponent<MeshFilter>();
		meshFilter.mesh = mesh;

		//renderer = GetComponent<MeshRenderer> ();

		//Material material = new Material (Shader.Find ("Standard"));
		//renderer.material.shader = Shader.Find ("Particles/Alpha Blended");
//		renderer.material.shader = Shader.Find ("Particles/Additive");
//		renderer.material.SetColor ("_TintColor", color);
//
		mCollider = GetComponent<MeshCollider> ();
		mCollider.sharedMesh = mesh;

	}



}
