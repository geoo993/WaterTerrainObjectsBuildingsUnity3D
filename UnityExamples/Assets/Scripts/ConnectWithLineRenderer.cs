using UnityEngine;
using System.Collections;

public class ConnectWithLineRenderer: MonoBehaviour {


	private LineRenderer lineRenderer;

	public int lengthOfLineRenderer = 20;

	public GameObject mainPoint;
	public GameObject[] points;

	private float counter;
	private float dist;
	private float lineDrawSpeed = 6f;

	void SetUpLines ()
	{
		lineRenderer = mainPoint.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(Color.red, Color.green);
		lineRenderer.SetWidth(0.2F, 0.2F);

	}
	void DrawLinesSimple ()
	{
		
		int startlineCount = 1;
		int endlineCount = 1;
		int vertexCount = startlineCount + points.Length + endlineCount;
		lineRenderer.SetVertexCount(vertexCount);



		//start line
		lineRenderer.SetPosition(0, mainPoint.transform.position);
		for (int a = 0; a < points.Length; a++) {

//			dist = Vector3.Distance (mainPoint.transform.position, points [a].transform.position);
//				
//			if (counter < dist) {
//
//				counter += 0.01f;
//
//				float x = Mathf.Lerp (0, dist, counter);
//				Vector3 pointA = mainPoint.transform.position; 
//				Vector3 pointB = points [a].transform.position;
//				Vector3 pointsAlongLines = x * Vector3.Normalize (pointB - pointA) + pointA; 
//				lineRenderer.SetPosition (a + 1, pointsAlongLines);
//
//			}

			//mid points
			lineRenderer.SetPosition (a + 1, points [a].transform.position);
		}
		//end line
		lineRenderer.SetPosition(points.Length+1, mainPoint.transform.position);

		this.transform.Rotate (1f, 1f, 0);
	}



	void DrawWavyLines ()
	{

		lineRenderer.SetVertexCount(points.Length);

		lineRenderer.SetVertexCount(lengthOfLineRenderer);
	}

	void Start ()
	{
		
		SetUpLines ();
	}


	void Update () {


		DrawLinesSimple ();

		////wavy lines
//		lineRenderer = mainPoint.GetComponent<LineRenderer>();
//		Vector3[] points = new Vector3[lengthOfLineRenderer];
//		float t = Time.time;
//		int i = 0;
//
//		////type1
//		while (i < lengthOfLineRenderer) {
//			points[i] = new Vector3(i * 0.5F, Mathf.Sin(i + t), 0);
//			i++;
//		}
//		lineRenderer.SetPositions(points);
//
//		////type 2
//		while (i < lengthOfLineRenderer) {
//			Vector3 pos = new Vector3(i * 0.5F, Mathf.Sin(i + t), 0);
//			lineRenderer.SetPosition(i, pos);
//			//i++;
//		}

	}


}
