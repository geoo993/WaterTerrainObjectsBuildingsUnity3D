using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	public Transform target = null;

	float r = 10.0f;
	public float rad = 0.5f;
	float angle = 0.0f;
	public float height = 2.0f;



	float mw = 0.0f;
	float mh = 0.0f;

	void Start () {


		//mw = target.GetComponent<GenerateCity> ().mapWidth;
		//mh = target.GetComponent<GenerateCity> ().mapHeight;
	}
	

	void Update () {

		mh = target.GetComponent<Buildings> ().toplengthY + target.GetComponent<Buildings> ().midlengthY;

		angle += rad * Time.deltaTime;
		transform.position = new Vector3( r * Mathf.Cos (angle), (mh+height), r * Mathf.Sin(angle) );

		LookAtTarget ();

	}

	void LookAtTarget ()
	{
		this.transform.LookAt (new Vector3 (target.transform.position.x,target.transform.position.y+2f,target.transform.position.z));
		//this.transform.LookAt (new Vector3 (target.transform.position.x-(mw/2),target.transform.position.y+2f,target.transform.position.z-(mw/2)));

	}
}
