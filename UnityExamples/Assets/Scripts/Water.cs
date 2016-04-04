using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer),typeof(MeshCollider))]

public class Water : MonoBehaviour {

	[Range(10,500)] public int xSize = 200;
	[Range(10,500)] public int ySize = 200;

	private Vector3[] vertices;

	[Range(0.001f, 0.01f)] public float transitionTime = 0.005f;

	private Color color;
	private Color PreviousColor;
	private Color TargetColor;

	private float scale = 0.2f;
	private float scalePreviousTarget = 0.2f;
	private float scaleTarget = 0.2f;

	private float speed = 5.0f;
	private float speedPreviousTarget = 5.0f;
	private float speedTarget = 5.0f;

	private float noiseStrength = 1.0f;
	private float noiseStrengthPreviousTarget = 1.0f;
	private float noiseStrengthTarget = 1.0f;

	private float noiseWalk = 2f;
	private float noiseWalkPreviousTarget = 2f;
	private float noiseWalkTarget = 2f;


	bool changeState = false;
	float smoothTime = 0.0f;
	float smoother = 0.1f;

	List<GameObject> waterBox = new List<GameObject> ();

	private void Awake () {

		//CreateMesh (xSize,ySize);

		this.name = "waterWaves";
		Vector3 rotation =  new Vector3(90,0,0);
		this.transform.localEulerAngles = rotation;

		//color = new Color( Random.value, Random.value, Random.value, 1.0f);
		//color = Color.white;
		CreateWaterArea();

	}


	void Update () {


		CreateMesh (xSize,ySize);

		wavesInWater ();
		updateWaterArea ();
	}

	void CreateWaterArea(){

		for (int i = 0; i < 4; i++) {

			GameObject a = GameObject.CreatePrimitive (PrimitiveType.Cube);
			a.GetComponent<Renderer> ().material.color = Color.black;
			a.transform.parent = this.transform;

			waterBox.Add (a);

		}
	}
	void updateWaterArea(){
			
		waterBox [0].transform.position = new Vector3 (this.transform.position.x+(xSize/2), this.transform.position.y, this.transform.position.z);
		waterBox [1].transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z+(ySize/2));
		waterBox [2].transform.position = new Vector3 (this.transform.position.x+(xSize/2), this.transform.position.y, this.transform.position.z+ ySize);
		waterBox [3].transform.position = new Vector3 (this.transform.position.x+xSize, this.transform.position.y, this.transform.position.z+(ySize/2));


		waterBox [0].transform.localScale = new Vector3 (xSize+2,2,2);
		waterBox [1].transform.localScale = new Vector3 (2,2,ySize+2);
		waterBox [2].transform.localScale = new Vector3 (xSize+2,2,2);
		waterBox [3].transform.localScale = new Vector3 (2,2,ySize+2);

		for (int i = 0; i < waterBox.Count; i++) {
			waterBox[i].GetComponent<Renderer> ().material.color = new Color(color.r,color.g,color.b,0.5f);
		}
	}
	void wavesInWater()
	{
		smoothTime += transitionTime;
		if (smoothTime > 10) {

			smoothTime = 0;
			changeState = false;
		}

		if (changeState == false)
		{
			float r = Random.Range (0.5f, 0.1f);
			float g = Random.Range (0.5f, 0.1f);
			float b = Random.Range (0.5f, 0.1f);

			TargetColor = new Color( r, g, b, 1.0f);
			scaleTarget = Random.Range (-0.3f, 0.3f);
			speedTarget = Random.Range (-5.0f, 5.0f);
			noiseStrengthTarget = Random.Range (-1.0f, 1.0f);
			noiseWalkTarget = Random.Range (-5.0f, 5.0f);

			PreviousColor = color;
			scalePreviousTarget = scale;
			speedPreviousTarget = speed;
			noiseStrengthPreviousTarget = noiseStrength;
			noiseWalkPreviousTarget = noiseWalk;

			changeState = true;
		}

		color = Color.Lerp(PreviousColor, TargetColor, smoother * smoothTime);
		scale = Mathf.Clamp (Mathf.Lerp (scalePreviousTarget, scaleTarget, smoother * smoothTime),-0.3f, 0.3f);			
		speed = Mathf.Clamp (Mathf.Lerp (speedPreviousTarget, speedTarget, smoother * smoothTime),-5.0f, 5.0f);
		noiseStrength = Mathf.Clamp (Mathf.Lerp (noiseStrengthPreviousTarget, noiseStrengthTarget, smoother * smoothTime),-1.0f, 1.0f);
		noiseWalk = Mathf.Clamp (Mathf.Lerp (noiseWalkPreviousTarget, noiseWalkTarget, smoother * smoothTime),-5.0f, 5.0f);
		print ("  speed: " + speed + "  scale: " + scale +"  noise Strength: "+noiseStrength+"  noise Walk: "+noiseWalk+ "  smoother: " + smoother + "    timer: " + smoothTime);

	}
	void CreateMesh(int width, int height)
	{

		Mesh mesh = new Mesh();
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
		//waves == perlin noise
		for (int o = 0; o < vertices.Length; o++) {
			Vector3 vertex = vertices[o];

			vertex.z += Mathf.Sin(Time.time * speed + vertices[o].x + vertices[o].y + vertices[o].z) * scale;
			vertex.z += Mathf.PerlinNoise(vertices[o].x + noiseWalk, vertices[o].y + Mathf.Sin(Time.time * 0.1f)    ) * noiseStrength;
			vertices[o] = vertex;
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

		MeshFilter meshFilter = GetComponent<MeshFilter>();
		meshFilter.mesh = mesh;

		MeshRenderer renderer = GetComponent<MeshRenderer> ();

		Texture texture = Resources.Load ("waterTexture") as Texture;
		//Material material = new Material (Shader.Find ("Standard"));
		//renderer.material.shader = Shader.Find ("Particles/Alpha Blended");
		renderer.material.shader = Shader.Find ("Particles/Additive");
		renderer.material.mainTexture = texture;
		renderer.material.SetColor ("_TintColor", color);


		GetComponent<MeshCollider>().sharedMesh = null;
		MeshCollider mCollider = GetComponent<MeshCollider> ();
		mCollider.sharedMesh = mesh;

	}



}
