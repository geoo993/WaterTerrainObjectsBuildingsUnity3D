using UnityEngine;
using System.Collections;

public class ConnectWithlines: MonoBehaviour {


	public float radius = 0.05f;

	public Mesh cylinderLineMesh;

	public Material lineMaterial;	
	GameObject[] lineObjects;
	public GameObject mainPoint;
	public GameObject[] points;

	void DrawConnectingLines() {

		if (!lineMaterial) {
			Debug.LogError("Please Assign a material on the inspector");
			return;
		}

//		if(mainPoint && points.Length > 0) {
//
//			for (int i = 0; i < points.Length; i++) {
//
//				Vector3 mainPointPos = mainPoint.transform.position;
//				Vector3 pointPos = points[i].transform.position;
//
//				GL.Begin(GL.LINES);
//				lineMaterial.SetPass(0);
//				GL.Color(new Color(lineMaterial.color.r, lineMaterial.color.g, lineMaterial.color.b, lineMaterial.color.a));
//				GL.Vertex3(mainPointPos.x, mainPointPos.y, mainPointPos.z);
//				GL.Vertex3(pointPos.x, pointPos.y, pointPos.z);
//				GL.End();
//			}
//		}

	
		for (int a = 0; a < points.Length-1 ; a++) {

			Vector3 pointPos1 = points[a].transform.position;
			Vector3 pointPos2 = points[a+1].transform.position;

			lineMaterial.SetPass (0);
			GL.Begin (GL.LINES);
			GL.Color (Color.blue);
			GL.Vertex3 (pointPos1.x, pointPos1.y, pointPos1.z);
			GL.Vertex3 (pointPos2.x, pointPos2.y, pointPos2.z);

			GL.End ();

		}


//		this.lineObjects = new GameObject[points.Length];//4
//		for(int i = 0; i < points.Length; i++) {
//			
//			this.lineObjects[i] = new GameObject();
//			this.lineObjects[i].name = "ConnectingLines" + i;
//			this.lineObjects[i].transform.parent = points[i].gameObject.transform;
//
//			GameObject ringOffsetCylinderMeshObject = new GameObject();
//			ringOffsetCylinderMeshObject.transform.parent =  this.lineObjects[i].transform;
//
//			ringOffsetCylinderMeshObject.transform.localPosition = new Vector3(0f, 1f, 0f);
//
//			ringOffsetCylinderMeshObject.transform.localScale = new Vector3(radius, 1f, radius);
//
//			MeshFilter ringMesh = ringOffsetCylinderMeshObject.AddComponent<MeshFilter>();
//			ringMesh.mesh = this.cylinderLineMesh;
//
//			MeshRenderer ringRenderer = ringOffsetCylinderMeshObject.AddComponent<MeshRenderer>();
//			ringRenderer.material = lineMaterial;
//
//		}

	}

	void OnUpdate()
	{
		for(int i = 0; i < points.Length; i++) {

			this.lineObjects[i].transform.position = this.points[i].transform.position;

			float cylinderDistance = 0.5f*Vector3.Distance(this.points[i].transform.position, this.mainPoint.transform.position);
			this.lineObjects[i].transform.localScale = new Vector3(this.lineObjects[i].transform.localScale.x, cylinderDistance, this.lineObjects[i].transform.localScale.z);

			this.lineObjects[i].transform.LookAt(this.mainPoint.transform, Vector3.up);
			this.lineObjects[i].transform.rotation *= Quaternion.Euler(90, 0, 0);
		}

		this.transform.Rotate (1f, 1f, 0);
	}
//	void Start ()
//	{
//		//DrawConnectingLines();
//	}
	void OnPostRender() {
		//DrawConnectingLines();


	}


	void OnDrawGizmos() {
		//DrawConnectingLines();
	}




}
