using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public bool 		isEyeGaze = false;

	Transform 	limitedVisionTransform;
	Vector3 	_eyeGazePosition, lastPosition;
	EyeTracker 	_tracker;
	float 		_maxDistance;
	float 		standtime;

	// Use this for initialization
	void Awake()
	{
		GameObject limitedVisionObject = Instantiate(Resources.Load("Prefab/SightView"), Vector3.zero, Quaternion.identity) as GameObject;
		limitedVisionTransform = limitedVisionObject.transform;
		_tracker = GameObject.Find("EyeTracker").GetComponent<EyeTracker>();
		if (_tracker == null){
			Debug.LogError("Error: Could not find EyeTracker prefab or its EyeTracker component!");
		}

		_maxDistance = Movement_Constant.FAIRY_MIN_DISTANCE*Movement_Constant.FAIRY_MIN_DISTANCE;

		// size control
		lastPosition = new Vector3 (20, 20, -5);
		standtime = 0;
		///////////////
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

		// size control
		// count dis
		Vector3 vec = lastPosition - cursorPosition;
		vec.z = 0;
		float dis = vec.magnitude;
		// count standtime
		if (dis <= Constant.distencelimit) {
			standtime += Time.deltaTime;
			// decrease
			if (standtime >= Constant.standtimelimit && limitedVisionTransform.localScale.x > Constant.sizelowerbound)
				limitedVisionTransform.localScale *= Constant.decreaseratio;
		} else {
			standtime = 0;
			limitedVisionTransform.localScale = Vector3.one;
		}
		lastPosition = cursorPosition;
		///////////////

		cursorPosition.x = CheckBoundary (cursorPosition.x, Movement_Constant.FAIRY_X_BOUNDARY.x, Movement_Constant.FAIRY_X_BOUNDARY.y);
		cursorPosition.y = CheckBoundary (cursorPosition.y, Movement_Constant.FAIRY_Y_BOUNDARY.x, Movement_Constant.FAIRY_Y_BOUNDARY.y);

		float xDistance = cursorPosition.x - transform.position.x;
		float yDistance = cursorPosition.y - transform.position.y;
		float sqrDistance = xDistance * xDistance + yDistance * yDistance;
		Vector3 currentPosition = transform.position;
		
		if (sqrDistance > _maxDistance) {
			float newXPosition = currentPosition.x + xDistance * Movement_Constant.FAIRY_EASING_NUMBER;
			float newYPosition = currentPosition.y + yDistance * Movement_Constant.FAIRY_EASING_NUMBER;
			currentPosition.x = CheckBoundary (newXPosition, Movement_Constant.FAIRY_X_BOUNDARY.x, Movement_Constant.FAIRY_X_BOUNDARY.y);
			currentPosition.y = CheckBoundary (newYPosition, Movement_Constant.FAIRY_Y_BOUNDARY.x, Movement_Constant.FAIRY_Y_BOUNDARY.y);
		}

		// score
		Constant.score += (int) (sqrDistance / 5);
		////////////////////
		
		transform.position = currentPosition;
		limitedVisionTransform.position = cursorPosition;
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
