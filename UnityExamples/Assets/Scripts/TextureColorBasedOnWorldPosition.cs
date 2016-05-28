using UnityEngine;
using System.Collections;

public class TextureColorBasedOnWorldPosition : MonoBehaviour {


	private int resolution = 512;

	private Texture2D texture;
	[Range(100, 1000)] public int rangeFromCenter = 200;

	public Color ColorInPoint00 = Color.yellow;
	public Color ColorInPoint10 = Color.blue;
	public Color ColorInPoint01 = Color.green;
	public Color ColorInPoint11 = Color.red;

	public Color verticalColor = Color.cyan;

	//private void Awake () {
	private void OnEnable () {

		MeshRenderer renderer = GetComponent<MeshRenderer> ();
		if (texture == null) {

			texture = new Texture2D (resolution, resolution, TextureFormat.RGB24, false);
			texture.name = "color positioning Texture";

			renderer.material.mainTexture = texture;
		}
		
	}

	private void Update () {
		if (transform.hasChanged) {
			transform.hasChanged = false;
			FillTexture();
		}
	}


	public void FillTexture () {


		if (texture.width != resolution) {
			texture.Resize(resolution, resolution);
		}

		//// type 1  - get absolute value of each transform position to reference color
		Color colorVector3 = new Color (Mathf.Abs ((float)(this.transform.position.x / rangeFromCenter)), Mathf.Abs ((float)(this.transform.position.y / rangeFromCenter)), Mathf.Abs ((float)(this.transform.position.z / rangeFromCenter)));
		//Debug.Log (colorVector3);


		//// type 2 - get absolute value of each transform position to get the color within each area
		float tx = Mathf.Abs((float)(this.transform.position.x / rangeFromCenter));
		float ty = Mathf.Abs((float)(this.transform.position.y / rangeFromCenter));
		float tz = Mathf.Abs((float)(this.transform.position.z / rangeFromCenter));


		////interpolate between the bottom left and top left corner based on y
		Color xz0Color = Color.Lerp(ColorInPoint00, ColorInPoint01, tz);

		////interpolate between the bottom right and top right corner based on x
		Color xz1Color = Color.Lerp(ColorInPoint10, ColorInPoint11, tz );	

		Color xzColor = Color.Lerp(xz0Color, xz1Color, tx);

		Color finalColor = Color.Lerp(xzColor, verticalColor, ty);

		//Debug.Log ("xz: "+xzColor+"    final: "+finalColor);

		for (int y = 0; y < resolution; y++) {

			for (int x = 0; x < resolution; x++) {

				texture.SetPixel(x, y, finalColor);
			}
		}

		// Apply all SetPixel calls
		texture.Apply();
	}






}

