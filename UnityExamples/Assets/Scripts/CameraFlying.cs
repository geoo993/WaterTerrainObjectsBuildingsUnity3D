using UnityEngine;
using System.Collections;

public class CameraFlying : MonoBehaviour {



	private float mainSpeed = 100.0f; //regular speed
	private float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
	private float maxShift = 1000.0f; //Maximum speed when holdin gshift
	private float camSens = 0.25f; //How sensitive it with mouse
	private Vector3 lastMouse = new Vector3(255.0f, 255.0f, 255.0f); //kind of in the middle of the screen, rather than at the top (play)
	private float totalRun  = 1.0f;


	//private Vector3 velocity = Vector3.zero;

	void Update () {

		lastMouse = Input.mousePosition - lastMouse ;
		lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0.0f );
		lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, 0);
		transform.eulerAngles = lastMouse;
		lastMouse =  Input.mousePosition;
		//Mouse  camera angle done.  
		
		//Keyboard commands
		Vector3 p = GetBaseInput();

		if ( Input.GetKey (KeyCode.LeftShift) ){

			totalRun += Time.deltaTime;
			p  = p * totalRun * shiftAdd;
			p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
			p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
			p.z = Mathf.Clamp(p.z, -maxShift, maxShift);

		}
		else{

			totalRun = Mathf.Clamp(totalRun * 0.5f, 1.0f, 1000.0f);
			p = p * mainSpeed;

		}
		
		p = p * Time.deltaTime;


//		if ( Input.GetKey(KeyCode.Space) ){ //If player wants to move on X and Z axis only
//			f = transform.position.y;
//			transform.Translate(p);
//			transform.position = new Vector3 (transform.position.x,f,transform.position.z);
//		}
//		else{
//			transform.Translate( p);
//		}

		Vector3 newPosition = transform.position;
		if (Input.GetKey(KeyCode.Space)){ //If player wants to move on X and Z axis only
			transform.Translate(p);
			newPosition.x = transform.position.x;
			newPosition.z = transform.position.z;
			transform.position = newPosition;
		}
		else{
			transform.Translate(p);
		}



	}

	private Vector3 GetBaseInput ()//returns the basic values, if it's 0 than it's not active.
	{
		Vector3 velocity = new Vector3();

		if ( Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow) ){
			//velocity = new Vector3(0.0f, 0.0f , 1.0f);
			velocity += new Vector3(0, 0 , 1);
		}
		if ( Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow) ){
			//velocity = new Vector3(0.0f, 0.0f , -1.0f);
			velocity += new Vector3(0, 0, -1);
		}
		if ( Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow) ){
			//velocity = new Vector3(-1.0f, 0.0f , 0.0f);
			velocity += new Vector3(-1, 0, 0);
		}
		if ( Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow) ){
			//velocity = new Vector3(1.0f, 0.0f , 0.0f);
			velocity += new Vector3(1, 0, 0);
		}
		return velocity;
	}


}