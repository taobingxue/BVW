using UnityEngine;
using System.Collections;

public class fullhaiku : MonoBehaviour {
	Sprite[] haiku;
	// Use this for initialization
	void Start () {
		haiku = new Sprite[185];
		haiku = Resources.LoadAll<Sprite> ("fullhaiku");
	}

	public void startanim() {
		Debug.Log ("anim");
		StartCoroutine ("anim");
	}

	IEnumerator anim() {
		for (int i=0; i < haiku.Length; i++) {
			Debug.Log (i);
			this.GetComponent<SpriteRenderer>().sprite = haiku[i];
			yield return new WaitForSeconds(0.04f);
		}
	}
}
