using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(LineRenderer)) ]

public class Gizmo : MonoBehaviour {



	public int xSize, ySize;
	private Vector3[] vertices;
	private LineRenderer lineRenderer;


//	private void Awake () {
//		StartCoroutine(Generate());
//	}
	private void Start () {
		StartCoroutine(Generate());
	}

	private IEnumerator Generate () {
		WaitForSeconds wait = new WaitForSeconds(0.5f);
		vertices = new Vector3[(xSize + 1) * (ySize + 1)];

		for (int i = 0, y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++, i++) {
				vertices[i] = new Vector3(x, y);
				yield return wait;
			}
		}
	}

	private void OnDrawGizmos () {

		if (vertices == null) {
			return;
		}
		Gizmos.color = Color.black;
		for (int i = 0; i < vertices.Length; i++) {
			Gizmos.DrawSphere(vertices[i], 0.1f);
		}

		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(Color.red, Color.green);
		lineRenderer.SetWidth(0.2F, 0.2F);

		lineRenderer.SetVertexCount(vertices.Length);

		for (int a = 0; a < vertices.Length; a++) {

			lineRenderer.SetPosition (a, vertices [a]);
		}


		//print (vertices.Length);
	}
}

