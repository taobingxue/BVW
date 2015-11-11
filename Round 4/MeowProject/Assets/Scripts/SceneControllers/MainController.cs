using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainController : MonoBehaviour {
	public bool PlayBeginning;
	//===============================
	public AudioClip runningsteps;
	public AudioClip bgm1;
	public AudioClip bgm2;
	public AudioClip bgm3;
	public AudioClip boatsound;

	/* 11/1 Luna
     *Things I have made change
     *bgm music*3
     *delete the cat talk
     *soundmanager add the script of mute background music
     *Delete 2 shrimps otherwise foreach no receiver
     *LiquidBanana soundflag(no water drips when pass the level)
		*/
	//========================
	// status
	string[,] words = {{"", ""}, {"Oh no! It’s too far!! Move Me Closer!", "You know you need to press the trigger to move the painting, right?"},
						{"I’m stuck!!\nCan u inverse the gravity?", "Just rotate to get me out!!"},
						{"There is not enough water! I need more water!Make it rain!", "Look, it’s rainy over there! Just get it here!"}, 
						{"Shrimps are blocking my way!! We need more star bombs!!", "Just shake them off!!"},
						{"Kill me or kill the dog!!", "Till the table!!"}};
	string[] names = {"openning", "aftermoving", "afterrotating", "crossriver", "starfall", "Winn"};
	Sprite[] begin_image;
	Sprite[] go_die;
	int p = 0;
	// controllers;
	JumpController jumper;
	CameraController main_camera;
	// characters
	GameObject cat, dog;

	// lose my patient
	Vector3 posForUI;
	int rest_shrimp = 2;
	float time_for_shaking = 0;
	public Sprite happycat;
	// Use this for initialization
	void Start () {
		jumper = GameObject.Find("JumpController").GetComponent<JumpController>();
		main_camera = GameObject.Find ("CameraController").GetComponent<CameraController> ();
		cat = GameObject.Find("screemcat");
		dog = GameObject.Find("dog");

		//  sound background music1
		SoundManager.instance.PlayBgm (bgm1,true);
		//=================================

		posForUI = GameObject.Find ("talking_bubble5").transform.position;
		if (PlayBeginning) {
			begin_image = new Sprite[311];
			begin_image = Resources.LoadAll<Sprite> ("beginning");
		}
		go_die = new Sprite[191];
		go_die = Resources.LoadAll<Sprite> ("dog_die");
		Debug.Log (go_die.Length);
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log ("p="  + p);
		if (Constant.moveEnabled == false) return ;
		if (Input.GetKeyDown (KeyCode.A))
			Next ();
		else if (p == 1) {
			GameObject pt1 = GameObject.Find ("movePaint_1");
			GameObject pt2 = GameObject.Find ("movePaint_2");
			if ((pt1.transform.position - pt2.transform.position).magnitude < 32)
				Next ();
		} else if (p == 2) {
			Vector3 rotation2 = GameObject.Find ("movePaint_2").transform.rotation.eulerAngles;
			if (rotation2.z > 15   && rotation2.z < 90)
				Next ();
		} else if (p == 4) {
			Vector3 rotation_star = GameObject.Find ("movePaint_4").transform.rotation.eulerAngles;
			if (GameObject.Find("MotionController").GetComponent<MotionController>().getShake() || Input.GetKey(KeyCode.S) || (rotation_star.z > 30 && rotation_star.z < 330)) 
				time_for_shaking += Time.deltaTime;
			else time_for_shaking = 0;
			if (time_for_shaking > Constant.starWaitinTime) {
				time_for_shaking = 0;
				GameObject.Find("cat" + rest_shrimp).SendMessage("fall");
				rest_shrimp ++;
				if (rest_shrimp == 5) Next();
			}
		} else if (p == 5) {
			Vector3 rotation_supper = GameObject.Find ("movePaint_5").transform.rotation.eulerAngles;
			if (rotation_supper.z > 15 && rotation_supper.z <= 90) KillCat();
			else if (rotation_supper.z > 90 && rotation_supper.z <= 345) Next();
		}
	}

	// next animation
	public void Next() {
		Debug.Log ("next: " + names[p]);
		StopCoroutine("CountDown");
		dog.SendMessage("Idle");
		if (p > 0 && p < 6) GameObject.Find ("talking_bubble" + p).SetActive (false);
		StartCoroutine (names[p++]);
	}

	public void Restart() {
		StartCoroutine("CountDown");
		if (p == 4) dog.SendMessage("Climb");
		Constant.moveEnabled = true;
	}

	IEnumerator CountDown() {
		Debug.Log("count down ~~~~~~~~~~~~~~~~" + p);
		// change position
		GameObject pos = GameObject.Find ("bubble_pos" + p);
		GameObject talk = GameObject.Find ("talking_bubble" + p);
		GameObject text1 = GameObject.Find ("Text_" + p);
		text1.GetComponent<Text> ().text = words[p, 0];
		Vector3 vec = talk.transform.position;
		vec.z = 35.25f;
		//Debug.Log(vec);
		//Debug.Log(Camera.main.ScreenToWorldPoint (vec));
		//Debug.Log(Camera.main.WorldToScreenPoint (Camera.main.ScreenToWorldPoint (vec)));
		pos.transform.position = Camera.main.ScreenToWorldPoint (vec);
		talk.GetComponent<Image>().color = Color.white;
		text1.GetComponent<Text>().color = Color.white;

		Vector3 pos0dog = dog.transform.position;
		Vector3 pos0pic = GameObject.Find ("movePaint_" + p).transform.position;
		Quaternion rot0pic = GameObject.Find ("movePaint_" + p).transform.rotation;
		// keep position
		if (p < 4) dog.SendMessage("Climb");
		if (p == 5) dog.SendMessage("play");
		for (float rest_time = Constant.timeToDie; rest_time > 0; rest_time -= Constant.TIMESTEPJUMP) {
			// keep position
			if (p == 1 || p == 2 || p == 5) {
				talk.transform.position = Camera.main.WorldToScreenPoint (pos.transform.position);
				talk.transform.rotation = pos.transform.rotation;
			}
			yield return new WaitForSeconds (Constant.TIMESTEPJUMP);
			// move dog
			Vector3 dis = cat.transform.position - dog.transform.position;
			float ll = dis.magnitude;
			if (ll > Constant.catchDis)
				dog.transform.position += dis * ((ll - Constant.catchDis) / ll) * (1 / rest_time) * Constant.TIMESTEPJUMP;
			else
				break;
			// random talk
		}
		// final talk
		text1.GetComponent<Text> ().text = words[p, 1];
		yield return new WaitForSeconds (0.5f);
		Constant.moveEnabled = false;
		GameObject.Find ("EndingController").GetComponent<EndingCOntroller> ().Ending (1);
		yield return new WaitForSeconds (0.6f);
		// reset everything
		if (p != 5)
			dog.transform.position = pos0dog;
		else
			dog.transform.position = GameObject.Find ("DogTarget4").transform.position;
		GameObject.Find ("movePaint_" + p).transform.position = pos0pic;
		GameObject.Find ("movePaint_" + p).transform.rotation = rot0pic;
		talk.GetComponent<Image>().color = new Color(1, 1, 1, 0);
		text1.GetComponent<Text>().color = new Color(1, 1, 1, 0);
		vec.z = 0;
		talk.transform.position = vec;
		talk.transform.rotation = Quaternion.Euler (Vector3.zero);

		if (p == 3) {
			Debug.Log("reset !!!!!!!!!!!!!!!");
			GameObject.Find ("LiquidBanana").GetComponent<LiquidBanana> ().reset ();
		}
		dog.SendMessage("Idle");
	}

	public void KillCat() {
		StopCoroutine("CountDown");
		StartCoroutine ("KillItself");
	}

	IEnumerator KillItself() {
		Debug.Log("kill cat");
		StopCoroutine("CountDown");
		Constant.moveEnabled = false;
		float speed = 0.25f;
		float rotspeed = 25;
		GameObject things = GameObject.Find ("ThingsOnTable");
		GameObject copied = Instantiate (things);
		copied.transform.parent = things.transform.parent;
		copied.SetActive (false);
		for (int i = 0; i < 60; ++i) {
			if (i == 30) {
				GameObject.Find("Text_5").GetComponent<Text>().text = "Why kill me ?!!";
			}
			foreach (Transform tran in things.transform) {
				tran.localPosition += Vector3.left * speed;
				if (tran.localPosition.x < -8)
					tran.localPosition += Vector3.down * speed;
				if (tran.localPosition.x < -9)
					tran.gameObject.SetActive(false);
				if (tran.tag != "apple") continue;
				Vector3 rote = tran.rotation.eulerAngles;
				rote.z += 25;
				tran.rotation = Quaternion.Euler(rote);
			}
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		GameObject.Find ("EndingController").GetComponent<EndingCOntroller> ().Ending (0);
		yield return new WaitForSeconds (0.6f);
		Destroy (things);
		copied.SetActive (true);
		copied.name = "ThingsOnTable";
		GameObject.Find ("movePaint_5").transform.rotation = Quaternion.Euler (0, 0, 0);
		copied.transform.localPosition = Vector3.zero;
		copied.transform.localScale = Vector3.one;
		copied.transform.localRotation = Quaternion.Euler (0, 0, 0);

		GameObject talk = GameObject.Find ("talking_bubble5");
		GameObject text1 = GameObject.Find ("Text_5");
		talk.GetComponent<Image>().color = new Color(1, 1, 1, 0);
		text1.GetComponent<Text>().color = new Color(1, 1, 1, 0);
		talk.transform.position = posForUI;
		talk.transform.rotation = Quaternion.Euler (Vector3.zero);
	}

	// zoom after delay_time
	IEnumerator ZoomAfter(Vector3 target, float time_l, float time_delay) {
		yield return new WaitForSeconds (time_delay);
		main_camera.Zoom (target, time_l);
	}
	// set moveable
	IEnumerator SetMoveable(float time_delay) {
		yield return new WaitForSeconds (time_delay);
		Constant.moveEnabled = true;
		StartCoroutine("CountDown");
	}

	// openning animation
	IEnumerator openning() {
		Constant.moveEnabled = false;
		Debug.Log ("open");
		//===================================

		// talking ~~
		Debug.Log ("beginning");
		//yield return new WaitForSeconds (6);
		GameObject myimage = GameObject.Find("begin_sprite");
		if (PlayBeginning) {
			for (int i = 0; i < begin_image.Length; ++i) {
				myimage.GetComponent<SpriteRenderer> ().sprite = begin_image [i];
				yield return new WaitForSeconds (0.04f);
			}
		}

		// fading
		Debug.Log("fade");
		float fadetime = 1;
		Color c = myimage.GetComponent<SpriteRenderer> ().color;
		for (float rest_time = fadetime; rest_time > 0; rest_time -= Constant.TIMESTEPJUMP) {
			c.a = rest_time / fadetime;
			myimage.GetComponent<SpriteRenderer>().color = c;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		/*
		 * moving
		Debug.Log ("move");
		cat.SendMessage("play");
		float movingspeed = 0.3f;
		float goa = GameObject.Find("Target00").transform.position.x;
		//====================================
		SoundManager.instance.PlaySfx (runningsteps, true);

		float len = goa - cat.transform.position.x;
		while (cat.transform.position.x < goa) {
			// chang dog color
			Color c = dog.GetComponent<SpriteRenderer>().color;
			c.a = 1 - (goa - cat.transform.position.x) / len;
			dog.GetComponent<SpriteRenderer>().color = c;
			// move cat
			cat.transform.position += Vector3.right * movingspeed;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}*/

		dog.SendMessage("play");
		cat.SendMessage("play");
		//====================================
		SoundManager.instance.PlaySfx (runningsteps, true);
		// running
		Debug.Log ("run");
		float runningtime = 2;
		float runningspeed = 0.02f;
		GameObject screem_bg = GameObject.Find("screemcat_bg");

		bool flag = true;
		for (float rest_time = runningtime; rest_time > 0; rest_time -= Constant.TIMESTEPJUMP) {
			// run bg
			Vector2 vec = screem_bg.GetComponent<Renderer>().material.GetTextureOffset("_MainTex");
			vec += Vector2.right * runningspeed;
			screem_bg.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", vec);

			// zoom camera
			if (flag && rest_time < 1) {
				flag = false;
				main_camera.Zoom(Constant.farShot, 0.8f);
			}
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		//===================================
		SoundManager.instance.PlaySfx (null, false);
	
		// jump
		Vector3 target = GameObject.Find ("Target10").transform.position;
		jumper.Jump (cat, 3, cat.transform.position.y + 3 - target.y, target.x - cat.transform.position.x);
		StartCoroutine (ZoomAfter (Constant.closeView1, 1, 2));
		StartCoroutine (SetMoveable(3.8f));

		// move dog
		float goa = GameObject.Find("Target00").transform.position.x;
		float movingspeed = 0.3f;
		while (dog.transform.position.x < goa) {
			dog.transform.position += Vector3.right * movingspeed;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		cat.transform.parent = GameObject.Find ("movePaint_1").transform;
	}

	// jump from moving to ratatin
	IEnumerator aftermoving() {
		Constant.moveEnabled = false;
		Debug.Log ("start animation");
		// turn
		Vector3 vec = cat.transform.localScale;
		vec.x *= -1;
		cat.transform.localScale = vec;
		// cat jump
		Debug.Log ("cat jump");
		Vector3 ngoa = GameObject.Find ("Target11").transform.position;
		jumper.Jump (cat, 3, cat.transform.position.y + 3 - ngoa.y, ngoa.x - cat.transform.position.x);
		yield return new WaitForSeconds (0.5f);
		// dog jump
		Debug.Log ("dog jump");
		Vector3 dogG = GameObject.Find ("Target10").transform.position;
		Debug.Log ("dog " + dog.transform.position + ", goa " + dogG);
		jumper.Jump (dog, 3, dog.transform.position.y + 3 - dogG.y, dogG.x - dog.transform.position.x, 1.4f);
		/*
		// move
		Debug.Log ("cat move");
		float goa = GameObject.Find ("Target10").transform.position.x;
		float movingspeed = 0.3f;
		while (cat.transform.position.x > goa) {
			cat.transform.position -= Vector3.right * movingspeed;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		*/
		// jump

		StartCoroutine (SetMoveable(3.8f));
		main_camera.Zoom (Constant.farView2, 1);
		StartCoroutine (ZoomAfter (Constant.closeView2, 1, 2));
		GameObject pt2 = GameObject.Find("movePaint_2");
		cat.transform.parent = pt2.transform;
	}

	// jump from afterrotating to monalisa
	IEnumerator afterrotating() {
		Constant.moveEnabled = false;
		Debug.Log("start animation");
		// move
		GameObject boat = GameObject.Find("boat");
		boat.SendMessage("play");
		Debug.Log("cat move");
		float movingspeed = 0.3f;
		float goa = GameObject.Find ("Target20").transform.position.x;
		Vector3 dir = GameObject.Find ("Target20").transform.position - cat.transform.position;
		dir.z = 0;
		dir = dir.normalized;

		while (cat.transform.position.x > goa) {
			cat.transform.position += dir * movingspeed;
			boat.transform.position += dir * movingspeed;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		// dog jump
		Debug.Log("dog jump");
		GameObject dog = GameObject.Find("dog");
		Vector3 dogG = GameObject.Find ("Target20").transform.position;
		jumper.Jump (dog, 3, dog.transform.position.y + 3 - dogG.y, dogG.x - dog.transform.position.x);

		// turn
		Debug.Log("cat stand up");
		float rotatespeed = 1;
		cat.transform.parent = null;
		float angle = GameObject.Find ("movePaint_2").transform.rotation.eulerAngles.z;

		for (int i = 0; i < angle; ++i) {
			Vector3 vec = cat.transform.rotation.eulerAngles;
			vec.z -= rotatespeed; if (vec.z < 0) vec.z = 0;
			cat.transform.rotation = Quaternion.Euler(vec);
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}

		// jump
		boat.SendMessage("sStop");
		boat.transform.parent = cat.transform;
		Debug.Log("cat jump");
		Vector3 ngoa = GameObject.Find ("Target21").transform.position;
		float xx = ngoa.x - cat.transform.position.x;
		if (xx < 0) xx *= -1;
		jumper.Jump (cat, 3, cat.transform.position.y + 3 - ngoa.y, ngoa.x - cat.transform.position.x);
		main_camera.Zoom (Constant.farView3, 1);
		StartCoroutine (ZoomAfter (Constant.closeView3, 1, 2));
		StartCoroutine (SetMoveable(8f));

		// release boat
		yield return new WaitForSeconds (3);
		boat.transform.parent = null;
		Vector3 vec2 = boat.transform.position;
		vec2.x = cat.transform.position.x;
		boat.transform.position = vec2;
		boat.transform.rotation = Quaternion.Euler (0, 0, 0);
		// jump dog
		yield return new WaitForSeconds(1);
		Debug.Log("dog jump");
		dog = GameObject.Find("dog");
		dogG = GameObject.Find ("DogTarget1").transform.position;
		jumper.Jump (dog, 3, dog.transform.position.y + 3 - dogG.y, dogG.x - dog.transform.position.x);
	}

	// cross the river
	IEnumerator crossriver() {
		Constant.moveEnabled = false;
		Debug.Log("cross river");
		GameObject boat = GameObject.Find("boat");

		//======================================
//		SoundManager.instance.PlayDialoge(null);
		SoundManager.instance.PlaySfx (boatsound,false);

		boat.SendMessage("play");
		//move
		Debug.Log("move boat & cat");
		float movingspeed = 0.25f;
		float goa = GameObject.Find ("Target30").transform.position.x;

		GameObject camera = GameObject.Find("Main Camera");

		while (cat.transform.position.x > goa) {
			cat.transform.position -= Vector3.right * movingspeed;
			boat.transform.position -= Vector3.right * movingspeed;
			camera.transform.position -= Vector3.right * movingspeed;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}

		// jump
		Debug.Log ("dog jump");
		Vector3 dogG = GameObject.Find ("DogTarget2").transform.position;
		jumper.Jump (dog, 3, dog.transform.position.y + 3 - dogG.y, dogG.x - dog.transform.position.x);
		// wait
		yield return new WaitForSeconds(0.1f);
		// jump
		Debug.Log ("cat jump");
		Vector3 ngoa = GameObject.Find ("Target31").transform.position;
		boat.SendMessage("sStop");
		boat.transform.parent = cat.transform;
		jumper.Jump (cat, 4, cat.transform.position.y + 4 - ngoa.y, ngoa.x - cat.transform.position.x);
		main_camera.Zoom (Constant.farView4, 1);
		StartCoroutine (ZoomAfter (Constant.closeView4, 1, 2));
		StartCoroutine (SetMoveable(4));

		yield return new WaitForSeconds(3.3f);
		dogG = GameObject.Find ("DogTarget3").transform.position;
		jumper.Jump (dog, 3, dog.transform.position.y + 3 - dogG.y, dogG.x - dog.transform.position.x);

		// wait to arrive
		while (boat.transform.position.x - ngoa.x > 0.01f)
			yield return new WaitForSeconds (Constant.TIMESTEPJUMP);

		//=========================change bgm to bgmusic2
		SoundManager.instance.PlayBgm (bgm2,true);
		//change bgm LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLuna


		Debug.Log ("arrive");
		boat.transform.parent = null;
		Vector3 pos = boat.transform.position;
		pos.x = cat.transform.position.x;
		pos.y = cat.transform.position.y - 3.22f;
		boat.transform.position = pos;
		boat.SendMessage("play");
		// swim
		Debug.Log("swim");
		Transform trans = GameObject.Find ("shrimps").transform;
		foreach (Transform tran in trans) tran.gameObject.SendMessage("Swim");
		yield return new WaitForSeconds (2.6f);
		GameObject obj = GameObject.Find("movePaint_4");
		Debug.Log("rotate");
		for (int i = 0; i < 10; ++i) {
			Vector3 vec = obj.transform.rotation.eulerAngles;
			vec.z += 0.8f;
			obj.transform.rotation = Quaternion.Euler(vec);
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		GameObject.Find("cat1").SendMessage("fall");
		/*for (int i = 0; i < 10; ++i) {
			Vector3 vec = obj.transform.rotation.eulerAngles;
			vec.z -= 0.8f;
			obj.transform.rotation = Quaternion.Euler(vec);
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}*/
		dog.SendMessage("Climb");
	}

	//star fall
	IEnumerator starfall() {
		Constant.moveEnabled = false;
		yield return new WaitForSeconds (Constant.starFallingTime);
		// move
		GameObject boat = GameObject.Find("boat");
		Debug.Log("move boat & cat");
		float movingspeed = 0.25f;
		float goa = GameObject.Find ("Target40").transform.position.x;
		Vector3 dir = GameObject.Find ("Target40").transform.position - cat.transform.position;
		dir.z = 0;
		dir = dir.normalized;
		
		while (cat.transform.position.x > goa) {
			cat.transform.position += dir * movingspeed;
			boat.transform.position += dir * movingspeed;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		boat.SendMessage("sStop");
		// jump out of picture
		Debug.Log ("cat jump");
		Vector3 ngoa = GameObject.Find ("Target41").transform.position;
		jumper.Jump (cat, 3, cat.transform.position.y + 3 - ngoa.y, ngoa.x - cat.transform.position.x, 1.35f);
		main_camera.Zoom (Constant.closeView50, 2);


		// fix dog
		dog.SendMessage("sStop");
		dog.transform.rotation = Quaternion.Euler (0, 0, 0);
		dog.SendMessage("play");

		// move dog
		Debug.Log("dog move");
		float time = 4f;
		goa = GameObject.Find ("DogTarget5").transform.position.x;
		dir = GameObject.Find ("DogTarget5").transform.position - dog.transform.position;
		dir.z = 0;
		float speed = dir.magnitude / time / 50;
		Debug.Log (dir.magnitude);
		Debug.Log ("speed + " + speed);
		dir = dir.normalized;

		Debug.Log(goa);
		Debug.Log("X = " + dog.transform.position.x);
		while (dog.transform.position.x > goa) {
			dog.transform.position += dir * speed;
			Debug.Log("X = " + dog.transform.position.x);
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}


		Debug.Log ("cat jump");
		ngoa = GameObject.Find ("cat5").transform.position + Vector3.up * 3.6f;
		jumper.Jump (cat, 3, cat.transform.position.y + 3 - ngoa.y, ngoa.x - cat.transform.position.x, 1.35f);
		yield return new WaitForSeconds(0.2f);
		Debug.Log ("dog jump");
		jumper.Jump (dog, 8, 8, 0.1f, 2.2f);
		yield return new WaitForSeconds(2);
		Debug.Log ("dog jump");
		jumper.Jump (dog, 8, 8, 0.1f, 2.2f);
		yield return new WaitForSeconds(2);

		
		Debug.Log ("cat jump");
		ngoa = GameObject.Find ("Target42").transform.position;
		jumper.Jump (cat, 3, cat.transform.position.y + 3 - ngoa.y, ngoa.x - cat.transform.position.x, 1.35f);
		GameObject.Find("cat5").SendMessage("fall");
		main_camera.Zoom (Constant.closeView51, 1.5f);
		yield return new WaitForSeconds(0.2f);
		Debug.Log ("dog jump");
		jumper.Jump (dog, 8, 8, 0.1f, 2.2f);
		yield return new WaitForSeconds(3.5f);

		Debug.Log ("cat jump");
		ngoa = GameObject.Find ("Target43").transform.position;
		jumper.Jump (cat, 3, cat.transform.position.y + 3 - ngoa.y, ngoa.x - cat.transform.position.x, 1.35f);

		//
		main_camera.Zoom (Constant.closeViewCat, 1.5f);
		yield return new WaitForSeconds(0.15f);
		GameObject.Find ("angrydog").GetComponent<SpriteRenderer> ().color = Color.white;
		dog.SendMessage("hide");
		dog.transform.localScale *= 1.3f;
		yield return new WaitForSeconds(0.5f);
		dog.transform.localScale *= 1 / 1.3f;
		yield return new WaitForSeconds(0.3f);
		// shake
		Vector3 vec = dog.transform.rotation.eulerAngles;
		for (int i = 0; i < 6; ++i) {
			vec += Vector3.forward * 2;
			dog.transform.rotation = Quaternion.Euler(vec);
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		for (int i = 0; i < 12; ++i) {
			vec -= Vector3.forward;
			dog.transform.rotation = Quaternion.Euler(vec);
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		for (int i = 0; i < 12; ++i) {
			vec += Vector3.forward;
			dog.transform.rotation = Quaternion.Euler(vec);
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		for (int i = 0; i < 6; ++i) {
			vec -= Vector3.forward;
			dog.transform.rotation = Quaternion.Euler(vec);
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		//
		yield return new WaitForSeconds(0.5f);
		Debug.Log ("dog jump");
		ngoa = GameObject.Find ("DogTarget4").transform.position;
		jumper.Jump (dog, 3, dog.transform.position.y + 3 - ngoa.y, ngoa.x - dog.transform.position.x, 3);
		GameObject fireL = GameObject.Find ("fireL");
		GameObject fireM = GameObject.Find ("fireM");
		fireL.GetComponent<SpriteRenderer> ().color = Color.white;
		main_camera.Zoom (Constant.closeView5, 1);
		for (float rest_time = 1.68f; rest_time > 0; rest_time -= 0.04f) {
			Color c = fireL.GetComponent<SpriteRenderer> ().color;
			c.a = 1 - c.a;
			fireL.GetComponent<SpriteRenderer> ().color = c;
			c = fireM.GetComponent<SpriteRenderer> ().color;
			c.a = 1 - c.a;
			fireM.GetComponent<SpriteRenderer> ().color = c;
			yield return new WaitForSeconds(0.04f);
		}
		Transform painting = GameObject.Find ("movePaint_5").transform;
		cat.transform.parent = painting;
		dog.transform.parent = painting;
		for (int i = 0; i < 8; ++i) {
			vec = painting.transform.rotation.eulerAngles;
			vec.z -= 1 - 0.15f * i;
			painting.transform.rotation = Quaternion.Euler(vec);
			GameObject.Find("ThingsOnTable").transform.position += Vector3.right;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		for (int i = 7; i > 0; --i) {
			vec = painting.transform.rotation.eulerAngles;
			vec.z += 1 - 0.15f * i;
			painting.transform.rotation = Quaternion.Euler(vec);
			//GameObject.Find("ThingsOnTable").transform.position += Vector3.right;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		fireL.SetActive(false);
		fireM.SetActive(false);
		GameObject.Find ("angrydog").SetActive(false);
		dog.SendMessage("show");
		StartCoroutine (SetMoveable(0.1f));
		//LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLuna
		SoundManager.instance.PlayBgm (bgm3,true);
		//LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLuna
	}

	// final
	IEnumerator Winn() {
		Debug.Log("Win");
		StopCoroutine("CountDown");
		Constant.moveEnabled = false;
		float speed = 0.25f;
		float rotspeed = 25;
		// dogs
		GameObject things = GameObject.Find ("ThingsOnTable");
		GameObject dogdie = GameObject.Find("dogdie");
		GameObject dogfly = GameObject.Find("dogfly");
		Debug.Log("mvoe");
		for (int i = 0; i < 60; ++i) {
			foreach (Transform tran in things.transform) {
				tran.localPosition += Vector3.right * speed;
				if (tran.localPosition.x > 8)
					tran.localPosition += Vector3.down * speed;
				if (tran.localPosition.x > 9.1f)
					tran.gameObject.SetActive(false);
				if (tran.tag != "apple") continue;
				Vector3 rote = tran.rotation.eulerAngles;
				rote.z += 25;
				tran.rotation = Quaternion.Euler(rote);
			}
			if (i == 20) {
				Debug.Log("change");
				dogdie.GetComponent<SpriteRenderer>().color = Color.white;
				dogdie.transform.parent = null;
				dogfly.transform.parent = null;
				dog.SetActive(false);
				Vector3 vec = dogdie.transform.position;
				main_camera.Zoom(new Vector3(vec.x, vec.y - 6, 5), 0.8f);
			}
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}

		// dog die
		Debug.Log ("gou die !!!!");
		for (int i = 0; i < go_die.Length; ++i) {
			dogdie.GetComponent<SpriteRenderer>().sprite = go_die[i];
			yield return new WaitForSeconds(0.025f);
		}
		// fly
		dogfly.GetComponent<SpriteRenderer> ().color = Color.white;
		main_camera.Zoom (Constant.closeView7, 1);
		cat.GetComponent<SpriteRenderer> ().sprite = happycat;
		Vector3 goa = GameObject.Find("TargetDie").transform.position;
		goa.z = dogfly.transform.position.z;
		Vector3 dir = goa - dogfly.transform.position;
		float xx = goa.x, ll = dir.x, speedstart = ll, speedtop = ll / 3.5f * 4.5f / 5;
		dir *= (1 / ll);
		StartCoroutine (ZoomAfter (Constant.closeView8, 1, 2));
		while (true) {
			float x = dogfly.transform.position.x;
			float speednow = speedtop + Mathf.Abs((xx - x) / ll * (speedstart - speedtop));
			dogfly.transform.position += dir * speednow * Constant.TIMESTEPJUMP;

			if (dogfly.transform.position.x > xx) {
				dogfly.transform.position = goa;
				break;
			}
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		// change pic
		Debug.Log("Change Pictures");
		float time = 2;
		GameObject newpainting = GameObject.Find ("bath_dog");
		Color c = Color.white;
		for (float rest_time = time; rest_time > 0; rest_time -=Constant.TIMESTEPJUMP) {
			float ratio = rest_time / time;
			c.a = ratio;
			dogfly.GetComponent<SpriteRenderer>().color = c;
			c.a = 1 - ratio;
			newpainting.GetComponent<SpriteRenderer>().color = c;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		yield return new WaitForSeconds(1);
	
		// change to happy
		Debug.Log("Run Back");
		main_camera.Zoom (Constant.closeView5, 0.5f);
		yield return new WaitForSeconds (0.55f);
		cat.GetComponent<SpriteRenderer> ().sprite = happycat;
		speed = 0.1f;
		while (cat.transform.localPosition.x < 7.5f) {
			cat.transform.localPosition += Vector3.right * speed;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		// Jump back
		cat.transform.parent = null;
		Debug.Log("Jump Back");
		Vector3 ngoa = GameObject.Find ("TargetEnding").transform.position;
		jumper.Jump (cat, 3, cat.transform.position.y + 3 - ngoa.y, ngoa.x - cat.transform.position.x, 1.35f);
		main_camera.Zoom (Constant.closeView6, 1.5f);
	}
}
