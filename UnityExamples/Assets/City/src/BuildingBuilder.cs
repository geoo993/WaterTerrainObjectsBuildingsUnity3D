using UnityEngine;
using System.Collections;

public class BuildingBuilder : MonoBehaviour {
		
	private float boxSize = 0.333f;
	private float tw, th, tz;

	// nf = Number of floors
	// nwx = x width
	// nwz = z width
	public void Build(int nf, int nwx, int nwz, Vector3 offset, Material mat) {
		GetComponent<Renderer>().material = mat;
		
		tw = nwx * boxSize;
		th = nf * boxSize;
		tz = nwz * boxSize;
		
		createWalls(offset);
		createUV(nf, nwx, nwz);
		
		gameObject.AddComponent<BoxCollider>();
	}
	
	void createUV(int nf, int nwx, int nwz) {
		Mesh m = ((MeshFilter) gameObject.GetComponent("MeshFilter")).mesh;
		
		Vector2[] uv = new Vector2[20];
		
		float ro, xo, zo, ho;

		ro = (int)(Random.value * 100) * (1/64.0f);
	 	xo = nwx / 64.0f + ro;
		zo = nwz / 64.0f + ro;
		ho = nf / 64.0f + ro;
		
		// front
		uv[0] = new Vector2(ro,ro);
		uv[1] = new Vector2(xo,ro);
		uv[2] = new Vector2(xo,ho);
		uv[3] = new Vector2(ro,ho);
		
		ro = (int)(Random.value * 100) * (1/64.0f);
		xo = nwx / 64.0f + ro;
		zo = nwz / 64.0f + ro;
		ho = nf / 64.0f + ro;
		
		// left
		uv[4] = new Vector2(ro,ro);
		uv[5] = new Vector2(zo,ro);
		uv[6] = new Vector2(zo,ho);
		uv[7] = new Vector2(ro,ho);
		
		ro = (int)(Random.value * 100) * (1/64.0f);
		xo = nwx / 64.0f + ro;
		zo = nwz / 64.0f + ro;
		ho = nf / 64.0f + ro;
		
		// right
		uv[8] = new Vector2(ro,ro);
		uv[9] = new Vector2(zo,ro);
		uv[10] = new Vector2(zo,ho);
		uv[11] = new Vector2(ro,ho);
		
		ro = (int)(Random.value * 100) * (1/64.0f);
		xo = nwx / 64.0f + ro;
		zo = nwz / 64.0f + ro;
		ho = nf / 64.0f + ro;
		
		// back
		uv[12] = new Vector2(ro,ro);
		uv[13] = new Vector2(xo,ro);
		uv[14] = new Vector2(xo,ho);
		uv[15] = new Vector2(ro,ho);
		
		// roof
		uv[16] = new Vector3(0,0);
		uv[17] = new Vector3(0,0);
		uv[18] = new Vector3(0,0);
		uv[19] = new Vector3(0,0);
		
		m.uv = uv;
		m.uv2 = uv;
	}
	
	
	void createWalls(Vector3 pos) {
		Mesh m = ((MeshFilter) gameObject.GetComponent("MeshFilter")).mesh;
		
		Vector3[] vs = new Vector3[20];
		
		int[] t = {2, 1, 0, 3, 2, 0, 4, 5, 6, 4, 6, 7, 10, 9, 8, 11, 10, 8, 12, 13, 14, 12, 14, 15, 16, 18, 17, 16, 19, 18};

		vs[0] = new Vector3(tw/-2, 0, tz/-2) + pos;
		vs[1] = new Vector3(tw/2,  0, tz/-2) + pos;
		vs[2] = new Vector3(tw/2,  th,  tz/-2) + pos;
		vs[3] = new Vector3(tw/-2, th,  tz/-2) + pos;
		
		vs[4] = new Vector3(tw/-2, 0, tz/-2) + pos;
		vs[5] = new Vector3(tw/-2,  0, tz/2) + pos;
		vs[6] = new Vector3(tw/-2,  th,  tz/2) + pos;
		vs[7] = new Vector3(tw/-2, th,  tz/-2) + pos;
		
		vs[8] = new Vector3(tw/2, 0, tz/-2) + pos;
		vs[9] = new Vector3(tw/2,  0, tz/2) + pos;
		vs[10] = new Vector3(tw/2,  th,  tz/2) + pos;
		vs[11] = new Vector3(tw/2, th,  tz/-2) + pos;
		
		vs[12] = new Vector3(tw/-2, 0, tz/2) + pos;
		vs[13] = new Vector3(tw/2,  0, tz/2) + pos;
		vs[14] = new Vector3(tw/2,  th,  tz/2) + pos;
		vs[15] = new Vector3(tw/-2, th,  tz/2) + pos;
		
		vs[16] = new Vector3(tw/-2, th, tz/2) + pos;
		vs[17] = new Vector3(tw/-2,  th, tz/-2) + pos;
		vs[18] = new Vector3(tw/2,  th,  tz/-2) + pos;
		vs[19] = new Vector3(tw/2, th,  tz/2) + pos;
		
		m.vertices = vs;
		m.triangles = t;
		
		m.RecalculateNormals();
		m.RecalculateBounds();
	}
	
	void Update () {
	
	}
}
