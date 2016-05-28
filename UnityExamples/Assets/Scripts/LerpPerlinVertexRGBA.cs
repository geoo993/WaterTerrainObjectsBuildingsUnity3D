using UnityEngine;
using System.Collections;

public class LerpPerlinVertexRGBA : MonoBehaviour {


	private float xTime = 0; 
	private float currentX = 0; 
	private float previousX = 0.5f; 
	private int xDuration = 10;

	private float yTime = 0; 
	private float currentY = 0; 
	private float previousY = 0.5f; 
	private int yDuration = 10;

	private float zTime = 0; 
	private float currentZ = 0; 
	private float previousZ = 0.5f; 
	private int zDuration = 10;

	private MeshRenderer mR;

	void Start () {
		mR = GetComponent<MeshRenderer> ();
	}
	void Update () {


		////x
		if (xTime < 1.0f) {
			xTime += Time.deltaTime / xDuration;
		} else {
			
			xTime = 0;
			xDuration = Random.Range (5, 10);

			currentX = previousX;
			previousX = Random.Range (0.0f, 1.0f);
		}

		float xRes = Mathf.Lerp (currentX, previousX, xTime);
		//print("xTime: "+ xTime +"  xDuration: "+xDuration+"  xRes: "+xRes);


		////y
		if (yTime < 1.0f) {
			yTime += Time.deltaTime / yDuration;
		} else {

			yTime = 0;
			yDuration = Random.Range (5, 10);

			currentY = previousY;
			previousY = Random.Range (0.0f, 1.0f);
		}

		float yRes = Mathf.Lerp (currentY, previousY, yTime);
		//print("yTime: "+ yTime +"  yDuration: "+yDuration+"  yRes: "+yRes);


		///zz
		if (zTime < 1.0f) {
			zTime += Time.deltaTime / zDuration;
		} else {

			zTime = 0;
			zDuration = Random.Range (5, 10);

			currentZ = previousZ;
			previousZ = Random.Range (0.0f, 1.0f);
		}

		float zRes = Mathf.Lerp (currentZ, previousZ, zTime);
		//print("zTime: "+ zTime +"  xDuration: "+zDuration+"  zRes: "+zRes);

		//print("xRes: "+ xRes +"yRes: "+ yRes +"  zRes: "+zRes);

//		mat.SetVector ("_Offset", new Vector4 (4.0f, 0, 0, 0));
//		mat.SetFloat ("R", xRes);
//		mat.SetFloat ("G", yRes);
//		mat.SetFloat ("B", zRes);

		Debug.Log(mR.material.GetFloat("R"));
		mR.material.SetFloat ("R", xRes);
		mR.material.SetFloat ("G", yRes);
		mR.material.SetFloat ("B", zRes);
	}
}
