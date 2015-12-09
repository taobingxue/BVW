using UnityEngine;
using System.Collections;

public class ClearRankList : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int i = 2; i <= 8; ++i) {
            PlayerPrefs.SetString("name" + i, "Harsh Light");
            PlayerPrefs.SetInt("score" + i, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
