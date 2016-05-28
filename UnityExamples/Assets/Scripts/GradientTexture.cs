using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GradientToTexture{
	
	public static Texture2D ToTexture(this Gradient aGradient,int width)
	{
		if (width < 1)
			throw new System.ArgumentException("aWidth has to be 1 or greater");
		var colors = new Color[width];
		float denominator = Mathf.Max(1, width - 1);
		for(int i = 0; i < width; i++)
		{
			colors[i] = aGradient.Evaluate((float)i / denominator);
		}
		var tex = new Texture2D(width, 1);
		tex.SetPixels(colors);
		tex.Apply();
		return tex;
	}

}


public class GradientTexture : MonoBehaviour {


	[Range(2, 512)] public int resolution = 256;

	MeshRenderer renderer;
	private Texture2D texture;
	private Color color = Color.blue;

	public Gradient gradientColor = new Gradient();

	private Cubemap cubemap;

	private void OnEnable () {

		renderer = GetComponent<MeshRenderer> ();

	}


	private void Update () {

		if (transform.hasChanged) {
			transform.hasChanged = false;
			FillTexture();
		}

	}

	private void addGradient (Gradient g)
	{

		GradientColorKey blue = new GradientColorKey(Color.blue, 0.0f);
		GradientColorKey white = new GradientColorKey(Color.white, 0.3f);
		GradientColorKey black = new GradientColorKey(Color.black, 0.45f);
		GradientColorKey yellow = new GradientColorKey(Color.yellow, 0.6f);
		GradientColorKey red = new GradientColorKey(Color.red, 1f);

		GradientAlphaKey blueAlpha = new GradientAlphaKey(1,0);
		GradientAlphaKey yellowAlpha = new GradientAlphaKey(1,1);


		GradientColorKey[] colorKeys = new GradientColorKey[]{blue, white, black, yellow, red};
		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]{blueAlpha,yellowAlpha};
		g.SetKeys(colorKeys, alphaKeys);


	}


	public void FillTexture () {


		if (texture == null) {

			renderer.material.mainTexture = null;

			//cubemap = new Cubemap(resolution, TextureFormat.ARGB32, false);
			//texture = new Texture2D (cubemap.width, cubemap.height, TextureFormat.RGB24, false);


			texture = new Texture2D (resolution, resolution, TextureFormat.RGB24, false);
			texture.name = "gradientTexture";

			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Trilinear;//FilterMode.Bilinear; //FilterMode.Point;
			texture.anisoLevel = 9;

			addGradient (gradientColor);
			texture = gradientColor.ToTexture (resolution);

			//renderer.material.mainTexture = texture;


//
			//Material mat = new Material (Shader.Find ("Skybox/Cubemap"));
			Material mat = new Material (Shader.Find (".ShaderExample/ScreenScapeGradient"));
			//cubemap = Resources.Load ("ClearSkyRadiance") as Cubemap;
			mat.SetTexture("_MainTex",texture);


			renderer.material = mat;
			//cubemap.Apply ();

		
		}

	}
		



}

