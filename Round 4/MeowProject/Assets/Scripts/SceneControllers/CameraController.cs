using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	GameObject my_camera;
/*
	Vector3[] zoom_target;
	float[] zoom_time;
	int list_length = 15;
	int p_end = 0, p_start = 0;*/

	// Use this for initialization
	void Start () {
		/*
		zoom_target = new Vector3[15];
		zoom_time = new float[15];*/
		my_camera = GameObject.Find("Main Camera");
	}
	/*
	void Update() {
		if (p_end != p_start) StartCoroutine (ZoomProcess ());
	}
	*/
	
	// new zooming operation
	public void Zoom(Vector3 target, float time_length) {
		StartCoroutine (ZoomProcess (target, time_length));
		/*
		int new_p = (p_end + 1) % list_length;
		zoom_target [new_p] = target;
		zoom_time [new_p] = time_length;
		p_end = new_p;
		*/
	}

	// zoom process
	IEnumerator ZoomProcess(Vector3 target, float time_length) {
		//Debug.Log("zoom");
		Vector3 delta = (target - my_camera.transform.position) * (1 / time_length);
		// zoom
		for (float rest_time = time_length; rest_time > 0; rest_time -= Constant.TIMESTEPJUMP) {
			my_camera.transform.position += delta * Constant.TIMESTEPJUMP;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		my_camera.transform.position = target;
	}
}
