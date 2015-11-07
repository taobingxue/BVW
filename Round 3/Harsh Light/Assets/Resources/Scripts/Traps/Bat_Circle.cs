using UnityEngine;
using System.Collections;

public class Bat_Circle : MonoBehaviour {
	public GameObject BatPrefab;
	public int numofbat;
	public float speed_min, speed_max, size, speed;
	// Use this for initialization

	void Start () {
		// create bats
		for (int i = 0; i < numofbat; i++) {
			GameObject newbat = Instantiate(BatPrefab, new Vector3 (-30, -30, 0), Quaternion.identity) as GameObject;
			newbat.GetComponent<Bat>().speed = Random.Range(speed_min, speed_max);
			newbat.GetComponent<Bat>().distance = size * Random.Range(0.8f, 1.2f);
			newbat.transform.parent = this.transform;
		}
		Debug.Log ("STARTED!");
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += Vector3.left * speed * Time.deltaTime;
		if (!Constant.spacelimit.Contains (new Vector2 (this.transform.position.x, this.transform.position.y)))
			Destroy (this.gameObject);
	}

	public void init(int _numofbat, float _speed_min, float _speed_max, float _size, float _speed){
		numofbat = _numofbat;
		speed_max = _speed_min;
		speed_max = _speed_max;
		size = _size;
		speed = _speed;
		Debug.Log ("INITIALIZED!");
	}
}
