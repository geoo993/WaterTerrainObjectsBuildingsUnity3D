using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class BinarySpaceParticioningSimple : MonoBehaviour {

	public Camera mainCamera;
	private Ray ray;
	private RaycastHit hit;
	private GameObject hitObject = null;

	List<Vector3> mapEdgePoints = new List<Vector3>();

	List<GameObject> areas = new List<GameObject>();
	List<GameObject> areasIndexDelete = new List<GameObject>();
	private int gameObjectCount = 0;

	private List<Vector3> edgePoints = new List<Vector3>();

	[Range(5,20)] public int minSize = 10;
	[Range(100,400)] public int mapWidth = 200;
	[Range(100,400)] public int mapHeight = 200;


	public GameObject sphere = null;


	private Vector3[] vertices;



//	private void OnDrawGizmos () 
//	{
//
	//		if (edgePoints == null) {
//			return;
//		}
//
//		//Gizmos.color = Color.black;
//
//		Gizmos.color = new Color(1, 0, 0, 0.5F);
//		//Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
//		//yield return wait;
//
//
	//		for (int i = 0; i < edgePoints.Count; i++) {
//			Gizmos.DrawSphere(list[i], 0.5f);
//		}
//			
//	}

	void Start () {


		
	}

	private void Awake () {

		this.transform.name = "city";
		GenerateAreas ();
		StartCoroutine(CreateList());
		addMapEdges ();

		//print("edges: "+mapEdgePoints.Count);
	}

	private void addMapEdges()
	{
		mapEdgePoints.Add( new Vector3(0, 0, 0));
		mapEdgePoints.Add( new Vector3(mapWidth/2, 0, 0)); //center
		mapEdgePoints.Add( new Vector3(mapWidth, 0, 0));
		mapEdgePoints.Add( new Vector3(mapWidth, 0, mapHeight/2));
		mapEdgePoints.Add( new Vector3(mapWidth, 0, mapHeight));
		mapEdgePoints.Add( new Vector3(mapWidth/2, 0, mapHeight));
		mapEdgePoints.Add( new Vector3(0, 0, mapHeight));
		mapEdgePoints.Add( new Vector3(0, 0, mapHeight/2));
		//mapEdgePoints.Add( new Vector3(mapWidth/2, 0, mapHeight/2)); //center
	}

	Vector3 GetClosestEnemy (Vector3 currentPosition, List<Vector3> targets)
	{
		Vector3 bestTarget = new Vector3();
		float closestDistanceSqr = Mathf.Infinity;
		//Vector3 currentPosition = transform.position;

		foreach(Vector3 potentialTarget in targets)
		{
			Vector3 directionToTarget = potentialTarget - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget;
			}
		}

		return bestTarget;
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

	private IEnumerator CreateList ()
	{
		WaitForSeconds wait = new WaitForSeconds (1f);

		Color c = new Color(Random.Range(0.0f,0.2f),Random.Range(0.0f,0.2f), Random.Range(0.0f,0.2f));
		for (int i = 0; i < areas.Count; i++) {

//			GameObject a = (GameObject) Instantiate(sphere, areas [i].transform.position, Quaternion.identity);
//			a.transform.localScale = new Vector3 (3, 3, 3);
//			a.GetComponent<Renderer> ().material.color = Color.black;
//			a.transform.parent = this.transform;

//			if (areasScale [i].x > 50 || areasScale [i].z > 50) {
//				areas [i].GetComponent<Renderer> ().material.color = c;
//			
//			}
			GetVectors (areas [i]);

		}

		yield return wait;	
		Debug.Log ("  areas: " + areas.Count +"    list: " + edgePoints.Count );

		//GetDistinctArrayList (edgePoints, 0);


//		for (int e = 0; e < edgePoints.Count; e++) {
//			//Debug.Log (edgePoints[e]);
//			GameObject s = (GameObject) Instantiate(sphere, edgePoints [e], Quaternion.identity);
//			s.transform.parent = this.transform;
//		}
		Debug.Log ("    list: " + edgePoints.Count);
		yield return wait;

		WaitForSeconds wait2 = new WaitForSeconds(0.05f);

		for (int i = 0; i < areas.Count; i++) {

				GameObject buildingMesh = CreateCube ();
				buildingMesh.transform.parent = this.transform;
				buildingMesh.name = "building"; ///+ b;
				

				float distanceToCenter = Vector3.Distance (GetClosestEnemy(areas [i].transform.position,mapEdgePoints), areas [i].transform.position);
				float scaleX = 	Mathf.Clamp(areas [i].transform.localScale.x / (Random.Range(1.5f, 4.0f)), areas [i].transform.localScale.x / Random.Range(1.5f,20.0f), areas [i].transform.localScale.x - 5);
				float scaleHeight = distanceToCenter;//Random.Range ((mapHeight/15), (mapHeight/3));
				float scaleZ = 	Mathf.Clamp(areas [i].transform.localScale.z / (Random.Range(1.5f, 4.0f)), areas [i].transform.localScale.z / Random.Range(1.5f,20.0f), areas [i].transform.localScale.z - 5);
				
				buildingMesh.transform.localScale = new Vector3 (scaleX, scaleHeight, scaleZ);

				float xPos = areas [i].transform.position.x - (buildingMesh.transform.localScale.x / 2);
				float zPos = areas [i].transform.position.z - (buildingMesh.transform.localScale.z / 2);
				buildingMesh.transform.position = new Vector3 (xPos, 0, zPos);

			yield return wait2;
		}



//		yield return wait;
		//Debug.Break();

	}


	GameObject CreateCube()
	{
		GameObject cube = new GameObject ();

		MeshFilter filter = cube.AddComponent< MeshFilter >();
		Mesh mesh = filter.mesh;
		mesh.Clear();

		float lengthX = 1f; 
		float lengthY = 1f; 
		float lengthZ = 1f;  

		//region Vertices
		Vector3 p0 = new Vector3( 0, 0, lengthZ );
		Vector3 p1 = new Vector3( lengthX, 0, lengthZ );
		Vector3 p2 = new Vector3( lengthX, 0, 0 );
		Vector3 p3 = new Vector3( 0, 0, 0 );	

		Vector3 p4 = new Vector3( 0,	lengthY,  lengthZ );
		Vector3 p5 = new Vector3( lengthX, 	lengthY,  lengthZ);
		Vector3 p6 = new Vector3( lengthX, 	lengthY,  0 );
		Vector3 p7 = new Vector3( 0,	lengthY,  0 );

		vertices = new Vector3[]
		{
			// Bottom
			p0, p1, p2, p3,

			// Left
			p7, p4, p0, p3,

			// Front
			p4, p5, p1, p0,

			// Back
			p6, p7, p3, p2,

			// Right
			p5, p6, p2, p1,

			// Top
			p7, p6, p5, p4
		};
		//endregion

		//		//region Normales
		Vector3 up 	= Vector3.up;
		Vector3 down 	= Vector3.down;
		Vector3 front 	= Vector3.forward;
		Vector3 back 	= Vector3.back;
		Vector3 left 	= Vector3.left;
		Vector3 right 	= Vector3.right;

		Vector3[] normales = new Vector3[]
		{
			// Bottom
			down, down, down, down,

			// Left
			left, left, left, left,

			// Front
			front, front, front, front,

			// Back
			back, back, back, back,

			// Right
			right, right, right, right,

			// Top
			up, up, up, up
		};
		//endregion	

		//top and bottom
		Vector2 tbp1 = new Vector2( 0.68f, 0f);
		Vector2 tbp2 = new Vector2( 1f, 0f );
		Vector2 tbp3 = new Vector2( 1f, 1f );
		Vector2 tbp4 = new Vector2( 0.68f, 1f );

		//rest
		Vector2 rp1 = new Vector2( 0f, 0f );
		Vector2 rp2 = new Vector2( 0.68f, 0f );
		Vector2 rp3 = new Vector2( 0.68f, 1f );
		Vector2 rp4 = new Vector2( 0f, 1f );


		Vector2[] uvs = new Vector2[]
		{
			// Bottom
			//_11, _01, _00, _10,
			tbp1,tbp2,tbp3,tbp4,

			// Left
			//_11, _01, _00, _10,
			rp1,rp2,rp3,rp4,

			// Front
			//_11, _01, _00, _10,
			rp1,rp2,rp3,rp4,

			// Back
			//_11, _01, _00, _10,
			rp1,rp2,rp3,rp4,

			// Right
			//_11, _01, _00, _10,
			rp1,rp2,rp3,rp4,

			// Top
			//_11, _01, _00, _10,
			tbp1,tbp2,tbp3,tbp4
		};
		//endregion

		//region Triangles
		int[] triangles = new int[]
		{
			// Bottom
			3, 1, 0,
			3, 2, 1,			

			// Left
			3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
			3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,

			// Front
			3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
			3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,

			// Back
			3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
			3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,

			// Right
			3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
			3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,

			// Top
			3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
			3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,

		};
		//endregion


		MeshRenderer meshRenderer = cube.AddComponent<MeshRenderer> ();
		Material material = new Material (Shader.Find ("Standard"));
		material.color = Color.Lerp(Color.white, new Color((48.0f/255.0f),(48.0f/255.0f),(80.0f/255.0f)), Random.Range(0.0f, 1.0f));

		meshRenderer.material = material;

		MeshCollider meshCollider = cube.AddComponent<MeshCollider> ();
		meshCollider.isTrigger = false;

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.normals = normales;
		mesh.RecalculateBounds();
		mesh.Optimize();

		return cube as GameObject;
	}

	void GetVectors ( GameObject cube) 
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

	void GenerateAreas () 
	{

		GameObject startCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		startCube.transform.localScale = new Vector3 (mapWidth,1,mapHeight);
		startCube.transform.position = new Vector3(transform.position.x + mapWidth/2, transform.position.y,transform.position.z + mapHeight/2);
		
		areas.Add (startCube);

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

		}

		Debug.Log ("all Areas: "+areas.Count +"   toDeleted: "+areasIndexDelete.Count);


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

						//Debug.Log (areas.IndexOf(hit.collider.gameObject));
						hitObject.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));


//						if (hitObject.transform.localScale.x > 20){
//						hitObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));

//							splitX ( hitObject );	
//						}else if (hitObject.transform.localScale.z > 20){
//
//							splitZ ( hitObject );	
//						}
						Debug.Log (hitObject.transform.localScale);


					}
				}

				if (hit.collider.gameObject.name == "building") {
					//Debug.Log (hit.collider.gameObject.transform.localScale+"  size x: " + hit.collider.gameObject.GetComponent<MeshCollider> ().bounds.min + "  size z:" + hit.collider.gameObject.GetComponent<MeshCollider> ().bounds.max);


					//float distance = Vector3.Distance (new Vector3(mapWidth/2, 0, mapHeight/2), hit.collider.gameObject.transform.position);
					//Debug.Log (GetClosestEnemy(hit.collider.gameObject.transform.position,mapEdgePoints)+"   dist: "+distance);


				}
				//hitObject = hit.collider.gameObject;
				//Debug.Log (hit.collider.name);


			}
		}

	
	}
	
}
