using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetScore : MonoBehaviour {
	public Sprite whiteviewer;
	// Use this for initialization
	void Awake() {
		if (Constant.rank > -1 && Constant.nameFist == "") {
			Application.LoadLevel(4);
		}
		if (Constant.rank > -1) {
			PlayerPrefs.SetString("name"+Constant.rank, Constant.nameFist);
			PlayerPrefs.SetInt("score"+Constant.rank, Constant.score);
		}

		GameObject.Find("score").GetComponent<Text> ().text = Constant.score + "";
		//PlayerPrefs.DeleteAll ();
	}

	void Start() {
		if (Constant.winorlose) {
			GameObject.Find("lose").SetActive(false);
			GameObject.Find ("SightView(Clone)").GetComponent<SpriteRenderer> ().sprite = whiteviewer;
			GameObject.Find ("focuspoint").GetComponent<SpriteRenderer> ().color = new Color(0, 0, 0, 50.0f/255);
		} else GameObject.Find("win").SetActive(false);


		GameObject.Find ("SightView(Clone)").GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0.88f);
		GameObject.Find ("score").GetComponent<Text> ().color = Constant.winorlose ? Color.black : Color.white;
		for (int i = 1; i <= 8; i ++) {
			// get obj
			GameObject name = GameObject.Find("rank" + i);
			GameObject score = GameObject.Find("score" + i);

			Color c = Constant.winorlose? new Color(0, 0, 0, 1 - 0.12f * (i - 1)) : new Color(1, 1, 1, 1 - 0.12f * (i - 1));
			name.GetComponent<Text>().color = c;
			score.GetComponent<Text>().color = c;
			
			// get value
			string nameString = PlayerPrefs.HasKey("name"+i) ? PlayerPrefs.GetString("name"+i) : "none";
			int scoreInt = PlayerPrefs.HasKey("score"+i) ? PlayerPrefs.GetInt("score"+i) : -1;
			name.GetComponent<Text>().text = nameString;
			score.GetComponent<Text>().text = scoreInt + "";
		}
	}
}

