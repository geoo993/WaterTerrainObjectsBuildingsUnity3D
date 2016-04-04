using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
	
public class GizmoPoints : MonoBehaviour {

	public GameObject[] cubes = null; 

	private List<Vector3> vertices = new List<Vector3>();



	private void Start () 
	{

		for (int i = 0; i < cubes.Length; i++) {
			DrawCubeV (cubes[i]);
		}
		print (vertices.Count);
	}


	void DrawCubeV ( GameObject cube)  
	{

		Vector3 bMin = cube.GetComponent<BoxCollider>().bounds.min;
		Vector3 bMax = cube.GetComponent<BoxCollider>().bounds.max;


		vertices.Add(new Vector3(bMax.x, bMax.y, bMax.z)); //top
		vertices.Add(new Vector3(bMin.x, bMax.y, bMin.z)); //top
		vertices.Add(new Vector3(bMin.x, bMax.y, bMax.z)); //top
		vertices.Add(new Vector3(bMax.x, bMax.y, bMin.z)); //top

//		vertices.Add(new Vector3(bMin.x, bMin.y, bMin.z));//bottom			
//		vertices.Add(new Vector3(bMin.x, bMin.y, bMax.z));//bottom
//		vertices.Add(new Vector3(bMax.x, bMin.y, bMin.z));//bottom
//		vertices.Add(new Vector3(bMax.x, bMin.y, bMax.z));//bottom
//
	}


	private void OnDrawGizmos () 
	{
		
		if (vertices == null) {
			return;
		}

		//Gizmos.color = Color.black;

		Gizmos.color = new Color(1, 0, 0, 0.5F);
		//Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));

		for (int i = 0; i < vertices.Count; i++) {
			Gizmos.DrawSphere(vertices[i], 0.1f);
		}


	}
}


