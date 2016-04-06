using UnityEngine;
using System.Collections;

public class CityBuilder : MonoBehaviour {

	public GameObject buildingPrefab;
	private float cellSize = 4.2f;
	public Texture2D plan;
	
	public Texture2D[] rgbTextures;
	private Material[] windowMaterials;
	
	private GUIStyle lstyle;
	
	private float lightDenisity = 0.2f;
	private int maxBuildingHeight = 70;
	
	private GameObject city;
	
	void Start () {
		city = new GameObject("City");
		
		float csqs = 50;
		
		float hc = csqs / -2.0f * cellSize;
		
		int built = 0;
		
		Material[] windowMaterials = new Material[rgbTextures.Length];
		for(int i = 0; i < rgbTextures.Length; i++) {
			
			Material m = new Material(Shader.Find("Self-Illumin/Diffuse"));
			
			m.SetTexture("_MainTex", rgbTextures[i]);
			
			Texture2D rit = randomIllumTex(rgbTextures[i].width, rgbTextures[i].height);	
			m.SetTexture("_BumpMap", rit);
			
			m.SetTextureScale("_MainTex", new Vector2(64,64));
			m.SetTextureScale("_BumpMap", new Vector2(1,1));
			
			windowMaterials[i] = m;
		}
		
		for(int s = 0; s < csqs; s++) {		
			
			for(int a = 0; a < csqs; a++) {
				Color c = plan.GetPixelBilinear(s/csqs, a/csqs);
				if(c.r < 0.05f) continue;
				
				float xp = cellSize * s + cellSize/2 * (s / 5);
				float yp = cellSize * a + cellSize/2 * (a / 5);
				
				GameObject b = (GameObject) GameObject.Instantiate(buildingPrefab, new Vector3(hc + xp, 0, hc + yp), Quaternion.identity);
				b.name = "Building_" + s + "x" + a;
				b.transform.parent = city.transform;
				BuildingBuilder bt = (BuildingBuilder) b.GetComponent(typeof(BuildingBuilder));
				
				int tx = (int)Mathf.Floor(Random.value * rgbTextures.Length);
				
				
				
				bt.Build(
					(int)(c.r * maxBuildingHeight), 
					(int)(Random.value * 6 + 6), 
					(int)(Random.value * 6 + 6), 
					new Vector3(0,0,0),
					windowMaterials[tx]
				);
				
				built++;
			}
		}
		
		city.transform.localScale = new Vector3(10,10,10);
		//CombineChildren ch = (CombineChildren) city.AddComponent<CombineChildren>();
		//ch.generateTriangleStrips = false;
		
	}
	
	Texture2D randomIllumTex(int w, int h) {
		Texture2D randomIllum = new Texture2D(w, h , TextureFormat.Alpha8, true);
		int mipCount = randomIllum.mipmapCount;
		randomIllum.filterMode = FilterMode.Point;
		for( int mip = 0; mip < mipCount; ++mip ) {
			Color[] cols = randomIllum.GetPixels( mip );
			for(int i = 0; i < cols.Length; ++i ) {
				float rav = Random.value;
				if(rav < lightDenisity) rav = 0.0f;
				cols[i] = new Color(0, 0, 0, rav);
			}
			randomIllum.SetPixels( cols, mip );
		}
		randomIllum.Apply(false);
		return randomIllum;
	}
	
	void OnGUI() {
		GUI.color = new Color(1,1,1,1);
		GUI.Label(new Rect(10, 10, 200, 48), "Walk with cursors\nSpace for mega-jump to the roof");
	}
}
