using UnityEngine;
using System.Collections;

public class LerpPerlinVertexColor : MonoBehaviour {


	private float time = 0; 
	private Color currentColor = Color.green; 
	private Color previousColor = Color.red; 
	private int duration = 10;

	private MeshRenderer mR;

	void Start () {
		mR = GetComponent<MeshRenderer> ();
	}
	void Update () {


		if (time < 1.0f) {
			time += Time.deltaTime / duration;
		} else {
			
			time = 0;
			duration = Random.Range (5, 20);

			currentColor = previousColor;
			previousColor = ExtensionMethods.RandomColor();
		}

		Color col = Color.Lerp (currentColor, previousColor, time);
		//print("time: "+ time +"  duration: "+duration+"  col: "+col);


		//Debug.Log(mR.material.GetColor("_Color"));
		mR.material.SetColor ("_Color", col);
	}
}
