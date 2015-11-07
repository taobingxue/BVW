using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class cockroach : MonoBehaviour {
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
	// random
	System.Random rand;
	// do not move anymore
	bool moveflag;
	bool falling;
	bool baby, changesize;
	public GameObject particalPrefab;
	// Use this for initialization

	void Start () {
		cockAS = GetComponent<AudioSource> ();

		rand = new System.Random ();
		// Debug.Log ("start");
		anim = GetComponent<Animator> ();
		cockWalkSound ();
		goa = GameObject.Find ("Girlfriend");
		speed = Constant.normalspeed;
		moveflag = true;
		falling = false;
		initialdir();
	}

	void Update() {
		// if is going to die
		if (!moveflag) return ;
		// if is falling
		if (falling) {
			this.transform.position -= new Vector3(0, 10, 0);
			// Debug.Log (this.transform.position.y);
			if (this.transform.position.y < Constant.floory) {
				float xx = this.transform.position.x;
				this.transform.position = new Vector3(xx, Constant.floory, 6);
				// Debug.Log (this.transform.position.y);
				this.transform.rotation = Quaternion.Euler(0, 0, 0);
				falling = false;
				this.tag = "F";
				initialdir();
			}
			return ;
		}

		// count step
		count ++;
		// change angle
		if (count == Constant.turnstep) {
			angle = rand.Next (4) / 360.0 * Math.PI;
			// Debug.Log (rand.Next (4));
			bool flag = Constant.check(dir.x, dir.z,
			                   goa.transform.position.x - this.transform.position.x,
			                           goa.transform.position.z - this.transform.position.z) < 0;
			if (this.tag == "W")
				flag = Constant.check(dir.x, dir.y,
			                      goa.transform.position.x - this.transform.position.x,
			                      goa.transform.position.y - this.transform.position.y ) < 0;
			if (rand.Next (4) == 0 && flag) angle *= -1;
			else if (rand.Next (4) != 3 && flag == false) angle *= -1;
			this.transform.LookAt (this.transform.position - dir);
			if (this.tag == "W") {
				Vector3 vv = this.transform.rotation.eulerAngles;
				if (dir.x > 0) vv.z -= 90;
				else vv.z += 90;
				this.transform.rotation = Quaternion.Euler(vv);
			} 

			count = 0;
		} 

		// if inside move, else keep turn
		for (int i=0; i < 100; i++) {
			// turn
			if (this.tag == "F") {
				double nx = dir.x * Math.Cos (angle) + dir.z * Math.Sin (angle);
				double nz = 0 - dir.x * Math.Sin (angle) + dir.z * Math.Cos (angle);
				dir = new Vector3 ((float)nx, dir.y, (float)nz);
			} else {
				double nx = dir.x * Math.Cos (angle) + dir.y * Math.Sin (angle);
				double ny = 0 - dir.x * Math.Sin (angle) + dir.y * Math.Cos (angle);
				dir = new Vector3 ((float)nx, (float)ny, dir.z);
			}
			this.transform.LookAt (this.transform.position - dir);
			if (this.tag == "W") {
				Vector3 vv = this.transform.rotation.eulerAngles;
				if (dir.x > 0) vv.z -= 90;
				else vv.z += 90;
				this.transform.rotation = Quaternion.Euler(vv);
			}

			// Debug.Log (dir.x + " " + dir.z);
			// move
			Vector3 newp = this.transform.position + dir * (float)speed * Time.deltaTime;
			if (this.tag == "F" && Constant.limitZoneFloor.Contains (new Vector2 (newp.x, newp.z))) {
				this.transform.position = newp;
				break;
			} else if (this.tag == "W" && Constant.limitZoneWall.Contains (new Vector2 (newp.x, newp.y))) {
				if (Constant.limitZonebackWall.Contains(new Vector2(newp.x, newp.y)) == onback()) {
					this.transform.position = newp;
					break;
				}
			} else if (angle == 0) angle = rand.Next (1, 4) / 360.0 * Math.PI;
		}

		// touch GF
		// if hit girlfriend
		RaycastHit hit;
		Physics.Raycast (this.transform.position, goa.transform.position - this.transform.position, out hit, Constant.roacheyesight);
		if (hit.collider != null && Constant.isGF(hit.collider.gameObject)) {
			goa.SendMessage ("roach");
			// do something
			moveflag = false;
			foreach (Transform obj in this.transform)
				obj.GetComponent<Renderer>().material.color = Constant.roachonGF;
			StartCoroutine ("disappear", 1);
		}

		//baby resize
		if (changesize)	this.transform.localScale = Constant.babyroachsize;
	}

	public void pushon() {
		if (moveflag == false || falling) return ;
		if (rand.Next (100) / 100.0 < Constant.probTofall) {
			//fall
			// partical
			this.transform.rotation = Quaternion.Euler (-90, 90, 90);
			cockFallSound ();
			falling = true;
		} else {
			cockDieSound();
			die ();
		}
	}
	public void die() {
		if (!moveflag) return ;
		// Debug.Log ("die");
		moveflag = false;
		cockDieSound ();
		anim.SetBool ("idle", true);
		goa.SendMessage ("kill", Constant.killinc);
		StartCoroutine ("laststruggle");
	}
	IEnumerator laststruggle() {
		// rote
		Vector3 r = this.transform.rotation.eulerAngles;
		r.z += 180;
		this.transform.rotation = Quaternion.Euler (r);
		// particle
		GameObject light = Instantiate (particalPrefab);
		light.transform.position = this.transform.position;
		if (this.tag == "W") light.transform.rotation = Quaternion.identity;
		// wait
		yield return new WaitForSeconds (1);
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
		if (this.tag == "F") {
			this.transform.localScale = Constant.floorsize;
			dir.y = 0;
		} else {
			this.transform.localScale = Constant.wallsize;
			dir.z = 0;
		}
		dir = dir.normalized;
		angle = 0;
		count = 0;
		// baby need resize
		if (baby) changesize = true;
	}
	void changeSpeed(float mult) {
		speed = mult * Constant.normalspeed;
	}
	

	bool onback() {
		Vector3 pos = this.transform.position;
		if (Constant.limitZonebackWall.Contains (new Vector2(pos.x, pos.y))) {
			pos.z = -2.5f;
			this.transform.position = pos;
			return true;
		}
		return false;
	}
	public void bebaby() {
		baby = true;
		changesize = true;
	}
	////////////////////////SFX////////////////////////

	void cockWalkSound(){
		cockAS.clip = cockWalkSFX;
		cockAS.loop = true;
		cockAS.volume = 0.4f;
		cockAS.Play ();
	}

	void cockDieSound(){
		randomNum = Random.Range(0, cockDieSFX.Length);
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
	}
}


