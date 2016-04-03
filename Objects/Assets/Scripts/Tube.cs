using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CapsuleCollider))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (Rigidbody))]

public class Tube : MonoBehaviour {


	private Vector3[] vertices;

	void Awake ()
	{
		Tubee ();
	}

	private void OnDrawGizmos () {

		if (vertices == null) {
			return;
		}

		Gizmos.color = Color.yellow;
		for (int i = 0; i < vertices.Length; i++) {
			Gizmos.DrawSphere(vertices[i] + this.transform.position, 0.1f);
		}

	}

	void Update ()
	{
		//print (vertices.Length);
	}
	public void Tubee()
	{
		MeshFilter filter = GetComponent<MeshFilter> ();
		Mesh mesh = filter.mesh;
		mesh.Clear ();

		float height = 1f;
		int nbSides = 8;

		// Outter shell is at radius1 + radius2 / 2, inner shell at radius1 - radius2 / 2
		float bottomRadius1 = 0f; //0.5f;
		float bottomRadius2 = 0.5f; //0.15f; 
		float topRadius1 = 0f; //0.5f;
		float topRadius2 = 0.5f; //0.15f;

		int nbVerticesCap = nbSides * 2 + 2;
		int nbVerticesSides = nbSides * 2 + 2;


		//#Add Vertices region

		// bottom + top + sides
		vertices = new Vector3[nbVerticesCap * 2 + nbVerticesSides * 2];
		int vert = 0;
		float _2pi = Mathf.PI * 2f;

		// Bottom cap
		int sideCounter = 0;
		while (vert < nbVerticesCap) {
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;

			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos (r1);
			float sin = Mathf.Sin (r1);
			vertices [vert] = new Vector3 (cos * (bottomRadius1 - bottomRadius2 * .5f), 0f, sin * (bottomRadius1 - bottomRadius2 * .5f));
			vertices [vert + 1] = new Vector3 (cos * (bottomRadius1 + bottomRadius2 * .5f), 0f, sin * (bottomRadius1 + bottomRadius2 * .5f));
			vert += 2;
		}

		// Top cap
		sideCounter = 0;
		while (vert < nbVerticesCap * 2) {
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;

			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos (r1);
			float sin = Mathf.Sin (r1);
			vertices [vert] = new Vector3 (cos * (topRadius1 - topRadius2 * .5f), height, sin * (topRadius1 - topRadius2 * .5f));
			vertices [vert + 1] = new Vector3 (cos * (topRadius1 + topRadius2 * .5f), height, sin * (topRadius1 + topRadius2 * .5f));
			vert += 2;
		}

		// Sides (out)
		sideCounter = 0;
		while (vert < nbVerticesCap * 2 + nbVerticesSides) {
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;

			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos (r1);
			float sin = Mathf.Sin (r1);

			vertices [vert] = new Vector3 (cos * (topRadius1 + topRadius2 * .5f), height, sin * (topRadius1 + topRadius2 * .5f));
			vertices [vert + 1] = new Vector3 (cos * (bottomRadius1 + bottomRadius2 * .5f), 0, sin * (bottomRadius1 + bottomRadius2 * .5f));
			vert += 2;
		}

		// Sides (in)
//		sideCounter = 0;
//		while (vert < vertices.Length) {
//			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
//
//			float r1 = (float)(sideCounter++) / nbSides * _2pi;
//			float cos = Mathf.Cos (r1);
//			float sin = Mathf.Sin (r1);
//
//			vertices [vert] = new Vector3 (cos * (topRadius1 - topRadius2 * .5f), height, sin * (topRadius1 - topRadius2 * .5f));
//			vertices [vert + 1] = new Vector3 (cos * (bottomRadius1 - bottomRadius2 * .5f), 0, sin * (bottomRadius1 - bottomRadius2 * .5f));
//			vert += 2;
//		}
		//#end Vertices region


		//#Add region Normales

		// bottom + top + sides
		Vector3[] normales = new Vector3[vertices.Length];
		vert = 0;

		// Bottom cap
		while (vert < nbVerticesCap) {
			normales [vert++] = Vector3.down;
		}

		// Top cap
		while (vert < nbVerticesCap * 2) {
			normales [vert++] = Vector3.up;
		}

		// Sides (out)
		sideCounter = 0;
		while (vert < nbVerticesCap * 2 + nbVerticesSides) {
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;

			float r1 = (float)(sideCounter++) / nbSides * _2pi;

			normales [vert] = new Vector3 (Mathf.Cos (r1), 0f, Mathf.Sin (r1));
			normales [vert + 1] = normales [vert];
			vert += 2;
		}

		// Sides (in)
//		sideCounter = 0;
//		while (vert < vertices.Length) {
//			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
//
//			float r1 = (float)(sideCounter++) / nbSides * _2pi;
//
//			normales [vert] = -(new Vector3 (Mathf.Cos (r1), 0f, Mathf.Sin (r1)));
//			normales [vert + 1] = normales [vert];
//			vert += 2;
//		}
		//#end Normales  region

		//#Add UVs region
		Vector2[] uvs = new Vector2[vertices.Length];

		vert = 0;
		// Bottom cap
		sideCounter = 0;
		while (vert < nbVerticesCap) {
			float t = (float)(sideCounter++) / nbSides;
			uvs [vert++] = new Vector2 (0f, t);
			uvs [vert++] = new Vector2 (1f, t);
		}

		// Top cap
		sideCounter = 0;
		while (vert < nbVerticesCap * 2) {
			float t = (float)(sideCounter++) / nbSides;
			uvs [vert++] = new Vector2 (0f, t);
			uvs [vert++] = new Vector2 (1f, t);
		}

		// Sides (out)
		sideCounter = 0;
		while (vert < nbVerticesCap * 2 + nbVerticesSides) {
			float t = (float)(sideCounter++) / nbSides;
			uvs [vert++] = new Vector2 (t, 0f);
			uvs [vert++] = new Vector2 (t, 1f);
		}

		// Sides (in)
//		sideCounter = 0;
//		while (vert < vertices.Length) {
//			float t = (float)(sideCounter++) / nbSides;
//			uvs [vert++] = new Vector2 (t, 0f);
//			uvs [vert++] = new Vector2 (t, 1f);
//		}
		//#end UVs region

		//#Add Triangles region
		int nbFace = nbSides * 4;
		int nbTriangles = nbFace * 2;
		int nbIndexes = nbTriangles * 3;
		int[] triangles = new int[nbIndexes];

		// Bottom cap
		int i = 0;
		sideCounter = 0;
		while (sideCounter < nbSides) {
			int current = sideCounter * 2;
			int next = sideCounter * 2 + 2;

			triangles [i++] = next + 1;
			triangles [i++] = next;
			triangles [i++] = current;

			triangles [i++] = current + 1;
			triangles [i++] = next + 1;
			triangles [i++] = current;

			sideCounter++;
		}

		// Top cap
		while (sideCounter < nbSides * 2) {
			int current = sideCounter * 2 + 2;
			int next = sideCounter * 2 + 4;

			triangles [i++] = current;
			triangles [i++] = next;
			triangles [i++] = next + 1;

			triangles [i++] = current;
			triangles [i++] = next + 1;
			triangles [i++] = current + 1;

			sideCounter++;
		}

		// Sides (out)
		while (sideCounter < nbSides * 3) {
			int current = sideCounter * 2 + 4;
			int next = sideCounter * 2 + 6;

			triangles [i++] = current;
			triangles [i++] = next;
			triangles [i++] = next + 1;

			triangles [i++] = current;
			triangles [i++] = next + 1;
			triangles [i++] = current + 1;

			sideCounter++;
		}


		// Sides (in)
//		while (sideCounter < nbSides * 4) {
//			int current = sideCounter * 2 + 6;
//			int next = sideCounter * 2 + 8;
//
//			triangles [i++] = next + 1;
//			triangles [i++] = next;
//			triangles [i++] = current;
//
//			triangles [i++] = current + 1;
//			triangles [i++] = next + 1;
//			triangles [i++] = current;
//
//			sideCounter++;
//		}
		//#end Triangles region

		MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
		Material material = new Material (Shader.Find ("Standard"));
		Color color = Color.Lerp(Color.white, new Color( Random.value, Random.value, Random.value, 1.0f), Random.Range(0.0f, 1.0f));
		material.color = color;
		meshRenderer.material = material;

		Rigidbody meshRigidbody = GetComponent<Rigidbody> ();
		meshRigidbody.useGravity = false; 
		meshRigidbody.isKinematic = false;

		CapsuleCollider collider = GetComponent<CapsuleCollider> ();
		collider.radius = 0.25f;
		collider.center = new Vector3 (0, 0.5f, 0);
		collider.direction = 1; // y axis

		mesh.vertices = vertices;
		mesh.normals = normales;
		mesh.uv = uvs;
		mesh.triangles = triangles;

		mesh.RecalculateBounds ();
		mesh.Optimize ();
	}
}
