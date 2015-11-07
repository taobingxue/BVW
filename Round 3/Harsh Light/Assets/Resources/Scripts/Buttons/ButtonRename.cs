using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonRename : MonoBehaviour {

	public float		x_min, x_max, y_min, y_max;
	MovementViewer		_viewer;

	string[] firstName = {"欧阳", "喵", "meowski", "司空", "慕", "杨", "梁", "たわだ", "しろた", "ふじ", "てずか", "やた", "わたなべ", "かしわぎ", "わだ", "Justin", "냐","저스틴","강남","안과","떡볶이", "Whacky", "Lady", "General", "Sam", "Doctor", "Sir Fart", "Captain", "Majestic" };
	string[] lastName = {"延轩", "汪", "碧落", "紫陌", "红尘", "ミキ", "ゆき", "ヒデヤ", "ゆう", "しゅうすけ", "まゆ", "たくま","옹이", "바보", "스타일","가세요","먹고싶다", "Fischer", "Demon", "Phoenix", "Bubbles", "BigMac", "Vixen", "Snowball", "Pocky", "Makey" };

	AudioSource _audioS;
	Rect		_startSpace;
	float		_standtime;

	void Awake() {
		_audioS = this.GetComponent<AudioSource> ();
		_startSpace = new Rect (x_min, y_min, x_max - x_min, y_max - y_min);
	}
	// Use this for initialization
	void Start () {
		_viewer = GameObject.Find ("viewer").GetComponent<MovementViewer> ();
		Constant.nameFist = "Harsh Light";
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 cursorPosition = _viewer.getPosition();
		if (_startSpace.Contains(cursorPosition)) {
			float newtime = _standtime + Time.deltaTime;
			for (int i = 1; i < 3; i ++) {
				if (_standtime < i && newtime >= i) {
					_audioS.Play();
					if (i == 2) push();
				}
			}
			_standtime = newtime;
		} else _standtime = 0;	
	}
	void push() {
		Constant.nameFist = firstName [(int)Random.Range (0, firstName.Length - 0.01f)] + " " + lastName [(int)Random.Range (0, lastName.Length - 0.01f)];
		GameObject.Find ("name").GetComponent<Text> ().text = Constant.nameFist;
		_standtime = -0.01f;
	}
}
