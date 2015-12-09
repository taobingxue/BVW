using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public AudioSource sfxSwingBall;

	[SerializeField]
	float anglelimit, speedlimit;
	int direction = 1;

	void Start (){
		if (anglelimit != null) {
			StartCoroutine ("showball");
		}
	}

	public void init(float _angle, float _speed) {
		anglelimit = _angle;
		speedlimit = _speed;
		StartCoroutine ("showball");
	}

	public void init(float _x, float _angle, float _speed) {
		anglelimit = _angle;
		speedlimit = _speed;
		Vector3 vec = this.transform.position;
		vec.x = _x;
		this.transform.position = vec;
		StartCoroutine ("showball");
	}

	// Update is called once per frame
	void Update () {
		this.transform.position += Vector3.left * Constant.bgspeed * Time.deltaTime;
		if (!Constant.spacelimit.Contains (new Vector2 (this.transform.position.x, this.transform.position.y)))
			Destroy (this.gameObject);
	}

	// Update is called once per frame
	IEnumerator showball() {
		while (true) {
			Vector3 dir = this.transform.rotation.eulerAngles;
			if (dir.z > 180) dir.z = dir.z - 360;
			// if turn
			if (Mathf.Abs (dir.z) > anglelimit) {
				dir.z = (anglelimit - 0.01f) * direction;
				direction *= -1;
			}
			// rotate
			float speed = 1 - Mathf.Abs (dir.z) / anglelimit;
			if (speed < 0.05f) speed = 0.05f;
			speed *= speedlimit;
			dir.z += speed * Constant.timestep * direction;
			this.transform.rotation = Quaternion.Euler (dir);
			//Debug.Log ("Ball Speed: "+speed);
			if(speed > 20){
				PlaySwingBallSfx();
			}

			yield return new WaitForSeconds(Constant.timestep);
		}
	}

	void PlaySwingBallSfx(){
		if(!sfxSwingBall.isPlaying){
			sfxSwingBall.Play();
		}
	}
}
