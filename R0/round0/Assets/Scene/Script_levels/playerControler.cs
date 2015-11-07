using UnityEngine;
using System.Collections;
using System;

public class playerControler : MonoBehaviour {
	// force put on while operate
	public float force = 5f;
	public string tags = "0123";
	// wall sounds
	AudioSource audioS;
	AudioClip[] ac;
	// the wall set that is going to be changde
	GameObject wallset;
	// our player
	GameObject player;
	// speak object
	GameObject speak;
	// speed limit
	const float speedLimit = 5000;
	
	// Use this for initialization
	void Start () {
		// initial audio things
		player = GameObject.FindGameObjectWithTag("player");
		audioS = player.AddComponent<AudioSource>();
		ac = new AudioClip[4];
		for (int i = 0; i < 4; i++)
			ac[i] = Resources.Load("Sound " + i) as AudioClip;
		// initial speaker
		speak = GameObject.Find("Speak");

	}
	
	// Update is called once per frame
	void Update () {
		HandleInput();
	}
	
	// Move if operate~
	void HandleInput() {
		// if it won, do nothing, waiting for new position
		if (check()) {
			player.GetComponent<Rigidbody>().isKinematic = true;
			return ;
		} else {
			player.GetComponent<Rigidbody>().isKinematic = false;

		}
		// find the direction
		Vector3 forceDirection = Vector3.zero;
		if (Input.GetKey (KeyCode.UpArrow)) forceDirection = Vector3.forward;
		if (Input.GetKey (KeyCode.DownArrow)) forceDirection = Vector3.back;
		if (Input.GetKey (KeyCode.LeftArrow)) forceDirection = Vector3.left;
		if (Input.GetKey (KeyCode.RightArrow)) forceDirection = Vector3.right;
		// pull it 
		if (forceDirection != Vector3.zero) GetComponent<Rigidbody>().AddForce(forceDirection * force, ForceMode.VelocityChange);
		float length = GetComponent<Rigidbody>().velocity.sqrMagnitude;
		if (length > speedLimit) {
			length = speedLimit/length;
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * length;
		}
		// Debug.Log(GetComponent<Rigidbody>().velocity);
	}
	
	// make sound and make them appear if hit a wall
	void OnCollisionEnter(Collision collision) {
		// if hit Debug.Log("hit");
		if (collision.gameObject.GetComponent<Renderer>().material.color.a > 0.1) return ;
		
		// speak
		if (Constant.flag == 0) {
			speak.SendMessage("speaking", "What is that ?", SendMessageOptions.RequireReceiver);
			Constant.flag += 1;
		} else if (Constant.flag == 1) {
			speak.SendMessage("speaking", "Magic Wall ?", SendMessageOptions.RequireReceiver);
			Constant.flag = 5;
		}

		tag = collision.gameObject.tag;
		if (tag.Length == 1 && tags.IndexOf(tag) > -1) {
			// find walls maze
			int num = Convert.ToInt32(tag);
			audioS.clip = ac[num];
			audioS.Play();
			tag = tag + tag;
			wallset = GameObject.FindGameObjectWithTag(tag);
			
			// change thire opacity
			foreach (Transform child in wallset.transform) {
				Color colornow = child.GetComponent<Renderer>().material.color;
				child.GetComponent<Renderer>().material.color = new Color(colornow.r, colornow.g, colornow.b, 1);
			}
		}
	}
	
	// if is winning
	bool check() {
		Vector3 pos = player.transform.position;
		if (pos.x < Constant.xMin - 30 || pos.x > Constant.xMax + 30 || pos.z < Constant.yMin - 30 || pos.z > Constant.yMax + 30) return true;
		return false;
	}
}
