﻿using UnityEngine;
using System.Collections;

public class LiquidDetector : MonoBehaviour {
	
	
	void OnTriggerEnter2D(Collider2D Hit)
	{
		if (Hit.GetComponent<Rigidbody2D>() != null && Hit.gameObject.name =="Banana(Clone)") {
			transform.parent.GetComponent<LiquidBanana>().Splash(transform.position.x, Hit.GetComponent<Rigidbody2D>().velocity.y*Hit.GetComponent<Rigidbody2D>().mass / 40f);
			Hit.gameObject.GetComponent<SpriteRenderer>().enabled = false;	
		}
	}
	void OnTriggerExit2D(Collider2D Hit) {
		if (Hit.GetComponent<Rigidbody2D>() != null && Hit.gameObject.name =="Banana(Clone)") {
			transform.parent.GetComponent<LiquidBanana>().SetHeightUpdated(false);
			Destroy(Hit.gameObject);
		}
	}
	
}
