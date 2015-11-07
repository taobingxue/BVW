using UnityEngine;
using System.Collections;

public class player_controller : MonoBehaviour {
	// player
	GameObject player;
	GameObject playerdir;
	leveldefine[] levels;
	// levels & animation for levels
	public GameObject[] modl;
	public GameObject[] anim;
	public bool[] active;
	public GameObject[] goas;
	// is set right, do not again and again
	bool flag_setright = false;
	bool flag_ending = false;
	// Use this for initialization
	void Start () {
		// find objs
		player = GameObject.Find ("player");
		player.transform.position = Constant.startpos;
		Debug.Log ("set to" + player.transform.position.y);
		player.transform.LookAt (Constant.startpos + Constant.startdir);
		playerdir = GameObject.Find ("CenterEyeAnchor");

		// set levels
		setlevles ();
		active = new bool[10];
		for (int i = 0; i < 10; i++) active [i] = false;
	}

	// do not go to ilegal place
	public bool checknewlocation(Vector3 pos) {
		if (leveldefine.levelnow == 0 && (!Constant.limitedZoneforIsland.Contains (new Vector2 (pos.x, pos.z))))
			return false;
		return true;
	}

	// get distance
	public Vector3 getgoa() {
		return levels [leveldefine.levelnow].triggergoa;
	}

	// if level i is triggered now
	bool check(int i) {
		Vector3 pos = GameObject.Find ("RightEyeAnchor").transform.position;

		if (i == 0) {
			if (pos.z <= 8.5f) return true;
			else return false;
		}

		//Debug.Log ("len = "+(pos - levels [i].triggerpos).magnitude);
		//Debug.Log (levels [i].triggergoa);
		//Debug.Log (pos);
		//Debug.Log (levels [i].triggergoa - pos);
		//Debug.Log (playerdir.transform.forward);
		//Debug.Log ("ang = " + Vector3.Angle (levels [i].triggergoa - pos, playerdir.transform.forward));
		if ((pos - levels [i].triggerpos).magnitude > levels [i].triggerdis) return false;
		if (Vector3.Angle (levels [i].triggergoa - pos, playerdir.transform.forward) > levels [i].triggerangle)
			return false;
		return true;
	}
	
	// set all levels
	void setlevles() {
		levels = new leveldefine [6];
		levels[0] = new leveldefine();
		levels [0].triggerpos = new Vector3 (0, 27, 8);
		levels [0].triggergoa = new Vector3 (0, 27, 0);
		levels [0].triggerdis = 0.75f;
		levels [0].triggerangle = 200;

		/*
		levels[1] = new leveldefine();
		levels [1].triggerpos = new Vector3 (0, 1, 0);
		levels [1].triggergoa = new Vector3 (0, 250, 0);
		levels [1].triggerdis = 1.5f;
		levels [1].triggerangle = 30;  */

		//-0.5738193,0.3707313,0.7302669
		levels[1] = new leveldefine();
		levels [1].triggerpos = goas[1].transform.position;
		levels [1].triggerpos.y = Constant.playery;
		levels [1].triggergoa = modl[1].transform.position;
		levels [1].triggerdis = 2f;
		levels [1].triggerangle = 25;

		levels[2] = new leveldefine();
		levels [2].triggerpos = goas[2].transform.position;
		levels [2].triggerpos.y = Constant.playery;
		levels [2].triggergoa = modl[2].transform.position;
		levels [2].triggerdis = 2f;
		levels [2].triggerangle = 25;
		
		levels[3] = new leveldefine();
		levels [3].triggerpos = goas[3].transform.position;
		levels [3].triggerpos.y = Constant.playery;
		levels [3].triggergoa = modl[3].transform.position;
		levels [3].triggerdis = 2f;
		levels [3].triggerangle = 25;

		levels[4] = new leveldefine();
		levels [4].triggerpos = goas[4].transform.position;
		levels [4].triggerpos.y = Constant.playery;
		levels [4].triggergoa = modl[4].transform.position;
		levels [4].triggerdis = 2;
		levels [4].triggerangle = 360;

		levels[5] = new leveldefine();
	}
	
	// Update is called once per frame
	void Update () {
		animtrigger ();
	}

