using UnityEngine;
using System.Collections;

public class GF_controller : MonoBehaviour {
	float happiness;
	GameObject myui;

	public Texture[] faces;
	const int F_normal = 0;
	const int F_scared = 1;
	const int F_angry = 2;
	GameObject gf_tex;
	// moving around
	GameObject gf_pos;
	Animator anim;
	// 0-> idle, 1-> walk
	int gf_status;
	float resttime;
	Quaternion[] angles; 
	int dir;
///////////////// GF sound //////////////////
	public AudioClip[] gfScreamSFX;
	public AudioClip[] gfHappySFX;
	public AudioClip[] gfAngrySFX;
	public AudioSource gfAS;
	
	private int randomNum;
	public float coolingtime;


	// Use this for initialization
	void Start () {
		happiness = Constant.happyinit;
		myui = GameObject.Find ("Canvas");
		gf_tex = GameObject.Find ("Girlfriend_tex");
		coolingtime = 0;

		// moving around
		gf_pos = GameObject.Find ("gf_pos");
		anim = GameObject.Find ("Girlfriend").GetComponent<Animator> ();
		gf_status = 0;
		resttime = Constant.timelength [gf_status];
		dir = -1;

		angles = new Quaternion[3];
		angles [0] = Quaternion.Euler (0, -90, 0);
		angles [1] = Quaternion.Euler (0, 0, 0);
		angles [2] = Quaternion.Euler (0, 90, 0);
///////////////// GF sound //////////////////
		gfAS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		coolingtime -= Time.deltaTime;
		if (happiness <= 0) Application.LoadLevel(4);

		// walking around
		if (isfacing ()) return;
		gf_tex.GetComponent<Renderer> ().material.mainTexture = faces [F_normal];
		if (gf_status == 1) gf_pos.transform.localRotation = angles [dir + 1];
		else gf_pos.transform.localRotation = angles [1];

		resttime -= Time.deltaTime;
		if (resttime <= 0) {
			changestatus (Constant.timelength [1 - gf_status]);
			return ;
		}
		// if walking
		if (gf_status == 1) {
			float nx = gf_pos.transform.position.x + dir * Constant.gfspeed * Time.deltaTime;
			if (nx > Constant.gfxMin && nx < Constant.gfxMax) {
				Vector3 vec = gf_pos.transform.position;
				vec.x = nx;
				gf_pos.transform.position = vec;
			} else {
				int newdir = dir * (-1);
				changestatus(Constant.timelength[0] / 2);
				dir = newdir;
			}
		}
	}
	// changestatus with resttime = time
	void changestatus(float time) {
		gf_status = 1 - gf_status;
		resttime = time;
		if (Random.Range (0, 4) == 0) dir *= -1;
		bool flag = gf_status == 1;
		anim.SetBool ("walk", flag);
	}
	// if is making faces
	bool isfacing() {
		return (anim.GetCurrentAnimatorStateInfo (0).IsName ("angry") || anim.GetCurrentAnimatorStateInfo (0).IsName ("scared"));
	}
	
	// gf hit by accident
	public void hit() {
		if (coolingtime > 0) return;
		Debug.Log ("bf hit");
		coolingtime = Constant.touchcooling;
		happiness -= Constant.hitdamage;
		if(!gfAS.isPlaying)
			gfAngrySound ();
		myui.SendMessage ("hp", happiness);
		gf_pos.transform.localRotation = angles [1];
		gf_tex.GetComponent<Renderer> ().material.mainTexture = faces [F_angry];
		anim.SetTrigger("angry");
	}
	
	// gf touch by roach
	public void roach() {
		// Debug.Log ("roach touch");
		happiness -= Constant.roachdamage;
		if(!gfAS.isPlaying)
			gfScreamSound ();
		myui.SendMessage ("hp", happiness);
		gf_pos.transform.localRotation = angles [1];
		gf_tex.GetComponent<Renderer> ().material.mainTexture = faces [F_scared];
		anim.SetTrigger("scared");
	}

	// kill roach
	public void kill(float incvalue) {
		happiness += incvalue;
		if (happiness == 60) {
			gfAS.clip = gfAngrySFX [0];
			gfAS.Play ();
		} else if (happiness == 90) {
			gfAS.clip = gfAngrySFX [1];
			gfAS.Play ();
		}
		myui.SendMessage ("hp", happiness);
	}
///////////////// GF sound //////////////////
	public void gfScreamSound(){
		randomNum = Random.Range (0, gfScreamSFX.Length);
		gfAS.clip = gfScreamSFX [randomNum];
		gfAS.Play ();
	}
	
	public void gfHappySound(){
		randomNum = Random.Range (0, gfHappySFX.Length);
		gfAS.clip = gfHappySFX [randomNum];
		gfAS.volume = 0.8f;
		gfAS.Play ();
	}
	
	public void gfAngrySound(){
		randomNum = Random.Range (0, gfAngrySFX.Length);
		gfAS.clip = gfAngrySFX [randomNum];
		gfAS.volume = 0.8f;
		gfAS.Play ();
	}
}
