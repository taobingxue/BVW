using UnityEngine;
using System.Collections;

public class HaikuAppear : MonoBehaviour {


	Sprite[] haiku = new Sprite[1];

	// Use this for initialization
	void Start () {
		switch (gameObject.tag) {
		case "tree":
			haiku = new Sprite[105];
			haiku = Resources.LoadAll<Sprite> ("treehaiku");
			break;
		case "hawk":
			haiku = new Sprite[58];
			haiku = Resources.LoadAll<Sprite> ("hawkhaiku");
			break;
		case "pavilion":
			haiku = new Sprite[119];
			haiku = Resources.LoadAll<Sprite> ("springhaiku");
			break;
		}
		StartCoroutine("anim");
	}
	
	void OnEnable() {
		StartCoroutine("anim");
	}
	
	IEnumerator anim() {
		while (true) {
			for (int i = 0; i < haiku.Length; i++) {
				this.GetComponent<SpriteRenderer> ().sprite = haiku [i];
				yield return new WaitForSeconds (0.05f);
			}
		}
	}
}