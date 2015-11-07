using UnityEngine;
using System.Collections;

public class leveldefine : MonoBehaviour {
	// level now
	public static int levelnow = 0;
	// infor for trigger
	public Vector3 triggerpos, triggergoa;
	public float triggerdis, triggerangle;

	// trigger
	public void settriggergoa(Vector3 goa) {
		triggergoa = goa;
	}
	public void settriggerpos(Vector3 pos) {
		triggerpos = pos;
	}
	public void settriggerpdis(float dis) {
		triggerdis = dis;
	}
	public void settriggerpangle(float angle) {
		triggerangle = angle;
	}
}
