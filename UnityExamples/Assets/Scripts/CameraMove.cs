using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	public Transform city = null;

	public float rad = 0.5f;
	float angle = 0.0f;


	float mw = 0;
	float mh = 0;

	void Start () {
	
		mw = city.GetComponent<GenerateCity> ().mapWidth;
		mh = city.GetComponent<GenerateCity> ().mapHeight;
	}
	

	void Update () {

		angle += rad * Time.deltaTime;
		transform.position = new Vector3( mw * Mathf.Cos (angle), mh, mw * Mathf.Sin(angle) );

		LookAtTarget ();
	}

	void LookAtTarget ()
	{
		this.transform.LookAt (new Vector3 (city.transform.position.x+(mw/2), city.transform.position.y , city.transform.position.z+(mh/2)));

	}
}
