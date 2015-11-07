using UnityEngine;
using System.Collections;
using System;

public class roach_king : MonoBehaviour {
	////////////////////////SFX////////////////////////
	public AudioClip cockWalkSFX;
	public AudioClip cockFallSFX;
	public AudioClip[] cockDieSFX;
	public AudioSource cockAS;
	
	private int randomNum; //index for sound random
	////////////////////////  /////////////////////////

	Animator anim;
	// moving direction
	Vector3 dir;
	// speed and angle going to turn
	double speed, angle;
	// target
	GameObject goa;
	// change angle each turnstep
	int count;
	// give birth to babies
	float birthtime;
	// int life
	int restlife;
	// random
	System.Random rand;
	// do not move anymore
	bool moveflag;
	// prefab
	public GameObject kingPrefab;
	public GameObject roachPrefab;
	// Use this for initialization
	
	void Start () {
		cockAS = GetComponent<AudioSource> ();

		restlife = Constant.kinglife;
		this.transform.localScale = new Vector3 (10, 10, 10);
		birthtime = 0;
		// normal roach
		rand = new System.Random ();
		Debug.Log ("start king");
		anim = GetComponent<Animator> ();
		// cockWalkSound ();
		goa = GameObject.Find ("Girlfriend");
		initialdir();
		speed = Constant.normalspeed * 0.75;
		moveflag = true;
	}
	
	void Update() {
		// if is going to die
		if (!moveflag) return ;

		// count step
		count ++;
		// change angle
		if (count == 25) {
			angle = rand.Next (4) / 360.0 * Math.PI;
			// Debug.Log (rand.Next (4));
			if (rand.Next (2) == 0) angle *= -1;
			this.transform.LookAt (this.transform.position - dir);
			count = 0;
		} 
		
		// if inside move, else keep turn
		for (int i = 0; i < 360; i ++) {
			// turn
			double nx = dir.x * Math.Cos (angle) + dir.z * Math.Sin (angle);
			double nz = 0 - dir.x * Math.Sin (angle) + dir.z * Math.Cos (angle);
			dir = new Vector3 ((float)nx, dir.y, (float)nz);
			
			this.transform.LookAt (this.transform.position - dir);
			// move
			Vector3 newp = this.transform.position + dir * (float)speed * Time.deltaTime;

			RaycastHit hit;
			Physics.Raycast (newp, goa.transform.position - newp, out hit, Constant.roacheyesight * 15);
			bool notseeGF = (hit.collider == null || (!Constant.isGF(hit.collider.gameObject)));

			if (Constant.limitZoneKing.Contains (new Vector2 (newp.x, newp.z)) && notseeGF) {
				this.transform.position = newp;
				break;
			} else if (angle == 0) angle = rand.Next (1, 4) / 360.0 * Math.PI;
		}

		// baby roach
		birthtime += Time.deltaTime;
		if (birthtime >= 1.0f) {
			birthtime = 0;
			birth(this.transform.position);
		}
	}

	public void die() {
		Debug.Log ("die king " + restlife);
		if (!moveflag) return ;
		--restlife;
		if (restlife > 0) return;
		Debug.Log ("die");
		moveflag = false;
		// cockDieSound ();
		anim.SetBool ("idle", true);
		goa.SendMessage ("kill", 5.0f);
		StartCoroutine ("laststruggle");
	}
	IEnumerator laststruggle() {
		// thiner
		this.transform.localScale = new Vector3 (10, 1, 10);
		// particle
		GameObject light = Instantiate (kingPrefab);
		light.transform.position = this.transform.position;
		if (this.tag == "W") light.transform.rotation = Quaternion.identity;
		// wait
		yield return new WaitForSeconds (0.5f);
		// babies
		int step = 360 / Constant.birthsum;
		for (int i = 0; i < 360; i += step) {
			Debug.Log ("i= " + i);
			double nowx = 3, nowz = 0, anglenow = i / 180.0 * Math.PI;
			double nx = nowx * Math.Cos (anglenow) + nowz * Math.Sin (anglenow);
			double nz = 0 - nowx * Math.Sin (anglenow) + nowz * Math.Cos (anglenow);

			Vector3 pos = new Vector3((float)nx, Constant.floory, (float)nz);
			birth(pos + this.transform.position);
//			birth (new Vector3((float)nx, Constant.floory, (float)nz));
		}
		yield return new WaitForSeconds (1.0f);
		// remove
		Destroy (light);
		Destroy (gameObject);
	}
	IEnumerator disappear(float duration) {
		yield return new WaitForSeconds (duration);
		Destroy (gameObject);
	}
	
	void initialdir() {
		dir = goa.transform.position - this.transform.position;
		dir.y = 0;
		dir = dir.normalized;
		angle = 0;
		count = 0;
	}

	void birth( Vector3 pos) {
		GameObject baby = Instantiate (roachPrefab);
		baby.tag = "F";
		baby.transform.position = pos;
		baby.transform.localScale = Vector3.one * 1.2f;
		baby.SendMessage("bebaby");
	}
	void changeSpeed(float mult) {
		speed = mult * Constant.normalspeed;
	}

	
	
	
	
	/*
	////////////////////////SFX////////////////////////
	
	void cockWalkSound(){
		cockAS.clip = cockWalkSFX;
		cockAS.loop = true;
		cockAS.volume = 0.4f;
		cockAS.Play ();
	}
	
	void cockDieSound(){
		randomNum = UnityEngine.Random.Range(0, cockDieSFX.Length);
		cockAS.clip = cockDieSFX [randomNum];
		cockAS.loop = false;
		cockAS.volume = 0.7f;
		cockAS.Play ();
	}
	
	void cockFallSound(){
		cockAS.clip = cockFallSFX;
		cockAS.loop = false;
		cockAS.volume = 0.5f;
		cockAS.Play ();
	}*/
}