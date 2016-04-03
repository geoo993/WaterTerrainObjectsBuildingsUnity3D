using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collided : MonoBehaviour {


	public GameObject grid;

	private void Awake () {

		grid = GameObject.Find("grid");
	}



	void OnTriggerEnter (Collider other)
	{
		//Debug.Log (" Object: "+ other.name + " entered trigger.");

			if (other.name == "Sphere(Clone)") {

				
//				for (int i = 0; i < grid.GetComponent<BinarySpaceParticioningSimple> ().spheres.Count-1; i++) {
//					if (grid.GetComponent<BinarySpaceParticioningSimple> ().spheres [i].transform.position == other.gameObject.transform.position) {
//						
//						int idx = grid.GetComponent<BinarySpaceParticioningSimple> ().spheres.IndexOf (grid.GetComponent<BinarySpaceParticioningSimple> ().spheres [i]);
//
//						if (idx <= grid.GetComponent<BinarySpaceParticioningSimple> ().spheres.Count) {
//							
//							//Debug.Log (idx +"   "+grid.GetComponent<BinarySpaceParticioningSimple> ().spheres.Count);
//							//grid.GetComponent<BinarySpaceParticioningSimple> ().spheres.RemoveAt (idx);
//							Destroy (grid.GetComponent<BinarySpaceParticioningSimple> ().spheres [idx]);
//						}
//					}
//				}
//
				Destroy(other.gameObject);
//				//DestroyObject(sph[sph.IndexOf (other.gameObject)]);
//				//grid.GetComponent<BinarySpaceParticioningSimple> ().spheres.RemoveAt(idx);
//
			}


	}
	void OnTriggerStay(Collider other) {

			

	}


}
