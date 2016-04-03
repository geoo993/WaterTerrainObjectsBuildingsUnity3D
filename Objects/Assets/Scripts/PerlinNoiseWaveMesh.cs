using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MeshCollider))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
//[RequireComponent (typeof (Rigidbody))]

public class PerlinNoiseWaveMesh : MonoBehaviour {

	public float scale = 0.1f;
	public float speed = 1.0f;
	public float noiseStrength = 1f;
	public float noiseWalk = 1f;

	private Vector3[] baseHeight;

	void Awake ()
	{

		Color color = new Color( Random.value, Random.value, Random.value, 1.0f);
		GetComponent<MeshRenderer>().material.color = color;
		this.name = "wave";
	}
	void Update () {

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;

		if (baseHeight == null)
			baseHeight = mesh.vertices;

		Vector3[] vertices = new Vector3[baseHeight.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vertex = baseHeight[i];

			vertex.y += Mathf.Sin(Time.time * speed+ baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
			vertex.y += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)    ) * noiseStrength;
			vertices[i] = vertex;
		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals();

	}


}
