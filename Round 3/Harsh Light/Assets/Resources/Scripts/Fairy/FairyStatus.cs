using UnityEngine;
using System.Collections;

public class FairyStatus : MonoBehaviour {
	public RemoveBodyPart[] removeBodyPartArr;
	int point = 0;
	bool isImmune = false;
	float immuneDuration = 1f;
	float fadeTime = 1f;
	float timesum = 0, timesafe = 0;
	bool NeverSayDie = false;

	AudioSource _sfxLoseLife;

	void Start() {
		Constant.score = 0;
		Constant.winorlose = true;
		_sfxLoseLife = GetComponent<AudioSource>();
	}

	void Update() {
		timesum += Time.deltaTime;
		timesafe += Time.deltaTime;

		if(Input.GetKeyDown(KeyCode.G) && !NeverSayDie){
			NeverSayDie = true;
		}
		else if (Input.GetKeyDown(KeyCode.G) && NeverSayDie){
			NeverSayDie = false;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		// change sprite
		if (point < removeBodyPartArr.Length && collider.tag != "Trigger"){
			if(!isImmune){
				removeBodyPartArr [point].GetComponent<RemoveBodyPart> ().LosePart();
				point++;
				StartCoroutine("CoImmuneDelay");
				// not touch
				Constant.score += (int)((timesafe / 9.0f) * (timesafe / 9.0f) * (timesafe / 9.0f));
				timesafe = 0;
				_sfxLoseLife.Play();
			}
		}else if (collider.tag != "Trigger" && ! NeverSayDie) {
			// final particle

			Debug.Log("Die !!");
			// stop ?
			GameObject bg = GameObject.Find("Environment");
			foreach (Transform trans in bg.transform) {
				MoveBackground obj = trans.gameObject.GetComponent<MoveBackground>();
				if (obj != null) obj.enabled = false;
			}
			this.GetComponent<Movement>().enabled = false;
			// fade
			StartCoroutine("fade");
		}

		if(collider.tag == "Trigger"){
			collider.gameObject.SendMessage("ExecuteTrigger");
		}
	}

	IEnumerator CoImmuneDelay(){
		isImmune = true;
		yield return new WaitForSeconds(immuneDuration);
		isImmune = false;
	}

	IEnumerator fade() {
		Color c = Color.black; 
		GameObject blackbg = GameObject.Find("black");
		for (float rest_time = fadeTime; rest_time > 0; rest_time -= Constant.timestep) {
			c.a = 1 - rest_time / fadeTime;
			blackbg.GetComponent<SpriteRenderer>().color = c;
			yield return new WaitForSeconds(Constant.timestep);
		}
		countscore ();
		Constant.winorlose = false;
		Application.LoadLevel (3);
	}

	public void countscore() {
		// rest lifes
		Constant.score += (removeBodyPartArr.Length - point + 1) * 1437;
		// game time
		Constant.score += (int)(timesum * 59);
		// not touch
		Constant.score += (int)((timesafe / 8.5f) * (timesafe / 9.0f) * (timesafe / 8.0f));
		// count rank
		bool flag = true;
		Constant.rank = -1;
		Constant.nameFist = "";
		for (int i = 1; i <= Constant.ranklength; i++) {
			// get value
			string nameString = PlayerPrefs.HasKey("name"+i) ? PlayerPrefs.GetString("name"+i) : "none";
			int scoreInt = PlayerPrefs.HasKey("score"+i) ? PlayerPrefs.GetInt("score"+i) : -1;
			// judge
			if (flag) {
				if (nameString == "none") {
					flag = false;	Constant.rank = i;
				} else if (scoreInt < Constant.score) {
					flag = false;	Constant.rank = i;
					// move others down
					for (int j = Constant.ranklength; j > i; --j) {
						if (PlayerPrefs.HasKey("name" + (j-1))) {
							string s = PlayerPrefs.GetString("name" + (j-1));
							int c = PlayerPrefs.GetInt("score" + (j-1));
							PlayerPrefs.SetString("name" + j, s);
							PlayerPrefs.SetInt("score" + j, c);
						}
					}
				}
			} else if (nameString == "none") break;
		}
	}
}
