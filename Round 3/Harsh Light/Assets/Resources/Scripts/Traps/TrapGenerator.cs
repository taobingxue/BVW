using UnityEngine;
using System.Collections;

public class TrapGenerator : MonoBehaviour {
	public GameObject TrapPrefab;
	public GameObject BallPrefab;
	public GameObject FixhandPrefab;
	public GameObject PillarPrefab;
	public GameObject BatcirclePrefab;
	// Use this for initialization
	void Start () {
		// generate (30, 0, 7.5f, 0, 0, false);
		// generate_closing (0);
		// generate_ball (0, 40, 150);
		// generate_fixedhand (0, 0, 0);
		// generate_pillar (0, 0, 0, 0.2f, 1.6f);
		// generate_pillar (1, 0, 0, 0.2f, 1.6f, true);
		// generate_batcircle (0, 0, 1.5f, 0.5f, 3.5f, 4, 3);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q))
			generate_random (0);
		else if (Input.GetKeyDown (KeyCode.W))
			generate_random (1);
		else if (Input.GetKeyDown (KeyCode.E))
			generate_random_group (10, 0.5f);
		else if (Input.GetKeyDown (KeyCode.A))
			generate_closing_group (1);
		else if (Input.GetKeyDown (KeyCode.S))
			generate_closing_group (5);
		else if (Input.GetKeyDown (KeyCode.Z))
			generate_focus ();
		else if (Input.GetKeyDown (KeyCode.X))
			generate_newball ();
		else if (Input.GetKeyDown (KeyCode.C))
			generate_fixedhand_right_random ();
		else if (Input.GetKeyDown (KeyCode.V))
			generate_pillarfixed_random ();
		else if (Input.GetKeyDown (KeyCode.B))
			generate_pillar_moving ();
		else if (Input.GetKeyDown (KeyCode.N))
			generate_batcircle_random ();
	}
	
