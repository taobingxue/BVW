using UnityEngine;
using System.Collections;

public class PicesControler : MonoBehaviour {
	//info
	public bool hologram;
	public string camera_name;
	// texture
	RenderTexture render_texture;
	Camera main_camera;
	// Use this for initialization
	void Start () {
		main_camera = GameObject.Find (camera_name).GetComponent<Camera> ();
		if (hologram) {
			render_texture = new RenderTexture (Constant.WIDTH, Constant.HEIGHT * 2, 24);
			main_camera.targetTexture = render_texture;
			main_camera.Render ();
			RenderTexture.active = render_texture;

			GameObject.Find ("triangle0").GetComponent<Renderer> ().material.mainTexture = render_texture;
			GameObject.Find ("triangle1").GetComponent<Renderer> ().material.mainTexture = render_texture;
			GameObject.Find ("triangle2").GetComponent<Renderer> ().material.mainTexture = render_texture;
			GameObject.Find ("triangle3").GetComponent<Renderer> ().material.mainTexture = render_texture;
		} else {
			main_camera.depth = 0;
			main_camera.transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*
		main_camera.targetTexture = render_texture;
		main_camera.Render();
		RenderTexture.active = render_texture;
		GameObject.Find ("triangle0").GetComponent<Renderer> ().material.mainTexture = render_texture;
		*/
	}
}
