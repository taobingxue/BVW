using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class main_controller : MonoBehaviour {
	////////////////////////SFX////////////////////////
	public AudioClip stepSFX;
	public AudioClip snapSFX;
	public AudioSource AS;

	private int randomNum;
	private bool bfHitCockroach;
	////////////////////////  /////////////////////////

	///////////////////Game Desgin////////////////////
	public float gdM1Begin;
	public float gdM1RepeatRate;

	public float gdM2Begin;
	public float gdM2RepeatRate;

	public float gdRoutineBegin;
	public float gdRoutineFRepeatRate;
	public float gdRoutineWRepeatRate;

	public float gdC1Begin;
	public float gdC1RepeatRate;
	public float gdC1RepeatTime;
	public int gdC1Amount;

	public float gdC2Begin;
	public float gdC2RepeatRate;
	public float gdC2RepeatTime;
	public int gdC2Amount;

	public float gdB2Begin;

	public float gdCu1Begin;
	public float gdCu1RepeatRate;
	public float gdCu1RepeatTime;
	public int gdCu1Amount;

	public float gdCu2Begin;
	public float gdCu2RepeatRate;
	public float gdCu2RepeatTime;
	public int gdCu2Amount;

	float gameStartTime;
	float gameCurrentTime;
	////////////////////////  /////////////////////////

	// light for control
	GameObject dirlight;
	// king
	GameObject roachking;
	// status mantain
	float timeCounter;
	int timeShow;
	GameObject myui;
	System.Random rand;
	// prefab
	public GameObject cockroachPrefab;
	// GF
	GameObject girlfriend;

	// Use this for initialization
	void Start () {
		AS = GetComponent<AudioSource> ();

		girlfriend = GameObject.Find("Girlfriend");
		dirlight = GameObject.Find ("Directional light");
		roachking = GameObject.Find("RoachKing");
		myui = GameObject.Find("Canvas");
		timeCounter = 120;
		timeShow = 120;
		rand = new System.Random ();

		roachking.SetActive (false);
		//roachking.SendMessage("setactive");
		///////////////////Game Desgin////////////////////
		gameStartTime = Time.time;

		StartCoroutine ("spark", 20);
		StartCoroutine ("cockraochKingShowUp");

//		StartCoroutine("gdM1");
//		StartCoroutine("gdM2");
//
//		StartCoroutine("gdRoutineFloor");
//		StartCoroutine("gdRoutineWall");
//
//		StartCoroutine("gdC1");
//		StartCoroutine("gdC2");
//
//		StartCoroutine("gdCu1");
//		StartCoroutine("gdCu2");

		InvokeRepeating("gdM1", gdM1Begin, gdM1RepeatRate);
		InvokeRepeating ("gdM2", gdM2Begin, gdM2RepeatRate);

		InvokeRepeating("gdRoutineFloor", gdRoutineBegin, gdRoutineFRepeatRate);
		InvokeRepeating ("gdRoutineWall", gdRoutineBegin, gdRoutineWRepeatRate);

		InvokeRepeating ("gdC1", gdC1Begin, gdC1RepeatRate);
		InvokeRepeating ("gdC2", gdC2Begin, gdC2RepeatRate);

		InvokeRepeating ("gdCu1", gdCu1Begin, gdCu1RepeatRate);
		InvokeRepeating ("gdCu2", gdCu2Begin, gdCu2RepeatRate);
		////////////////////////  /////////////////////////

	}
	
	// Update is called once per frame
	void Update () {
		timeCounter -= Time.deltaTime;
		if (timeCounter < timeShow) {
			timeShow --;
			myui.SendMessage("decTime");
		}

		// testing
		if (Input.GetKeyDown (KeyCode.Return))
//			generateOne (true);
			generateOne (true,true);
		if (Input.GetKeyDown (KeyCode.A))
//			generateOne (false);
			generateOne (false,true);
		if (Input.GetKeyDown (KeyCode.B))
			generateGroup (25, 0.5f);
		if (Input.GetKeyDown (KeyCode.C))
			generateGroup (25, 0.5f, true);

		////////////////////////SFX////////////////////////
		// Bf hit cockroach, disgusting randomly
		if (bfHitCockroach) {
			if (!AS.isPlaying) {
				GameObject.Find ("bfSFX").GetComponent<BfSound> ().bfDisgusting();
				bfHitCockroach = false;
			}
		}
		////////////////////////  /////////////////////////
		///////////////////Game Desgin////////////////////
		gameCurrentTime = Time.time - gameStartTime;

		if (gameCurrentTime > gdM2Begin && gameCurrentTime < gdM2Begin+1f)	CancelInvoke("gdM1");
		if (gameCurrentTime > gdC1Begin && gameCurrentTime < gdC1Begin+1f)	CancelInvoke("gdM2");
		if (gdC1RepeatTime<=0)	CancelInvoke("gdC1");
		if (gdC2RepeatTime<=0)	CancelInvoke("gdC2");
		if (gdCu1RepeatTime<=0)	CancelInvoke("gdCu1");
		if (gdCu2RepeatTime<=0)	CancelInvoke("gdCu2");
		////////////////////////  /////////////////////////

	}

	// generate a group of bugs, ratio for wall is ratio
	void generateGroup(int sum, float ratio = 0, bool notonedge = false) {
		int a = (int)(ratio * sum);
		for (int i = 0; i < a; i++)
			generateOne (false, notonedge);
		sum -= a;
		for (int i = 0; i < sum; i++)
			generateOne (true, notonedge);
	}
	
	// generate one randomly
	// ty == true -> on the floor
	void generateOne(bool ty, bool notonedge = false){
		// rand number
		int x = rand.Next (Constant.xMin, Constant.xMax);
		int y = rand.Next (Constant.yMin, Constant.yMax);
		int z = rand.Next (Constant.zMin, Constant.zMax);
		int selectX;
		if (rand.Next (2) == 0) selectX = Constant.xMin;
		else selectX = Constant.xMax;
		// edit by type
		if (ty)	y = Constant.floory;
		else z = Constant.wallz;
		if (notonedge) {
			if (ty) {
				if (rand.Next(2) == 0) x = selectX;
				else z = Constant.zMax;
			} else {
				if (rand.Next(10) < 8) x = selectX;
				else y = Constant.yMax;
			}
			/*if (rand.Next(2) == 0) x = selectX;
			else if (ty) z = Constant.zMax;
			else y = Constant.yMax;*/
		}
		// create
		GameObject now = Instantiate (cockroachPrefab);
		now.transform.position = new Vector3 (x, y, z);
		if (ty)
			now.tag = "F";
		else {
			now.tag = "W";
			Vector3 v = new Vector3(0, 0, 90);
			now.transform.rotation = Quaternion.Euler(v);
		}
	}

	// generate one with coordinate
	void generateWithLocation(bool ty, float x, float y, float z) {
		GameObject now;
		if (ty) {
			if (!Constant.limitZoneFloor.Contains(new Vector2(x, z))) {
				Debug.Log (x + "," + y + "," + z + "for ty = " + ty + " is out of the range");
				return ;
			}
			y = Constant.floory;
			now = Instantiate (cockroachPrefab);
			now.tag = "F";
		} else {
			if (!Constant.limitZoneWall.Contains(new Vector2(x, y))) {
				Debug.Log (x + "," + y + "," + z + "for ty = " + ty + " is out of the range");
				return ;
			}
			z = Constant.wallz;
			now = Instantiate (cockroachPrefab);
			now.tag = "W";
			Vector3 v = new Vector3(0, 0, 90);
			now.transform.rotation = Quaternion.Euler(v);
		}
		now.transform.position = new Vector3 (x, y, z);
	}

	// step on 
	public void stepon(float[] data) {
		float x = data[0]; float velocity = data[1];
		GameObject[] bugs = GameObject.FindGameObjectsWithTag("F");
		foreach (GameObject bug in bugs)
			if (Math.Abs (bug.transform.position.x - x) < Constant.steprange) {
				bug.SendMessage ("die");
				bfStepSound();
			}
	}
	// push
	public void push(Vector2 p) {
		// kill roach
		GameObject[] bugs = GameObject.FindGameObjectsWithTag("W");
		Vector3 pv3 = new Vector3 (p.x, p.y, 0);
		Vector3 vec;
		foreach (GameObject bug in bugs) {
			vec = pv3 - bug.transform.position;
			vec.z = 0;
			if (vec.magnitude < Constant.pushradius) {
				bfSnapSound();
				bug.SendMessage("pushon");
			}
		}
		// if hit girlfriend
		RaycastHit hit;
		Physics.Raycast (new Vector3 (p.x, p.y, 150), Vector3.back, out hit, 180);
		if (hit.collider != null && Constant.isGF(hit.collider.gameObject)) girlfriend.SendMessage ("hit");
	}

	// turn off the light -> scene become all black for duration seconds
	IEnumerator turnofflight(float duration) {
		RenderSettings.ambientLight = Color.black;
		yield return new WaitForSeconds(duration);
		RenderSettings.ambientLight = Color.white;
	}
	// spark
	IEnumerator spark(float duration) {
		yield return new WaitForSeconds(gdCu1Begin);
		while (duration > 0) {
			float timelength = rand.Next (3, 12) / 100.0f;
			float intens = rand.Next (150, 250) / 1000.0f;
			dirlight.GetComponent<Light>().intensity = intens;
			yield return new WaitForSeconds(timelength);
			duration -= timelength;
			
			timelength = rand.Next (10, 50) / 100.0f;
			intens = rand.Next (10, 55) / 1000.0f;
			float color = rand.Next(30, 80) / 100.0f;
			dirlight.GetComponent<Light>().intensity = intens;
			RenderSettings.ambientLight = Color.white * color;
			yield return new WaitForSeconds(timelength);
			RenderSettings.ambientLight = Color.white;
			duration -= timelength;
			
			timelength = rand.Next (3, 12) / 100.0f;
			intens = rand.Next (60, 150) / 1000.0f;
			dirlight.GetComponent<Light>().intensity = intens;
			yield return new WaitForSeconds(timelength);
			duration -= timelength;
			
			timelength = rand.Next (30, 80) / 100.0f;
			color = rand.Next(1, 150) / 1000.0f;
			RenderSettings.ambientLight = Color.white * color;
			yield return new WaitForSeconds(timelength);
			RenderSettings.ambientLight = Color.white;
			duration -= timelength;
			
			dirlight.GetComponent<Light>().intensity = 0.05f; 
		}
	}
	///////////////////////SFX////////////////////////
	void bfStepSound(){
		//could be rewrite by collision		
		bfHitCockroach = true;
		AS.clip = stepSFX;
		AS.volume = 0.5f;
		AS.Play ();
	}

	void bfSnapSound(){
		//Debug.Log ("Snap a cockroach!");
		bfHitCockroach = true;
		AS.clip = snapSFX;
		AS.volume = 0.3f;
		AS.Play ();
	}
	////////////////////////  /////////////////////////

	///////////////////Game Desgin////////////////////
	// Main 1 generate on the floor every 2 sec.
	void gdM1(){
		Debug.Log("generate M1 cockroach.");
		generateOne (true,true);
	}

	void gdM2(){
		Debug.Log("generate M2 cockroach.");
		generateOne (false,true);
	}

	void gdRoutineFloor(){
		Debug.Log("generate RoutineF cockroach.");
		generateOne (true,true);
	}
	
	void gdRoutineWall(){
		Debug.Log("generate RoutineW cockroach.");
		generateOne (false,true);
	}

	void gdC1(){
		Debug.Log("generate C1 cockroach.");
		generateGroup (gdC1Amount, 0f, true);
		gdC1RepeatTime--;
	}

	void gdC2(){
		Debug.Log("generate C2 cockroach.");
		generateGroup (gdC2Amount, 1f, true);
		gdC2RepeatTime--;
	}

	void gdCu1(){
		Debug.Log("generate Cu1 cockroach.");
		generateGroup (gdCu1Amount, 0.8f, true);
		gdCu1RepeatTime--;
	}
	
	void gdCu2(){
		Debug.Log("generate Cu2 cockroach.");
		generateGroup (gdCu2Amount, 0.8f, true);
		gdCu2RepeatTime--;
	}

//	IEnumerator gdM1(){
//		yield return new WaitForSeconds(gdM1Begin);
//		generateOne (true,true);
//		if (Time.time - gameStartTime < gdM2Begin) {
//			yield return new WaitForSeconds (gdM1RepeatRate);
//			StartCoroutine ("gdM1");
//		}
//	}
//	
//	IEnumerator gdM2(){
//		yield return new WaitForSeconds(gdM2Begin);
//		generateOne (false,true);
//		if (Time.time - gameStartTime < gdRoutineBegin) {
//			yield return new WaitForSeconds (gdM2RepeatRate);
//			StartCoroutine ("gdM2");
//		}
//	}
//	
//	IEnumerator gdRoutineFloor(){
//		yield return new WaitForSeconds(gdRoutineBegin);
//		generateOne (true,true);
//		yield return new WaitForSeconds (gdRoutineFRepeatRate);
//		StartCoroutine ("gdRoutineFloor");
//	}
//	
//	IEnumerator gdRoutineWall(){
//		yield return new WaitForSeconds(gdRoutineBegin);
//		generateOne (false,true);
//	}
//	
//	IEnumerator gdC1(){
//		yield return new WaitForSeconds(gdC1Begin);
//		generateGroup (gdC1Amount, 0f, true);
//		gdC1RepeatTime--;
//	}
//	
//	IEnumerator gdC2(){
//		yield return new WaitForSeconds(gdC2Begin);
//		generateGroup (gdC2Amount, 1f, true);
//		gdC2RepeatTime--;
//	}
//	
//	IEnumerator gdCu1(){
//		yield return new WaitForSeconds(gdCu1Begin);
//		generateGroup (gdCu1Amount, 0.8f, true);
//		gdCu1RepeatTime--;
//	}
//	
//	IEnumerator gdCu2(){
//		yield return new WaitForSeconds(gdCu2Begin);
//		generateGroup (gdCu2Amount, 0.8f, true);
//		gdCu2RepeatTime--;
//	}

	IEnumerator cockraochKingShowUp(){
		yield return new WaitForSeconds(gdB2Begin);
		Debug.Log ("set active cockking");
		// roachking.SendMessage ("setactive");
		roachking.SetActive (true);
	}
}
