using UnityEngine;
using System.Collections;

public class Constant : MonoBehaviour {
	// player
	public static Vector3 startpos = new Vector3(0, 27f, 17);
	public static Vector3 startdir = new Vector3(0, 0, -1);
	public static float playery = 1;

	// moving
	public static float handdischeck = 180;
	public static float movespeed = 2f;
	public static float speed(float timelength, float distence, bool direction = true) {
		float ratio = 1.8f - 1.6f / timelength;
		if (ratio < 0.25f) ratio = 0.25f;

		float ratiod = 1.0f;
		if (!direction) {
			if (distence > 10) ratiod = 8.0f / distence + 0.05f;
			// if (ratiod < 0.2f) ratiod = 0.2f;
		}

		//Debug.Log (movespeed * ratiod * ratio);
		return movespeed * ratiod * ratio;
	}

	// level0
	public static float fallingspeed = 5;
	public static Rect limitedZoneforIsland = new Rect(-1.5f, 7, 3.2f, 11);

	// trigger
	public static float timetosetright = 0.2f;
	public static float steptosetright = 0.01f;
	public static float fadingtime2D = 2;

	// ending
	public static float endingwaiting = 10;
	public static float endingtime = 2;

}