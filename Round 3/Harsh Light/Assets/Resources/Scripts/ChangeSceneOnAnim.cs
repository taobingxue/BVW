using UnityEngine;
using System.Collections;

public class ChangeSceneOnAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void JumpToScene (){
		Application.LoadLevel (3);
	}
}
