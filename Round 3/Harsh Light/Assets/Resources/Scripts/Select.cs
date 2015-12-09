using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Select : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<InputField> ().Select ();
		Debug.Log("Select");
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<InputField> ().Select ();
		string name = GameObject.Find ("InputField").GetComponent<InputField> ().text;
		if (name != Constant.nameFist)
			Constant.nameFist = name;
	}
}
