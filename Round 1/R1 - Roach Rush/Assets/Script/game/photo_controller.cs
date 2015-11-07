using UnityEngine;
using System.Collections;

public class photo_controller : MonoBehaviour {
	public DeviceOrEmulator DoE;
//	private Kinect.KinectInterface kinect;
	public DisplayColor dc;

	bool working;
	int savepoint;

	// Use this for initialization
	void Start () {
		GameObject obj = GameObject.Find ("KinectPrefab");
		DoE = obj.GetComponent<DeviceOrEmulator> ();
		working = DoE.useEmulator;

		obj = GameObject.Find("ColorImagePlane");
		dc = obj.GetComponent<DisplayColor> ();
		// kinect = DoE.getKinect();
		savepoint = 0;

		Constant.photos = new Color32[10][];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void save() {
		if (working) return ;
		Debug.Log("save" + savepoint);
		// Constant.photos [savepoint] = kinect.getColor ();
		Constant.photos [savepoint] = dc.tex.GetPixels32 ();
		// photos [savepoint] = Color32 [640 * 480];
		savepoint ++;
	}
}
