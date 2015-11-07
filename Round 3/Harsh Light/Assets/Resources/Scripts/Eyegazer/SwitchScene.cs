using UnityEngine;
using System.Collections;

public class SwitchScene : MonoBehaviour
{
	private Rect pos = new Rect(100,100,500,500);

	void Start()
	{
	
	}
	
	void Update () 
	{
		if (Input.GetKey(KeyCode.Space))
		{
			Application.LoadLevel(1);
		}
	}

	void OnGUI()
	{
		GUI.Label(pos, "After calibration is complete, press Space to begin the world...");
	}
}
