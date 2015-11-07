using UnityEngine;
using System.Collections;

public class MovementViewer : MonoBehaviour {

	public bool 		isEyeGaze = false;

	Transform 	limitedVisionTransform;
	Vector3 	_eyeGazePosition;
	EyeTracker 	_tracker;

	// Use this for initialization
	void Awake()
	{
		GameObject limitedVisionObject = Instantiate(Resources.Load("Prefab/SightView"), Vector3.zero, Quaternion.identity) as GameObject;
		limitedVisionTransform = limitedVisionObject.transform;
		_tracker = GameObject.Find("EyeTracker").GetComponent<EyeTracker>();
		if (_tracker == null){
			Debug.LogError("Error: Could not find EyeTracker prefab or its EyeTracker component!");
		}
	}	

	// fadein
	void Start() {
		StartCoroutine("fadein");
	}

	IEnumerator fadein() {
		Debug.Log("in");
		Color c = Color.black; 
		GameObject blackbg = GameObject.Find("black");
		for (float rest_time = Constant.fadeouttime; rest_time > 0; rest_time -= Constant.timestep) {
			c.a = rest_time / Constant.fadeouttime;
			blackbg.GetComponent<SpriteRenderer>().color = c;
			yield return new WaitForSeconds(Constant.timestep);
		}
	}

	// Update is called once per frame
	void Update () {
		UpdateEyeGazePosition();
		UpdateFairyPosition();
	}

	void UpdateEyeGazePosition(){
		if (_tracker && _tracker.FoundGaze)
		{
			Vector3 newPos = transform.localPosition;
			newPos.x = (float) _tracker.GazePointX;
			newPos.y = (float) (Screen.height - _tracker.GazePointY);
			newPos.z = 5;
			_eyeGazePosition = Camera.main.ScreenToWorldPoint(newPos);
		}
	}

	void UpdateFairyPosition(){
		Vector3 cursorPosition;

		if(isEyeGaze){
			cursorPosition = _eyeGazePosition;
		}else{
			cursorPosition = Input.mousePosition;
			cursorPosition.z = 5;
			cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
		}

		cursorPosition.x = CheckBoundary (cursorPosition.x, Movement_Constant.FAIRY_X_BOUNDARY.x, Movement_Constant.FAIRY_X_BOUNDARY.y);
		cursorPosition.y = CheckBoundary (cursorPosition.y, Movement_Constant.FAIRY_Y_BOUNDARY.x, Movement_Constant.FAIRY_Y_BOUNDARY.y);

		limitedVisionTransform.position = cursorPosition;
	}

	public Vector2 getPosition() {
		Vector3 cursorPosition;
		
		if(isEyeGaze){
			cursorPosition = _eyeGazePosition;
		} else{
			cursorPosition = Input.mousePosition;
			cursorPosition.z = 5;
			cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
		}

		return new Vector2 (cursorPosition.x, cursorPosition.y);
	}

	float CheckBoundary(float position, float min, float max){
		if(position < min){
			return min;
		}else if(position > max){
			return max;
		}
		return position;
	}
}
