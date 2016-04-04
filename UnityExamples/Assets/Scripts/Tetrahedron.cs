using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MeshCollider))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (Rigidbody))]


public class Tetrahedron : MonoBehaviour {

	private Vector3[] vertices;

	void Awake ()
	{
		
		Triangle ();
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

	public void Triangle()
	{

		this.name = "triangleTetrahedron";
		this.tag = "Player";

		MeshFilter meshFilter = GetComponent<MeshFilter>();

		if (meshFilter==null){
			Debug.LogError("MeshFilter not found!");
			return ;
		}

		Vector3 p0 = new Vector3(0,0,0);
		Vector3 p1 = new Vector3(1,0,0);
		Vector3 p2 = new Vector3(0.5f,0,Mathf.Sqrt(0.75f));
		Vector3 p3 = new Vector3(0.5f,Mathf.Sqrt(0.75f),Mathf.Sqrt(0.75f)/3);

		Mesh mesh = meshFilter.sharedMesh;

		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}

		mesh.Clear();

		vertices = new Vector3[]{
			p0,p1,p2,
			p0,p2,p3,
			p2,p1,p3,
			p0,p3,p1
		};

		int[] triangles = new int[]{
			0,1,2,
			3,4,5,
			6,7,8,
			9,10,11
		};

		Vector2 uv0 = new Vector2(0,0);
		Vector2 uv1 = new Vector2(1,0);
		Vector2 uv2 = new Vector2(0.5f,1);

		Vector2[] uvs = new Vector2[]{
			uv0,uv1,uv2,
			uv0,uv1,uv2,
			uv0,uv1,uv2,
			uv0,uv1,uv2
		};

		MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
		Material material = new Material (Shader.Find ("Standard"));
		Color color = Color.Lerp(Color.white, new Color( Random.value, Random.value, Random.value, 1.0f), Random.Range(0.0f, 1.0f));
		material.color = color;
		meshRenderer.material = material;

		//MeshCollider meshCollider = GetComponent<MeshCollider> ();
		//meshCollider.isTrigger = false;
		//meshCollider.sharedMesh = meshFilter.sharedMesh;

		Rigidbody meshRigidbody = GetComponent<Rigidbody> ();
		meshRigidbody.useGravity = false; 
		meshRigidbody.isKinematic = false;


		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();

	}

}