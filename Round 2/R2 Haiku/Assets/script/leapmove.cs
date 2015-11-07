using UnityEngine;
using System.Collections;
using Leap;
using System.IO;

public class leapmove : MonoBehaviour {
	Controller myController;
	GameObject player;
	GameObject playerdir;
	// start falling
	public bool fallingflag;
	public player_controller pc;
	// time
	float timesum;
	// test
	FileStream aFile;
	StreamWriter aw;
	string dis, angle;
	//Audio objects
	public AudioClip falling, splash;
	AudioSource src;
	bool audioPlaying;
	
	// Use this for initialization
	void Start () {
		// get controller
		GameObject obj = GameObject.Find ("HandControllerCycler");
		HandController handController = obj.GetComponent<HandController> ();
		myController = handController.GetLeapController ();
		// get player
		player = GameObject.Find ("player");
		playerdir = GameObject.Find ("CenterEyeAnchor");
		
		pc = GameObject.Find ("playercontroller").GetComponent<player_controller> ();
		// not falling
		fallingflag = false;

		timesum = 0;
		// testing
		aFile = new FileStream("rows.csv", FileMode.OpenOrCreate);
		aw = new StreamWriter(aFile);
		dis = ""; angle = "";

		//Initialize Audio Source
		src = GetComponent <AudioSource> ();
		audioPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		// startfalling
		if (fallingflag) {
			//Play falling sound
			if (!audioPlaying){
				src.PlayOneShot(falling, 0.7f);
				audioPlaying = true;
			}
			//Movement code
			if (player.transform.position.y <= 8)
				player.transform.position += Constant.fallingspeed * Time.deltaTime * Vector3.down * (player.transform.position.y / 8.0f);
			else 
				player.transform.position += Constant.fallingspeed * Time.deltaTime * Vector3.down;
			if (player.transform.position.y <= Constant.playery) {
				Vector3 vec = player.transform.position;
				vec.y = Constant.playery;
				player.transform.position = vec;
				fallingflag = false;
				//Mute audio after hitting ground
				StartCoroutine(Landing());
			}
			return ;
		}

		// moving if hand is -
		Hand myhand = handExist ();
		if (myhand != null || Input.GetKey (KeyCode.A)) {
			bool moving = Input.GetKey (KeyCode.A) || checkmoving (myhand.PalmPosition);
			timesum += Time.deltaTime;
			if (moving)
				move ();
		} else
			timesum = 0;
	}

	IEnumerator Landing (){
		src.clip = splash;
		src.volume = 0.3f;
		src.Play ();
		yield return new WaitForSeconds (src.clip.length);
		src.Stop ();
	}

	// called by leapmove, y is always zero
	void move() {
		Vector3 pos = player.transform.position; //+playerdir.transform.position + ;

		Vector3 dir = playerdir.transform.forward.normalized;
		dir.y = 0;

		Vector3 goa = pc.getgoa();
		float dis = (goa - pos).magnitude;
		float ang = Vector3.Angle ((goa - pos).normalized, dir);
		//Debug.Log ("ang =" + ang);
		Vector3 newpos = pos + dir * Time.deltaTime * Constant.speed(timesum, dis, ang <= 90);
		
		if (pc.checknewlocation(newpos)) player.transform.position = newpos;
	}

	// if hand exist return the hand
	private Hand handExist() {
		// get hands
		Frame framenow = myController.Frame ();
		HandList handList = framenow.Hands;
		// if exist
		if (handList.Count <= 0) {
			// Debug.Log ("not exist");
			return null;
		} else return handList [0];
	}
	public bool findhand() {
		Frame framenow = myController.Frame ();
		HandList handList = framenow.Hands;
		return handList.Count > 0;
	}
	
	// if is 'moving'
	private bool checkmoving(Vector pos) {
		// get distance and angle
		Vector3 posplayer = player.transform.position;
		Vector3 poshand = new Vector3(pos.x, pos.y, pos.z);
		float distence = (posplayer - poshand).magnitude;
		float ang = Vector3.Angle (poshand - posplayer, GameObject.Find ("player").transform.forward);
		// testing
		dis += distence + ",";
		angle += ang + ",";

		// check
		return distence > Constant.handdischeck;
	}
	public bool ismoving() {
		Hand handnow = handExist ();
		if (handnow == null) return false;
		return checkmoving (handnow.PalmPosition);
	}
	
	// for testing
	void OnApplicationQuit() {
		aw.WriteLine (dis);
		aw.WriteLine (angle);
		aw.Close ();
	}
}
