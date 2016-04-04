using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class BinarySpaceParticioningSimple : MonoBehaviour {

	public Camera mainCamera;
	private Ray ray;
	private RaycastHit hit;
	private GameObject hitObject = null;

	List<GameObject> areas = new List<GameObject>();
	List<GameObject> areasIndexDelete = new List<GameObject>();
	private int gameObjectCount = 0;

	private List<Vector3> edgePoints = new List<Vector3>();

	[Range(5,20)] public int minSize = 10;
	[Range(100,400)] public int mapWidth = 200;
	[Range(100,400)] public int mapHeight = 200;


	public GameObject sphere = null;


	private void Awake () {

		this.transform.name = "binarySpace";
		StartCoroutine(GenerateAreas ());

	}


	private void GetDistinctArrayList(List<Vector3> arr, int idx)
	{

		int count = 0;

		if (idx >= arr.Count) return;

		Vector3 val = arr[idx];
		foreach (Vector3 v in arr)
		{
			if (v.Equals(arr[idx]))
			{
				count++;
			}
		}

		if (count > 1)
		{
			arr.Remove(val);
			GetDistinctArrayList(arr, idx);
		}
		else
		{
			idx += 1;
			GetDistinctArrayList(arr, idx);
		}
	}


	void AddVector3Edges ( GameObject cube) 
	{
		Vector3[] v = new Vector3[4];

		Vector3 bMin = cube.GetComponent<BoxCollider>().bounds.min;
		Vector3 bMax = cube.GetComponent<BoxCollider>().bounds.max;

		edgePoints.Add(new Vector3 (Mathf.Round (bMax.x), Mathf.Round (bMax.y), Mathf.Round (bMax.z)));
		edgePoints.Add(new Vector3 (Mathf.Round (bMin.x), Mathf.Round (bMax.y), Mathf.Round (bMin.z)));
		edgePoints.Add(new Vector3 (Mathf.Round (bMin.x), Mathf.Round (bMax.y), Mathf.Round (bMax.z)));
		edgePoints.Add(new Vector3 (Mathf.Round (bMax.x), Mathf.Round (bMax.y), Mathf.Round (bMin.z)));

		GetDistinctArrayList (edgePoints, 0);

	}

	private IEnumerator GenerateAreas () 
	{
		WaitForSeconds wait = new WaitForSeconds (1f);
		WaitForSeconds wait2 = new WaitForSeconds (0.04f);

		//create map area object 
		GameObject startCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		startCube.transform.localScale = new Vector3 (mapWidth,1,mapHeight);
		startCube.transform.position = new Vector3(transform.position.x + mapWidth/2, transform.position.y,transform.position.z + mapHeight/2);
		
		areas.Add (startCube);

		//split map object into chuncks
		for (int i = 0; i < areas.Count; i++) {

			float choice = Random.Range(0.0f,1.0f);
			//Debug.Log (choice);

			if (choice <= 0.5f){
				splitX ( areas [i] );
			}else{

				splitZ ( areas [i] );
			}

//			if (areas [i].transform.localScale.x > 30){
//				splitX ( areas [i] );	
//			}else if (areas [i].transform.localScale.z > 40){
//
//				splitZ ( areas [i] );	
//			}

			yield return wait2;
		}
		Debug.Log ("all Areas: "+areas.Count +"   toDeleted: "+areasIndexDelete.Count);

		//remove every remaining chunck that is null in the areas array 
		for (int j = 0; j< areas.Count; j++) {

			for (int a = 0; a < areasIndexDelete.Count; a++) {

				areas.Remove (areasIndexDelete [a]);
			}
			if (areas [j] == null) {
				print (j);
				areas.RemoveAt(j);
			}
		}
		Debug.Log (areas.Count);

		yield return wait;

		//count each area and add sphere to show there center position
		Color c = new Color(Random.Range(0.0f,0.4f),Random.Range(0.0f,0.4f), Random.Range(0.0f,0.4f));
		for (int i = 0; i < areas.Count; i++) {

			GameObject a = (GameObject) Instantiate(sphere, areas [i].transform.position, Quaternion.identity);
			a.transform.localScale = new Vector3 (3, 3, 3);
			a.GetComponent<Renderer> ().material.color = c;
			a.transform.parent = this.transform;

			// add edge vertices of each area block
			AddVector3Edges (areas [i]);
			yield return wait2;
		}
		Debug.Log ("  areas: " + areas.Count +"    edges list: " + edgePoints.Count );

		// remove any duplicated point that is edge point array
		GetDistinctArrayList (edgePoints, 0);

		yield return wait;

		// add sphere on the edges of each area block
		for (int e = 0; e < edgePoints.Count; e++) {
			
			GameObject s = (GameObject) Instantiate(sphere, edgePoints [e], Quaternion.identity);
			s.transform.parent = this.transform;
			yield return wait2;
		}
		Debug.Log ("   edges list: " + edgePoints.Count);

		yield return wait;
		//Debug.Break();

	}



	void splitX(GameObject splitMe){

		float xSplit =  Random.Range(minSize,splitMe.transform.localScale.x - minSize);
		float split1 = splitMe.transform.localScale.x - xSplit;

		float x1 = splitMe.transform.position.x - ((xSplit - splitMe.transform.localScale.x) / 2);
		float x2 = splitMe.transform.position.x + ((split1 - splitMe.transform.localScale.x) / 2);

		if (xSplit > minSize){

			gameObjectCount += 1;
			GameObject c1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c1.transform.localScale = new Vector3 (xSplit, splitMe.transform.localScale.y,splitMe.transform.localScale.z);
			c1.transform.position = new Vector3(x1,splitMe.transform.position.y,splitMe.transform.position.z);
			c1.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c1.transform.parent = this.transform;
			c1.name = "ground" + gameObjectCount;
			areas.Add (c1);


			gameObjectCount += 1;
			GameObject c2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c2.transform.localScale = new Vector3 (split1, splitMe.transform.localScale.y,splitMe.transform.localScale.z);
			c2.transform.position = new Vector3(x2,splitMe.transform.position.y,splitMe.transform.position.z);
			c2.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c2.transform.parent = this.transform;

			c2.name = "ground" + gameObjectCount;
			areas.Add (c2);

			areasIndexDelete.Add(splitMe);
			GameObject.DestroyImmediate(splitMe);
		}		
	}

	void splitZ(GameObject splitMe){
		
		float zSplit = Random.Range(minSize, splitMe.transform.localScale.z - minSize);
		float zSplit1 = splitMe.transform.localScale.z - zSplit;

		float z1 = splitMe.transform.position.z - ((zSplit - splitMe.transform.localScale.z) / 2);
		float z2 = splitMe.transform.position.z+ ((zSplit1 - splitMe.transform.localScale.z) / 2);


		if (zSplit > minSize){
			
			gameObjectCount += 1;
			GameObject c1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c1.transform.localScale = new Vector3 (splitMe.transform.localScale.x, splitMe.transform.localScale.y,zSplit);
			c1.transform.position = new Vector3( splitMe.transform.position.x, splitMe.transform.position.y, z1);
			c1.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c1.transform.parent = this.transform;
			c1.name = "ground" + gameObjectCount;
			areas.Add (c1);


			gameObjectCount += 1;
			GameObject c2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c2.transform.localScale = new Vector3 (splitMe.transform.localScale.x, splitMe.transform.localScale.y,zSplit1);
			c2.transform.position = new Vector3(splitMe.transform.position.x, splitMe.transform.position.y, z2);
			c2.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c2.transform.parent = this.transform;

			c2.name = "ground" + gameObjectCount;
			areas.Add (c2);

			areasIndexDelete.Add(splitMe);
			GameObject.DestroyImmediate(splitMe);

		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) {

			ray = mainCamera.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {

				for (int i = 0; i < areas.Count; i++) {

					if (hit.collider.gameObject == areas [i]) {
						hitObject = hit.collider.gameObject;

						Debug.Log ("area: "+areas.IndexOf(hit.collider.gameObject));
						hitObject.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));


//						if (hitObject.transform.localScale.x > 20){
//						hitObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));

//							splitX ( hitObject );	
//						}else if (hitObject.transform.localScale.z > 20){
//
//							splitZ ( hitObject );	
//						}
						//Debug.Log (hitObject.transform.localScale);


					}
				}


				//hitObject = hit.collider.gameObject;
				//Debug.Log (hit.collider.name);


			}
		}

	
	}
	
}
