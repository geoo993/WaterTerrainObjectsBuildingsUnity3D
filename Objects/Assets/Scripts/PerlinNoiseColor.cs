using UnityEngine;
using System.Collections;

public class PerlinNoiseColor : MonoBehaviour {


	public int size;
	public GameObject obj; 
	public float scale = 6.0f;


	public float heightScale = 1.0F;
	public float xScale = 0.2F;

	public bool move = true;

	void Start () {
		
		this.name = "perlinNoise";

		for (int x = 0; x < size; x++) {
			
			for (int z = 0; z < size; z++) {

				GameObject c = Instantiate(obj, new Vector3(this.transform.position.x+x, this.transform.position.y, this.transform.position.z+z), Quaternion.identity) as GameObject;

				c.transform.parent = this.transform;
				c.name = "c"+x+z;
				//Color color = Color.Lerp(Color.white, new Color( Random.value, Random.value, Random.value, 1.0f), Random.Range(0.0f, 1.0f));
				//c.GetComponent<MeshRenderer> ().material.color = color;

			}
		}
//
//		foreach (Transform child in transform)
//		{
//			float height = Mathf.PerlinNoise ( child.transform.position.x / scale, child.transform.position.z / scale );
//
//			Color color = new Color ( height, height, height, height );
//
//			child.GetComponent<MeshRenderer> ().material.color = color;
//		}
	}

	void Update () {

		if (move) {
			foreach (Transform child in transform) {

				//type 1
				float height = Mathf.PerlinNoise ( child.transform.position.x / scale, child.transform.position.z / scale );
				Color color = new Color ( height, height, height, height);
				child.GetComponent<MeshRenderer> ().material.color = color;

				float yForWaves = height * 3;
				int yForBlock = Mathf.RoundToInt ( height * 3 );

				//type 2
				float yWaveAnimation = heightScale * Mathf.PerlinNoise ( Time.time + (child.transform.position.x * xScale) , Time.time + (child.transform.position.z * xScale) );

				child.transform.position = new Vector3 ( child.transform.position.x, yWaveAnimation, child.transform.position.z );



			}
		}



	}







}
