using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class title_Kinect_Controller : MonoBehaviour {
	public title_main_controller titlemain;
	// kinect input
	public KinectPointController kinect_man;
	// basic parameters;
	float foot_baseline, foot_lowerbound, time_total;
	float peopleheight;
	// hand
	Vector3[] lasthand;
	float[][] handspeed;
	float[] handspeedsum;
	int handpoint;
	// hands and feet
	GameObject[] hand;
	int showcount;
	// wait for updata
	bool waitingForSet;
	// Use this for initialization
	void Start () {
		waitingForSet = true;
		// find limbs
		hand = new GameObject[2];
		hand[0] = GameObject.Find("handL");
		hand[1] = GameObject.Find("handR");
		showcount = 0;
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
		// update data for hand
		updatahand (0, kinect_man.Hand_Left.transform.position);
		updatahand (1, kinect_man.Hand_Right.transform.position);
		handpoint = (handpoint + 1) % Constant.handcheckl;

		showcount ++;
		if (showcount == Constant.showframesize) {
			showcount = 0;
			showhand(hand[0], kinect_man.Hand_Left.transform.position);
			showhand(hand[1], kinect_man.Hand_Right.transform.position);
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

		if (handspeedsum[ty] / Constant.handcheckl > Constant.handspeed) {
			float nx = transXshow(hand.x);
			float ny = transYshow(hand.y);
			// if (ty == 1) Debug.Log ("push " + nx + "," + ny);
			titlemain.push (nx, ny);
		}
		lasthand [ty] = hand;
	}
	// show limbs
	void showhand(GameObject hand, Vector3 pos) {
		float nx = transXshow (pos.x);
		float ny = transYshow (pos.y);
		hand.transform.position = new Vector3 (nx, ny , 10);
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
	// transform y  foot - heand =>  5-> 65
	public float transYshow(float y) { return (y - foot_lowerbound) / peopleheight * 60 + 5;}
	// 37 - -70
	public float transXshow(float x) { return 37 - (x + 1) / 2 * 107;}
}
