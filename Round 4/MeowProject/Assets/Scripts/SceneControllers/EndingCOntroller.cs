using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndingCOntroller : MonoBehaviour {
	GameObject numbers, image;
	public Sprite[] num_image;
	public Sprite[] endingpics;
	// Use this for initialization
	void Start () {
		numbers = GameObject.Find("number");
		image = GameObject.Find("badendings");
	}
	
	public void Ending(int p) {
		image.GetComponent<Image> ().sprite = endingpics [p];
		numbers.GetComponent<Image> ().sprite = num_image [4];
		StartCoroutine("PlayEnding");
	}

	IEnumerator PlayEnding() {
		// fadein
		float time = 0.5f;
		for (float rest_time = time; rest_time > 0; rest_time -= Constant.TIMESTEPJUMP) {
			float ratio = 1 - rest_time / time;
			Color c = image.GetComponent<Image>().color;
			c.a = ratio;
			image.GetComponent<Image>().color = c;
			numbers.GetComponent<Image>().color = c;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		// count down
		for (int i = 3; i >= 0; --i) {
			yield return new WaitForSeconds(1);
			numbers.GetComponent<Image> ().sprite = num_image [i];
		}
		// fadeout
		time = 0.98f;
		for (float rest_time = time; rest_time > 0; rest_time -= Constant.TIMESTEPJUMP) {
			float ratio = rest_time / time;
			Color c = image.GetComponent<Image>().color;
			c.a = ratio;
			image.GetComponent<Image>().color = c;
			numbers.GetComponent<Image>().color = c;
			yield return new WaitForSeconds(Constant.TIMESTEPJUMP);
		}
		// restart
		GameObject.Find("MainController").SendMessage("Restart");
	}
}
