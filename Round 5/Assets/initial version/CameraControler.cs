using UnityEngine;
using System.Collections;

public class CameraControler : MonoBehaviour {
	public bool hologram;
	public string camera_name;
	// Use this for initialization
	void Start () {
		if (hologram) {
			GameObject[] cameras = new GameObject[4];
			cameras[0] = GameObject.Find(camera_name);
			cameras[1] = Instantiate(cameras[0]);
			cameras[2] = Instantiate(cameras[0]);
			cameras[3] = Instantiate(cameras[0]);

			cameras[0].GetComponent<Camera>().rect = new Rect(0, 0.25f, 0.4f, 0.5f);
			cameras[0].transform.rotation = Quaternion.Euler(0, 0, 270);
			cameras[1].GetComponent<Camera>().rect = new Rect(0.25f, 0.6f, 0.5f, 0.5f);
			cameras[1].transform.rotation = Quaternion.Euler(0, 0, 0);
			cameras[2].GetComponent<Camera>().rect = new Rect(0.6f, 0.25f, 0.5f, 0.5f);
			cameras[2].transform.rotation = Quaternion.Euler(0, 0, 90);
			cameras[3].GetComponent<Camera>().rect = new Rect(0.25f, 0, 0.5f, 0.4f);
			cameras[3].transform.rotation = Quaternion.Euler(0, 0, 180);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
