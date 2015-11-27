using UnityEngine;
using System.Collections;

public class EndLight : MonoBehaviour {
	// trigger once
	bool flag = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += Vector3.left * Constant.bgspeed * Time.deltaTime;
	}

	void ExecuteTrigger (){
		if (flag) return ;
		//GoTo ENDSCENE
		GameObject.Find("Fairy").GetComponent<FairyStatus>().countscore();
		GetComponent<WhiteFadeOut>().StartFadeOut();
		flag = true;
	}
}
