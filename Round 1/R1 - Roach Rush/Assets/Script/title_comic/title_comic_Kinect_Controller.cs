using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class title_comic_Kinect_Controller : MonoBehaviour {
	// kinect input
	public KinectPointController kinect_man;
	// basic parameters;
	float foot_baseline, foot_lowerbound, time_total;
	// foot information
	float[][] foot_times, foot_Ys;
	int[] foot_point;
	float peopleheight;
	// hands and feet
	GameObject[] foot;
	Animator[] footanim;
	int showcount;
	// wait for updata
	bool waitingForSet;

	public tiltle_comic_main_controller comicmain;
	// Use this for initialization
	void Start () {
		waitingForSet = true;
		// find limbs
		foot = new GameObject[2];
		foot[0] = GameObject.Find("footL");
		foot[1] = GameObject.Find("footR");
		footanim = new Animator[2];
		footanim [0] = GameObject.Find("footl").GetComponent<Animator> ();
		footanim [1] = GameObject.Find("footr").GetComponent<Animator> ();
		showcount = 0;
		// foot
		foot_times = new float[2][];
		foot_times[0] = new float[60000];
		foot_times[1] = new float[60000];
		foot_Ys = new float[2][];
		foot_Ys[0] = new float[60000];
		foot_Ys[1] = new float[60000];
		foot_point = new int[2];
		foot_point [0] = foot_point [1] = 0;
		time_total = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (comicmain.playing) return ;
		// set base, things before kinect work
		if (waitingForSet) {
			if (kinect_man.Head.transform.position.y != -0.6f) {
				waitingForSet = false;
				Debug.Log (kinect_man.Head.transform.position.y);
				setBaseline();
			} else return ;
		}
		
		// update data for foot
		time_total += Time.deltaTime;

		updatafoot (0, kinect_man.Foot_Left.transform.position);
		updatafoot (1, kinect_man.Foot_Right.transform.position);
		// show limbs
		showcount ++;
		if (showcount == Constant.showframesize) {
			showcount = 0;
			showfoot(foot[0], kinect_man.Foot_Left.transform.position);
			showfoot(foot[1], kinect_man.Foot_Right.transform.position);
		}
	}
	
	// update dataset for foot
	void updatafoot(int ty, Vector3 p) {
		float height = p.y;
		// add new data
		while (foot_point[ty] > 0 && foot_Ys[ty][foot_point[ty] - 1] < height)
			foot_point [ty] --;
		foot_Ys [ty][foot_point [ty]] = height;
		foot_times [ty][foot_point [ty]] = time_total;
		foot_point [ty]++;
		/* for testing
		for (int i = 0; i < foot_point [ty]; i++) aw.Write(foot_Ys[ty][i] + " ");
		aw.WriteLine(); */
		
		// detect step on
		if (height <= foot_lowerbound + 0.03) {
			int l = foot_point[ty];
			// clear the set, waiting for new movement
			foot_point[ty] = 0;
			// if it is a step on
			if (foot_Ys[ty][0] > foot_baseline) {
				// detect the speed
				float v = 0, flag = (foot_Ys[ty][0] - foot_lowerbound) / 2.0f;
				for (int i = 0; i < l; i++) {
					if (foot_Ys[ty][i] - foot_lowerbound < flag) break;
					float vnow = (foot_Ys[ty][i] - foot_lowerbound) / (time_total - foot_times[ty][i]);
					if ( vnow > v) v = vnow;
				}
				// send message
				// Debug.Log("step on happen x = " + p.x + " , speed = " + v + " , time = " + time_total);
				float nx = transX_comic(p.x);
				// roach.transform.position = new Vector3(nx, 2, 50);
				//float[] data = new float[2];
				//data[0] = nx; data[1] = v;
				footanim[ty].SetTrigger("raise");
				comicmain.stepon(nx);
			}
		}
	}
	void showfoot(GameObject foot, Vector3 pos) {
		float nx = transX_comic (pos.x);
		foot.transform.position = new Vector3 (nx, 5, 10);
	}
	
	// set basic things
	void setBaseline() {
		// set baseline for foot detect
		float floor = (kinect_man.Foot_Left.transform.position.y + kinect_man.Foot_Right.transform.position.y) / 2;
		float head = kinect_man.Head.transform.position.y;
		foot_lowerbound = floor;
		foot_baseline = (head - floor) * Constant.foot_delta + floor; 
		peopleheight = head - floor;
		
		Debug.Log ("foot lowerbound = " + foot_lowerbound);
		Debug.Log ("foot_baseline = " + foot_baseline);
	}
	// transform y  foot - heand =>  -60 -> 30
	public float transX_comic(float x) { return 30- (x + 1 ) / 2 * 90;}
}
