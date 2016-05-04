using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class DynamicCubeMesh : MonoBehaviour {

//	public Camera mainCamera;
//	private Ray ray;
//	private RaycastHit hit;
//	private GameObject hitObject = null;
//
	public GameObject sphere;
	private int currentSphereIndex = 0;
	private List <GameObject> newSpheres = new List<GameObject>();

	public bool poityOrNot = false;
	private int xlength = 0;
	private int ylength = 0;
	private int zlength = 0;

	public int xSize = 9;
	public int ySize = 4;
	public int zSize = 2;
	public int roundness = 0;
	private int zExtra = 0;

	private int offset = 0;
	private int midY = 0;

	private Vector3 topPoint = new Vector3 ();
	private List<int[]> controlPoints = new List<int[]>();
	private List<int> listOfIndexes = new List<int>();
	private List <Vector3> verticesCopy = new List<Vector3> ();
	private List<int> topControlPointIndexes = new List<int>();

	private BoxCollider meshCollider; 
	private MeshFilter meshFilter;
	private Renderer meshRenderer;
	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles; 

	private Vector3[] normals;
	private Color32[] cubeUV;
	private Vector2[] uv;
	private static int
	SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {
		triangles[i] = v00;
		triangles[i + 1] = triangles[i + 4] = v01;
		triangles[i + 2] = triangles[i + 3] = v10;
		triangles[i + 5] = v11;
		return i + 6;
	}

	public Texture2D[] rgbTextures;

	public float upLift = 20;

	void Awake ()
	{
		this.name = "dynamic object";
		CreateControllPointsIndexes ();
		MeshAndIndexes ();
		CreateSpheresInControlPoints ();


	}
	private GameObject createSphere(Vector3 pos , List <GameObject> objectArr){

		GameObject a = (GameObject) Instantiate(sphere, pos, Quaternion.identity);
		a.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
		a.GetComponent<Renderer> ().material.color = Color.red;
		a.transform.parent = this.transform;
		objectArr.Add (a);

		return a;
	}


	private void CreateControllPointsIndexes ()
	{
		xlength = xSize + 1;
		ylength = ySize + 1;
		zlength = zSize + 1;

		zExtra = zSize - 2;
		offset = ((xlength * 2 ) + (zSize - 1 + zExtra)) ;

		print (" offset " + offset);

		for (int x = 0; x < offset + 1; x++) {

			for (int z = 0; z < zlength; z++)
			{

				List<int> innerArray = new List<int>();

				for (int y = 0; y < ylength; y++)
				{
					int myPos = (((offset * y) + x) + y);
					innerArray.Add (myPos);
					//print(innerArray[y]);
				}
				controlPoints.Insert(x, innerArray.ToArray());
			}

//			for (int y = 0; y < ySize + 1; y++)
//			{
//				print(controlPoints[x][y]+"  "+ controlPoints.Count);
//			}

		}


	}
	private void CreateSpheresInControlPoints (){

		midY = (int)(Mathf.Round (ySize / 2));

		for (int s = 0; s < listOfIndexes.Count; s++) {

			//print (listOfIndexes.ToArray());
			for (int a = 0; a < offset + 1; a++) {

				if (  listOfIndexes[s].Equals(controlPoints [a] [midY])   ) {

					createSphere (verticesCopy [listOfIndexes [s]], newSpheres);

					//print (listOfIndexes[s]+"   "+newSpheres.Count);
				} 

			}

		}


		////create top sphere
		topPoint = new Vector3((float)xSize/2,ySize,(float)zSize/2);
		createSphere (topPoint, newSpheres);



	}
	private void MeshAndIndexes () {
		
		CreateMesh ();
		CreateVertices();
		CreateTriangles();
		CreateColliders();
		AddToMesh ();
		CreateColorAndtexture ();

		int p = 0;
		for (int i = 0; i < vertices.Length; i++) {

			verticesCopy.Add(new Vector3(vertices [i].x + this.transform.position.x, vertices [i].y + this.transform.position.y, vertices [i].z + this.transform.position.z));

			listOfIndexes.Add (p);
			p++;

			//createSphere (sideControlPoints[i], newSpheres);

			//top vertices
			//if (vertices [i].y == ySize || vertices [i].y == ySize - 1 ) {
			if (vertices [i].y == ySize ) {
				topControlPointIndexes.Add (i);
			}
		}
		Debug.Log ("  vertices length: "+vertices.Length +"   list of indexes length: "+ listOfIndexes.Count+"  v points: "+verticesCopy.Count);


	}

	private void CreateMesh()
	{

		meshFilter = GetComponent<MeshFilter>();
		if (meshFilter == null){
			Debug.LogError("MeshFilter not found!");
			return;
		}

		mesh = meshFilter.sharedMesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}
		mesh.name = "dynamic mesh";
		mesh.Clear();
	}
		
	private void CreateVertices() {


		int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize - 3) * 4;

		int faceVertices = (
			(xSize - 1) * (ySize - 1) +
			(xSize - 1) * (zSize - 1) +
			(ySize - 1) * (zSize - 1)) * 2;
		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
		normals = new Vector3[vertices.Length];
		cubeUV = new Color32[vertices.Length];
		uv = new Vector2[vertices.Length];


		int v = 0;
		// sides
		for (int y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++) {
				SetVertex(v++, x, y, 0);
			}
			for (int z = 1; z <= zSize; z++) {
				SetVertex(v++, xSize, y, z);
			}
			for (int x = xSize - 1; x >= 0; x--) {
				SetVertex(v++, x, y, zSize);
			}

			//
			for (int z = zSize - 1; z > 0; z--) {
				SetVertex(v++, 0, y, z);
			}
		}



		// top 
		for (int z = 1; z < zSize; z++) {
			for (int x = 1; x < xSize; x++) {
				SetVertex(v++, x, ySize, z);
			}
		}
		//bottom
		for (int z = 1; z < zSize; z++) {
			for (int x = 1; x < xSize; x++) {
				SetVertex(v++, x, 0, z);
			}
		}



	}
	private void SetVertex (int i, int x, int y, int z) {
		Vector3 inner = vertices[i] = new Vector3(x, y, z);

		if (x < roundness) {
			inner.x = roundness;
		}
		else if (x > xSize - roundness) {
			inner.x = xSize - roundness;
		}
		if (y < roundness) {
			inner.y = roundness;
		}
		else if (y > ySize - roundness) {
			inner.y = ySize - roundness;
		}
		if (z < roundness) {
			inner.z = roundness;
		}
		else if (z > zSize - roundness) {
			inner.z = zSize - roundness;
		}

		normals[i] = (vertices[i] - inner).normalized;
		vertices[i] = inner + normals[i] * roundness;
		cubeUV[i] = new Color32((byte)x, (byte)y, (byte)z, 0);
		uv[i] = new Vector2((float)x / (xSize * 4), (float)y / (ySize * 4));
	}


	private void CreateTriangles () {
		
		int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
		triangles = new int[quads * 6];
		int ring = (xSize + zSize) * 2;
		int t = 0, v = 0;

		for (int y = 0; y < ySize; y++, v++) {
			for (int q = 0; q < ring - 1; q++, v++) {
				t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
			}
			t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
		}

		t = CreateTopFace(triangles, t, ring);
		t = CreateBottomFace(triangles, t, ring);

	
	}

	private int CreateTopFace (int[] triangles, int t, int ring) {
		int v = ring * ySize;
		for (int x = 0; x < xSize - 1; x++, v++) {
			t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
		}
		t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

		int vMin = ring * (ySize + 1) - 1;
		int vMid = vMin + 1;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
			}
			t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
		}

		int vTop = vMin - 2;
		t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
		for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
		}
		t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

		return t;
	}

	private int CreateBottomFace (int[] triangles, int t, int ring) {
		int v = 1;
		int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
		t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
		for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
		}
		t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

		int vMin = ring - 2;
		vMid -= xSize - 2;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
			}
			t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
		}

		int vTop = vMin - 1;
		t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
		for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
		}
		t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

		return t;
	}

	private void AddToMesh()
	{
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.normals = normals;
		//mesh.colors32 = cubeUV;
		mesh.uv = uv;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();




	}

	private void CreateColorAndtexture() {

		meshRenderer = GetComponent<MeshRenderer>();

		//Material material = Resources.Load("Material") as Material;
		//meshRenderer.material = material;


		Material material = new Material (Shader.Find ("Standard"));
		material.color = Color.Lerp(Color.white, ExtensionMethods.RandomColor(), Random.Range(0.0f, 1.0f));

		Texture texture1 = Resources.Load ("TextureComplete1") as Texture;
		Texture texture2 = Resources.Load ("TextureComplete2") as Texture;
		Texture texture3 = Resources.Load ("TextureComplete3") as Texture;
		Texture texture4 = Resources.Load ("TextureComplete4") as Texture;

		Texture[] texture = new Texture[] {
			texture1,texture2,texture3,texture4
		};
		int randomTexture = (int)Random.Range(0, texture.Length-1);
		material.mainTexture = texture[ randomTexture ];
		meshRenderer.material = material;

//
//		Material[] windowMaterials = new Material[rgbTextures.Length];
//		for(int i = 0; i < rgbTextures.Length; i++) {
//
//			Material m = new Material(Shader.Find("Self-Illumin/Diffuse"));
//
//			m.SetTexture("_MainTex", rgbTextures[i]);
//
//			Texture2D rit = randomIllumTex(rgbTextures[i].width, rgbTextures[i].height);	
//			m.SetTexture("_BumpMap", rit);
//
//			m.SetTextureScale("_MainTex", new Vector2(64,64));
//			m.SetTextureScale("_BumpMap", new Vector2(1,1));
//
//			m.color = Color.Lerp(Color.white, ExtensionMethods.RandomColor(), Random.Range(0.5f, 1.0f));
//
//			windowMaterials[i] = m;
//		}
//
//		int randomrgbTextures = (int)Mathf.Floor(Random.value * rgbTextures.Length);
//		meshRenderer.material = windowMaterials[randomrgbTextures];
//

	}

	Texture2D randomIllumTex(int w, int h) {

		float lightDenisity = 0.2f;

		Texture2D randomIllum = new Texture2D(w, h , TextureFormat.Alpha8, true);
		int mipCount = randomIllum.mipmapCount;
		randomIllum.filterMode = FilterMode.Point;
		for( int mip = 0; mip < mipCount; ++mip ) {
			Color[] cols = randomIllum.GetPixels( mip );
			for(int i = 0; i < cols.Length; ++i ) {
				float rav = Random.value;
				if(rav < lightDenisity) rav = 0.0f;
				cols[i] = new Color(rav, rav, rav, rav);
			}
			randomIllum.SetPixels( cols, mip );
		}
		randomIllum.Apply(false);
		return randomIllum;
	}



	private void UpdateVerticesAndPositions() {

		//// type 1
//		for(int i = 0; i < vertices.Length; i++)
//		{
//			vertices [i] = newSpheres [i].transform.localPosition;
//		}
//


		//// type 2
		//for(int x = 0; x < xlength; x++)
//		for (int x = 0; x < newSpheres.Count-1; x++) 
//		{
//			for (int z = 0; z < controlPoints [x].Length; z++)
//			{
//
//				vertices [controlPoints [x] [z]] = new Vector3(
//				 	newSpheres [x].transform.localPosition.x, 
//					vertices [controlPoints [x] [z]].y, 
//					newSpheres [x].transform.localPosition.z);
//			}
//
//			//clamp y on side spheres
//			newSpheres [x].transform.localPosition = new Vector3(
//				 newSpheres [x].transform.localPosition.x,
//				 Mathf.Clamp (newSpheres [x].transform.localPosition.y, midY, midY),
//				 newSpheres [x].transform.localPosition.z);
//
//		}
//
//		for (int y = 0; y < topControlPointIndexes.Count; y++) {
//
//			if (poityOrNot){
//				// do pointy top
//				vertices [topControlPointIndexes[y]] = newSpheres [newSpheres.Count-1].transform.localPosition ;
//			} else{
//				//do normal top
//				vertices [topControlPointIndexes[y]] = new Vector3(
//					verticesCopy [topControlPointIndexes[y]].x,
//				newSpheres [newSpheres.Count-1].transform.localPosition.y,
//					verticesCopy [topControlPointIndexes[y]].z);
//			}
//		}


//		//clamp x and z on topsphere
//		newSpheres [newSpheres.Count-1].transform.localPosition = new Vector3(
//			Mathf.Clamp (newSpheres [newSpheres.Count-1].transform.localPosition.x, (float)topPoint.x, (float)topPoint.x),
//		 	newSpheres [newSpheres.Count-1].transform.localPosition.y, 
//			Mathf.Clamp (newSpheres [newSpheres.Count-1].transform.localPosition.z, (float)topPoint.z, (float)topPoint.z));
//	
//
//

	}

	private void CreateColliders () {

		Destroy(meshCollider);
		meshCollider = gameObject.AddComponent<BoxCollider>();
		//meshCollider = GetComponent<BoxCollider>();
		meshCollider.size = new Vector3(xSize, ySize, zSize);
		meshCollider.center = new Vector3 ((float)xSize/2, (float)ySize/2, (float)zSize/2);

	}
		
	void Update()
	{

//		CreateMesh ();
//		UpdateVerticesAndPositions ();
//		CreateTriangles();
//		AddToMesh();
//		CreateColliders();
//
//
//		if (Input.GetMouseButtonDown (0)) {
//
//			ray = mainCamera.ScreenPointToRay (Input.mousePosition);
//
//			if (Physics.Raycast (ray, out hit)) {
//
//
//				for (int i = 0; i < newSpheres.Count; i++) {
//
//					if (hit.collider.gameObject == newSpheres [i]) {
//						hitObject = hit.collider.gameObject;
//
//						//hitObject.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
//
//						//currentSphereIndex = mySpheres.IndexOf(hit.collider.gameObject);
//						currentSphereIndex = i;
//
//
//						Debug.Log ("index: "+currentSphereIndex+"   world pos: "+ newSpheres [i].transform.position +"   local pos: "+newSpheres [i].transform.localPosition+"  current vertexpoint: "+vertices[i]);
//
//
//
//					}
//				}
//
//
//			}
//		}



	}


}
