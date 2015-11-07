using UnityEngine;
using System.Collections;

public class EyeTest : MonoBehaviour
{
	private EyeTracker tracker;
	
	void Awake()
	{
		tracker = GameObject.Find("EyeTracker").GetComponent<EyeTracker>();
		if (tracker == null)
			Debug.LogError("Error: Could not find EyeTracker prefab or its EyeTracker component!");
	}
	
	void Update()
	{
		if (tracker && tracker.FoundGaze)
		{
			Vector3 newPos = transform.localPosition;
			newPos.x = (float)tracker.GazePointX;
			newPos.y = (float)(Screen.height - tracker.GazePointY);
			newPos.z = 10;
			transform.localPosition = Camera.main.ScreenToWorldPoint(newPos);
		}
	}
}