	// trigger animations
	void animtrigger() {
		if (flag_ending) return ;
		// if an old thing is triggered
		for (int i = 1; i < leveldefine.levelnow; i++)
			if (check (i)) {
				if (!GameObject.Find ("leap").GetComponent<leapmove>().ismoving()) StartCoroutine (setright (levels [i].triggerpos));
				sactive(i);
			} else {
				disactive(i);
			}

		if (leveldefine.levelnow == 4) return;
		// if it triggered a new level;
		if (check (leveldefine.levelnow)) {
			if (!GameObject.Find ("leap").GetComponent<leapmove>().ismoving()) StartCoroutine(setright(levels[leveldefine.levelnow].triggerpos));
			sactive(leveldefine.levelnow);
			modl [++leveldefine.levelnow].SetActive (true);

			if (leveldefine.levelnow == 4) {
				Debug.Log ("waitforEnding !!!");
				StartCoroutine("waitforending");
			}
			/*if (leveldefine.levelnow == 5 && (!flag_ending)) {
				Debug.Log ("Ending !!!");
				StopCoroutine("waitforending");
				StartCoroutine("ending");
			}*/
			if (leveldefine.levelnow == 1) {
				GameObject.Find ("leap").GetComponent<leapmove> ().fallingflag = true;
				GameObject.Find ("handprints").SetActive(false);
			}
		}
	}

	// adjust player's location
	IEnumerator setright(Vector3 goa) {
		Debug.Log("setright");
		if (!flag_setright) {
			flag_setright = true;
			int times = (int)(Constant.timetosetright / Constant.steptosetright);
			Vector3 vec = (goa - player.transform.position) * (1.0f / times);
			for (int i = 0; i < times - 1; i ++) {
				player.transform.position += vec;
				yield return new WaitForSeconds (Constant.steptosetright);
			}
			player.transform.position = goa;
			flag_setright = false;
		} else
			yield return null;
	}

	// ending
	IEnumerator waitforending() {
		yield return new WaitForSeconds (Constant.endingwaiting);
		StartCoroutine("ending");
	}
	IEnumerator ending() {
		flag_ending = true;
		Debug.Log("start ending");
		GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();
		// set transparent, remove sprite
		for (int i = 1; i < 4; i++) disactive(i);
		foreach (GameObject obj in objs) {
			if (obj.name != null && (obj.name == "OVRVolumeController(Clone)" || obj.name == "fullhaiku")) continue;
			if (obj.name != null && obj.name == "foot_print") obj.SetActive(false);
			if (obj.name != null && obj.name == "stream fish") obj.SetActive(false);
			if (obj.GetComponent<MeshRenderer> () != null)
				obj.GetComponent<Renderer> ().material.shader = Shader.Find ("Transparent/Diffuse");
			if (obj.GetComponent<ParticleSystem>() != null) {
				ParticleSystem ps = obj.GetComponent<ParticleSystem>();
				Destroy(ps);
			}
		}
		// fade out
		int times = (int)(Constant.endingtime / 0.02f);
		float r = 1.0f;
		for (int i = times; i > 0; i--) {
			float ratio = 1.0f * (i-1) / i;
			r *= ratio;
			Debug.Log ("r = " + r + ", i = " + i);
			foreach (GameObject obj in objs) {
				if (obj.name != null && (obj.name == "OVRVolumeController(Clone)" || obj.name == "fullhaiku")) continue;
				if (obj.GetComponent<MeshRenderer>() == null) continue;

				Material[] mats = obj.GetComponent<Renderer>().materials;
				foreach (Material mat in mats) {
					Color now = mat.color;
					now.a *= ratio;
					mat.color = now;
				}
			}
			yield return new WaitForSeconds(0.02f);
		}
		// ending
		if (GameObject.Find ("fullhaiku") != null) {
			GameObject.Find ("fullhaiku").GetComponent<fullhaiku>().startanim();
		} else Debug.Log ("null");
	}

	// active all animation
	void sactive(int i) {
		if (active [i]) return ;
		Debug.Log("set"+i);
		active [i] = true;
		GameObject obj = anim[i];
		if (obj == null) return ;

		foreach (Transform trans in obj.transform) {
			GameObject subobj = trans.gameObject;
			if (subobj.GetComponent<SpriteRenderer>() != null) subobj.GetComponent<fading2D>().fadein();
			else subobj.SetActive(true);

			if (i == 0 && subobj.name == "bgm") {
				Debug.Log("bgm");
				if (subobj.GetComponent<SpriteRenderer>() == null) Debug.Log(null);
			}
		}
	}
	// disactive all animation
	void disactive(int i) {
		if (active [i] == false) return ;
		active [i] = false;
		GameObject obj = anim [i];
		if (obj == null) return ;

		foreach (Transform trans in obj.transform) {
			GameObject subobj = trans.gameObject;
			Debug.Log (subobj.name);
			if (subobj.GetComponent<SpriteRenderer>() != null) subobj.GetComponent<fading2D>().fadeout();
			//Fade out music instead of disabling it
			//else subobj.SetActive(false);
			else subobj.GetComponent<bgm>().FadeOutAudio();
		}
	}
}
