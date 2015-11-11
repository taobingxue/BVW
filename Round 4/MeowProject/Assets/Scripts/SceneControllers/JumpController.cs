using UnityEngine;
using System.Collections;

public class JumpController : MonoBehaviour {
	//=======================
	public AudioClip jumpsound;
	public AudioClip landsound;

	// Use this for initialization
	void Start () {
	}	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
			Jump (GameObject.Find("screemcat"), 3, 21, 16);
	}

	// raise height1, fall height2, go cross width with start speed
	public void Jump(GameObject character, float height1, float height2, float width, float speedstart = -1, float speedtop = -1) {
		if (height2 < 0) {
			height1 += 3 - height2;
			height2 = 3;
		}
		float x1 = Mathf.Sqrt (height1);
		float x2 = Mathf.Sqrt (height2);
		// curve parameter for y = -(1/a)^2(x - b)^2 + c
		float a, b, c;
		a = Mathf.Abs(width) / (x1 + x2);
		b = x1 * a;
		c = height1;
		float xx = width;
		if (width < 0) {
			b *= -1;
			xx *= -1;
		}

		if (speedstart < 0) speedstart = 1 / 4.5f * 3;
		if (speedtop < 0) speedtop = 1 / 3.5f * 3 / 5;
		StartCoroutine (JumpProcess (character, 0, width, speedstart * xx, speedtop * xx, a, b, c));
	}
	
	// jump from xstart to xend from speed
	IEnumerator JumpProcess(GameObject character, float xstart, float xend, float speedstart, float speedtop, float a, float b, float c) {
		float tt = Time.time;

		//====================sound
		SoundManager.instance.PlaySfx (jumpsound, false);

		// stop walking
		DogController d = character.GetComponent<DogController> ();
		int ori = 0;
		if (d != null && d.flag != 0) {
			ori = d.flag;
			d.flag = 0;
		}

		float x = xstart;
		float dir = xstart < xend ? 1 : -1;
		Vector3 pos_character = character.transform.position;
		// jump process
		while (true) {
			// move
			float speednow = speedtop + Mathf.Abs((x - b) / (xstart - b) * (speedstart - speedtop));
			x += dir * speednow * Constant.TIMESTEPJUMP;
			if ((xstart - x) * (xend - x) > 0) x = xend;
			// new pos
			float y = 0 - (x - b) * (x - b) / a / a + c;
			Vector3 new_pos = new Vector3(x, y, 0);
			character.transform.position = pos_character + new_pos;

			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
			if (x == xend) break;
		}
		//start walking
		if (d != null && d.flag != 0) {
			if (ori == 1) d.SendMessage("play");
			else d.SendMessage("Climb");
		}

		//=============================================
		SoundManager.instance.PlaySfx (landsound, false);

		Debug.Log ("jump time: " + (Time.time - tt));
	}
}