	// generate a trap
	GameObject generate(float angle, float xstart, float ystart, float yend = 0, float depth = 0, bool moving = false) {
		GameObject newtrap = Instantiate (TrapPrefab);
		newtrap.GetComponent<Trap>().init (angle, xstart, ystart, yend, moving, Trap_Constant.appear_time, Trap_Constant.grow_time, Trap_Constant.hand_time, Trap_Constant.keep_time, Trap_Constant.fade_time, depth);
		return newtrap;
	}
	// generate a swingball
	GameObject generate_ball(float x, float anglelimit, float speedlimit) {
		GameObject newball = Instantiate (BallPrefab);
		newball.GetComponent<Ball> ().init (x, anglelimit, speedlimit);
		return newball;
	}
	// generate a swingball in the end
	void generate_newball() {
		generate_ball (Trap_Constant.x_max + 1, 40, 150);
	}
	// generate a fixed hand in spesific position with specific angle
	void generate_fixedhand(float _x, float _y, float angle) {
		GameObject newhand = Instantiate (FixhandPrefab);
		Vector3 pos = newhand.transform.position;
		pos.x = _x; pos.y = _y;
		newhand.transform.position = pos;
		Vector3 rotate = newhand.transform.rotation.eulerAngles;
		newhand.transform.rotation = Quaternion.Euler (rotate.x, rotate.y, rotate.z + angle);
	}
	// generate a fixed hand in the right, ty = 0 for sky, ty = 1 for ground
	void generate_fixedhand_right(int ty = 1) {
		if (ty == 1)
			generate_fixedhand (Trap_Constant.x_max + 1, 0, 0); 
		else
			generate_fixedhand (Trap_Constant.x_max + 1, 0, 180); 
	}
	// generate a random fixed hand in the right
	void generate_fixedhand_right_random() {
		if (Random.Range(-1, 1) < 0)
			generate_fixedhand (Trap_Constant.x_max + 1, 0, 0); 
		else
			generate_fixedhand (Trap_Constant.x_max + 1, 0, 180); 
	}
	// generate a pillar
	void generate_pillar(float _x, float _y, float _angle, float timeclose, float timeopen, bool once = false) {
		GameObject pillar = Instantiate (PillarPrefab);
		Vector3 pos = pillar.transform.position;
		pos.x = _x; pos.y = _y;
		pillar.transform.position = pos;
		Vector3 rotate = pillar.transform.rotation.eulerAngles;
		pillar.transform.rotation = Quaternion.Euler (rotate.x, rotate.y, rotate.z + _angle);

		pillar.GetComponent<Pillar> ().once = once;
		pillar.GetComponent<Pillar> ().closet = timeclose;
		pillar.GetComponent<Pillar> ().opent = timeopen;
	}
	// generate a fixed pillar for once
	void generate_pillarfixed(float _x) {
		generate_pillar (_x, 0, 0, 0.2f, 1.6f, true);
	}
	// generate a random pillar for once
	void generate_pillarfixed_random() {
		generate_pillar (Random.Range(Trap_Constant.x_min, Trap_Constant.x_max), 0, 0, 0.2f, 1.6f, true);
	}
	// generate a moving pilar
	void generate_pillar_moving() {
		generate_pillar (Trap_Constant.x_max + 1, 0, 0, 0.2f, 1.6f);
	}
	// generate a batcircle
	void generate_batcircle(float x, float y, float size, float movingspeed, float batspeed_min, float batspeed_max, int numofbat) {
		GameObject newcircle = Instantiate (BatcirclePrefab);
		newcircle.transform.position = new Vector3 (x, y, 0);
		// set parameters
		newcircle.GetComponent<Bat_Circle>().speed = movingspeed;
		newcircle.GetComponent<Bat_Circle>().size = size;
		newcircle.GetComponent<Bat_Circle>().speed_min = batspeed_min;
		newcircle.GetComponent<Bat_Circle>().speed_max = batspeed_max;
		newcircle.GetComponent<Bat_Circle>().numofbat = numofbat;
	}
	// generate a batcircle with specific y
	void generate_batcircle_fixedx(float y) {
		generate_batcircle (Trap_Constant.x_max + 1, y, Random.Range (1.3f, 1.8f), Random.Range (0.5f, 0.9f), 3.5f, 4, (int)Random.Range (3, 6));
	}
	// generate a batcircle randomely
	void generate_batcircle_random() {
		generate_batcircle_fixedx(Random.Range(-4, 4));
	}
	// a random trap, ty = 0 for sky, tu = 1 for ground
	GameObject generate_random(int ty) {
		// angle
		float angle = Random.Range (0, Trap_Constant.angle_limit);
		// up or downs
		if (ty == 1) angle = 180 - angle;
		// left or right
		if (Random.Range (-1, 1) < 0) angle *= -1;
		// ystart
		float ystart = Trap_Constant.y_max;
		if (ty == 1) ystart = Trap_Constant.y_min;
		
		// Debug.Log ("angle = "+ angle);
		// Debug.Log ("ymin = "+ ystart);
		// Debug.Log ("ymax = "+ Trap_Constant.y_norm);
		// generate
		return generate (angle, Random.Range (Trap_Constant.x_min, Trap_Constant.x_max), ystart, Trap_Constant.y_norm);
	}
	// s random trap, ratio of sky = s * ratio
	void generate_random_group(int s, float ratio = 1) {
		int ss = (int)(s * ratio);
		for (int i = 0; i < ss; ++i) generate_random(0);
		for (int i = 0; i < s - ss; ++i) generate_random (1);
	}
	
	// generate a pair of closing trap on specific x
	void generate_closing(float x) {
		generate (0, x, Trap_Constant.y_max, Trap_Constant.y_norm, 0, true);
		generate (180, x, Trap_Constant.y_min, Trap_Constant.y_norm, 0 , true);
	}
	// generate a group of s closing trap
	void generate_closing_group(int s) {
		for (int i = 0; i < s; ++i) generate_closing (Random.Range (Trap_Constant.x_min, Trap_Constant.x_max));
	}
	
	// generate a focus trap
	GameObject generate_focus() {
		// angle
		float angle = Random.Range (0, Trap_Constant.angle_limit);
		// up or downs
		float ystart = Trap_Constant.y_max;
		if (Random.Range (-1, 1) < 0) {
			angle = 180 - angle;
			ystart = Trap_Constant.y_min;
		}
		// left or right
		if (Random.Range (-1, 1) < 0) angle *= -1;
		float ang = angle;
		angle *= Mathf.PI / 180.0f;
		
		Vector3 pos = GameObject.Find ("Fairy").transform.position;
		Vector3 dir = new Vector3 (Mathf.Sin (angle), 0 - Mathf.Cos (angle), 0);
		float xstart = pos.x - (pos.y - ystart) / dir.y * dir.x;
		return generate (ang, xstart, ystart, Trap_Constant.y_norm);
	}
}
