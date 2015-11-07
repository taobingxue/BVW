using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ui_controller : MonoBehaviour {
	// timer
	public Sprite[] images;
	int resttime;
	GameObject min, se1, se2;
	// emotion
	public Sprite[] faces;
	public float[] threshold;
	public Sprite brokenedge;
	GameObject medge, faceobj, happiness, brokenhp;
	//
	public int[] phototime;
	GameObject photos;

	// Use this for initialization
	void Start () {
		// timer
		resttime = 120;
		min = GameObject.Find ("min");
		se1 = GameObject.Find ("se1");
		se2 = GameObject.Find ("se2");

		// emotion
		medge = GameObject.Find ("Medge");
		faceobj = GameObject.Find ("faces");
		happiness = GameObject.Find ("happiness");
		brokenhp = GameObject.Find ("brokenhp");

		happiness.GetComponent<Image> ().type = Image.Type.Filled;
		brokenhp.GetComponent<Image> ().type = Image.Type.Filled;
		happiness.GetComponent<Image> ().fillMethod = Image.FillMethod.Horizontal;
		brokenhp.GetComponent<Image> ().fillMethod = Image.FillMethod.Horizontal;
		happiness.GetComponent<Image> ().fillAmount = Constant.happyinit / Constant.happyupbound;
		brokenhp.GetComponent<Image> ().fillAmount = 0;
		//
		photos = GameObject.Find ("savephoto");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// decrease rest time
	public void decTime() {
		resttime --;
		if (resttime < 0) {
			Application.LoadLevel(3);
			return;
		}

		min.GetComponent<Image> ().sprite = images [resttime / 60];
		se1.GetComponent<Image> ().sprite = images [(resttime % 60)/10];
		se2.GetComponent<Image> ().sprite = images [resttime % 10];
		// photo
		foreach (int i in phototime) if (resttime == i) photos.SendMessage("save");
	}
	// new happiness
	public void hp(float newhp) {
		float p = newhp / Constant.happyupbound;
		p = p > 2 ? 2 : p;
		Constant.finalscore = (int)(p * 100);
		// show the face
		for (int i=0; i<6; i++)
			if (p <= threshold [i]) {
				faceobj.GetComponent<Image> ().sprite = faces [i];
				break;
			}
		// show the bar
		if (p > 1) {
			medge.GetComponent<Image> ().sprite = brokenedge;
			brokenhp.GetComponent<Image> ().fillAmount = p - 1;
			p = 1;
		}
		happiness.GetComponent<Image> ().fillAmount = p;

		// if die
		if (p <= 0.0f) {
//			Debug.Log ("die");
		}
	}
}
