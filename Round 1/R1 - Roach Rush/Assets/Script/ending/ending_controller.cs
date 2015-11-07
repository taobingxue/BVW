using UnityEngine;
using System.Collections;

public class ending_controller : MonoBehaviour {
	public DeviceOrEmulator devOrEmu;
	Texture2D[] tex;
	// Use this for initialization
	void Start () {
		GameObject percent;
		percent = GameObject.Find("percent");
		percent.GetComponent<TextMesh>().text = "      " + Constant.finalscore + "%\nhappiness";
		Debug.Log ("percent");
		// if (devOrEmu.useEmulator) return;
		Debug.Log ("hi");
		if (Constant.photos == null) return ;

		tex = new Texture2D[4];
		for (int i = 0; i < 4; i ++) {
			// get photo
			tex[i] = new Texture2D(320,240,TextureFormat.ARGB32,false);
			tex[i].SetPixels32(Constant.photos[i]);
			Debug.Log (Constant.photos[i].Length);
			tex[i].Apply(false);

			// set texture
			GameObject obj = GameObject.Find ("photo" + i);
			obj.GetComponent<Renderer>().material.mainTexture = tex[i];
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
