using UnityEngine;
using System.Collections;

public class ShrimpController : MonoBehaviour {
	Sprite[] image;
	public Sprite cookedshrimp;
	public float dis_to_ship = 15;
	public float time = 2;
	// Use this for initialization
	void Start () {
		image = new Sprite[33];
		image = Resources.LoadAll<Sprite> ("Shrimps");
		this.transform.localScale *= 1.8f;
		//Swim ();
		//StartCoroutine ("ChangeImage");
		//StartCoroutine (SwimToShip ());
	}
	
	void Swim() {
		StartCoroutine (SwimToShip ());
	}

	public void die() {
		// turn cooked
		StopCoroutine ("ChangeImage");
		this.GetComponent<SpriteRenderer> ().sprite = cookedshrimp;
		if (this.gameObject.activeSelf) StartCoroutine("flow");
	}

	IEnumerator ChangeImage() {
		while (true) {
			for (int i = 0; i < image.Length; i++) {
				this.GetComponent<SpriteRenderer>().sprite = image[i];
				yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
			}
		}
	}

	IEnumerator SwimToShip() {
		StartCoroutine ("ChangeImage");
		Vector3 target = GameObject.Find ("Target30").transform.position;
		//target.y -= 3;
		Vector3 dis = target - this.transform.position;
		dis.z = 0;
		float angle = Vector3.Angle (Vector3.up, dis);
		float anglePI = angle / 180 * Mathf.PI;
		if (Vector3.Angle (new Vector3 (Mathf.Sin (anglePI), Mathf.Cos (anglePI), 0), dis) > Vector3.Angle (new Vector3 (Mathf.Cos (anglePI), Mathf.Sin (anglePI), 0), dis))
			angle *= -1;

		Vector3 ang = this.transform.rotation.eulerAngles;
		ang.z = -angle;
		ang.z -= 90;
		this.transform.rotation = Quaternion.Euler (ang);
		//this.transform.localScale = this.transform.localScale * 2;

		float l = 1 - dis_to_ship / dis.magnitude;
		Vector3 now = this.transform.position;
		for (float rest_time = time; rest_time > 0; rest_time -= Constant.TIMESTEPJUMP) {
			float ratio = (1 - rest_time / time) * l;
			this.transform.position = now + dis * ratio;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
	}

	IEnumerator flow() {
		float dis = Random.Range (10, 20);
		float speed = 0.3f;
		for (float rest_time = dis/speed; rest_time>0; rest_time -= 1) {
			Vector3 vec = this.transform.position;
			vec.y += Random.Range(-1, 1) * speed / 2;
			vec.x -= speed;
			this.transform.position = vec;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
	}
}
