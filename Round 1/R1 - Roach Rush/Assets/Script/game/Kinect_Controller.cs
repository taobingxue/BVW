using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Kinect_Controller : MonoBehaviour {
	// testing
	FileStream aFile;
	StreamWriter aw;
	GameObject roach;
	GameObject[] obj;
	
	GameObject mainContr;
	// kinect input
	public KinectPointController kinect_man;
	// basic parameters;
	float foot_baseline, foot_lowerbound, time_total;
	// foot information
	float[][] foot_times, foot_Ys;
	int[] foot_point;
	float peopleheight;
	// hand information
	/*  QAQ check angle
	Vector3[][] wrists;
	float[][] slops;
	*/
	Vector3[] lasthand;
	float[][] handspeed;
	float[] handspeedsum;
	int handpoint;
	// hands and feet
	GameObject[] hand, foot;
	Animator[] footanim;
	int showcount;
	// wait for updata
	bool waitingForSet;
	// Use this for initialization
	void Start () {
		waitingForSet = true;
		mainContr = GameObject.Find ("main");
		// find limbs
		hand = new GameObject[2];
		foot = new GameObject[2];
		hand[0] = GameObject.Find("handL");
		hand[1] = GameObject.Find("handR");
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
		// hand
		handpoint = 0;
		lasthand = new Vector3[2];
		handspeed = new float[2][];
		for (int i = 0; i < 2; i ++) {
			handspeed [i] = new float[50];
			for (int j = 0; j < 50; j ++) handspeed[i][j] = 0.0f;
		}
		handspeedsum = new float[2];
		handspeedsum [0] = handspeedsum [1] = 0.0f;
		
		// for testing
		aFile = new FileStream("rows.csv", FileMode.OpenOrCreate);
		aw = new StreamWriter(aFile);
		roach = GameObject.Find("cockroach");
		obj = new GameObject[2];
		obj[0] = GameObject.Find("Sphere");
		obj[1] = GameObject.Find("Sphere1");
	}
	
	// Update is called once per frame
	void Update () {
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
		// update data for hand
		updatahand (0, kinect_man.Hand_Left.transform.position);
		updatahand (1, kinect_man.Hand_Right.transform.position);
		handpoint = (handpoint + 1) % Constant.handcheckl;
		/* angle check
		updatahand (0, kinect_man.Wrist_Left.transform.position, kinect_man.Hand_Left.transform.position);
		updatahand (1, kinect_man.Wrist_Right.transform.position, kinect_man.Hand_Right.transform.position);
		// check hand
		handpoint ++;
		if (handpoint == Constant.handcheckl) {
			aw.Write((slops [0] [handpoint - 1] - slops[0][0]) / (time_total - lasttime) + ",");
			lasttime = time_total;
			handpoint = 0;
		}
		*/
		
		// show limbs
		showcount ++;
		if (showcount == Constant.showframesize) {
			showcount = 0;
			showhand(hand[0], kinect_man.Hand_Left.transform.position);
			showhand(hand[1], kinect_man.Hand_Right.transform.position);
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
				float nx = Constant.transX(p.x);
				// roach.transform.position = new Vector3(nx, 2, 50);
				float[] data = new float[2];
				data[0] = nx; data[1] = v;
				mainContr.SendMessage("stepon", data);
				footanim[ty].SetTrigger("raise");
			}
		}
	}
	// updata dataset for hands
	void updatahand(int ty, Vector3 hand) {
		// updata speed set
		handspeedsum[ty] -= handspeed [ty][handpoint];
		Vector3 vec = hand - lasthand [ty];
		float speednow = vec.magnitude / Time.deltaTime;
		handspeedsum [ty] += speednow;
		handspeed [ty] [handpoint] = speednow;
		// check
		aw.WriteLine(time_total + "," + handspeedsum[ty] / Constant.handcheckl + "," + speednow);


		if (handspeedsum[ty] / Constant.handcheckl > Constant.handspeed) {
			float nx = Constant.transX(hand.x);
			float ny = transY(hand.y);
			// if (ty == 1) Debug.Log ("push " + nx + "," + ny);
			mainContr.SendMessage("push", new Vector2(nx, ny));
		}
		lasthand [ty] = hand;
	}
	/*  angle detect
	void updatahand(int ty, Vector3 wrist, Vector3 hand) {
		// new data
		Vector3 nowvec = hand - wrist;
		nowvec.z = 0;
		wrists [ty][handpoint] = wrist;
		slops [ty][handpoint] = (float) Math.Asin (nowvec.y / nowvec.magnitude);
		// check
		if (handpoint == Constant.handcheckl - 1) {
			double speed = Math.Asin(slops[ty][handpoint] - slops[ty][0]) / (time_total - lasttime);
			if (speed > Constant.handspeed) {
				float nx = Constant.transX(hand.x);
				float ny = transY(hand.y);
				mainContr.SendMessage("push", new Vector2(nx, ny));
			}
		}
	}
	*/
	
	// show limbs
	void showhand(GameObject hand, Vector3 pos) {
		float nx = Constant.transX (pos.x);
		float ny = transYshow (pos.y);
		hand.transform.position = new Vector3 (nx, ny - 10, Constant.handz);
	}
	void showfoot(GameObject foot, Vector3 pos) {
		float nx = Constant.transX (pos.x);
		foot.transform.position = new Vector3 (nx, Constant.footy, Constant.footz);
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
		
		lasthand [0] = kinect_man.Hand_Left.transform.position;
		lasthand [1] = kinect_man.Hand_Right.transform.position;
	}
	// transform y  foot - heand =>  5-> 95
	public float transY(float y) { return (y - foot_lowerbound) / peopleheight * 90 + 5;}
	public float transYshow(float y) { return (y - foot_lowerbound) / peopleheight * 80 + 20;}
	// for testing
	void OnApplicationQuit() {
		aw.Close ();
	}
}
