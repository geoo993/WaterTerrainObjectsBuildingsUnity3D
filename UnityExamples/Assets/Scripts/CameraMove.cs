using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	public Transform target = null;


	float rad = 0.5f;
	float angle = 0.0f;

	float height = 2.0f;
	float r = 10.0f;

	float mw = 0.0f;
	float mh = 0.0f;


	void Update () {


		mw = target.GetComponent<GenerateCity> ().mapWidth;
		mh = target.GetComponent<GenerateCity> ().mapHeight;

		//mh = target.GetComponent<Buildings> ().toplengthY + target.GetComponent<Buildings> ().midlengthY;

		angle += rad * Time.deltaTime;
		transform.position = new Vector3( (r+mw) * Mathf.Cos (angle), (mh+height), (r+mw) * Mathf.Sin(angle) );

		LookAtTarget ();

	}

	void LookAtTarget ()
	{
		this.transform.LookAt (new Vector3 (target.transform.position.x+(mw/2),target.transform.position.y+2f,target.transform.position.z+(mw/2)));

	}
}
