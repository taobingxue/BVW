using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public float		x_min, x_max, y_min, y_max;
	public int			nextScence;

	MovementViewer		_viewer;

	AudioSource audioS;
	Rect		_startSpace;
	float		_standtime;

	void Awake() {
		audioS = this.GetComponent<AudioSource> ();
		_startSpace = new Rect (x_min, y_min, x_max - x_min, y_max - y_min);
	}
	// Use this for initialization
	void Start () {
		_viewer = GameObject.Find ("viewer").GetComponent<MovementViewer> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 cursorPosition = _viewer.getPosition();
		if (_startSpace.Contains(cursorPosition)) {
			float newtime = _standtime + Time.deltaTime;
			for (int i = 1; i < 4; i ++) {
				if (_standtime < i && newtime >= i) {
					audioS.Play();
					if (i == 3)
						StartCoroutine(push());
				}
			}
			_standtime = newtime;
		} else _standtime = 0;	
	}
	IEnumerator push() {
		Color c = Color.black; 
		GameObject blackbg = GameObject.Find("black");
		for (float rest_time = Constant.fadeouttime; rest_time > 0; rest_time -= Constant.timestep) {
			c.a = 1 - rest_time / Constant.fadeouttime;
			blackbg.GetComponent<SpriteRenderer>().color = c;
			yield return new WaitForSeconds(Constant.timestep);
		}
		Application.LoadLevel (nextScence);
	}
}
