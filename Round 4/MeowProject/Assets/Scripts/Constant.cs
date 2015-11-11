using UnityEngine;
using System.Collections;

public class Constant : MonoBehaviour {
	/*kinect
	 * 
	 * 
	// ratio for the body
	public static float LOWERRATIO = 0.2f;
	public static float HIGHERRATIO = 1.05f;
	// kinect x range
	public static float KINECTXMIN = -1;
	public static float KINECTXMAX = 1;
	// mouse postion z
	public static float MOUSEDEEPTH = 5;
	*/
	
	/*another version
	 * 
	 * 
	// hand postion z
	public static float HANDDEEPTH = 5;
	// screen position
	public static float SCREENXMIN = -10;
	public static float SCREENXMAX = 10;
	public static float SCREENYMIN = -5;
	public static float SCREENYMAX = 5;
	// kinect threshold
	public static int BUTTONTHRESHOLD = 250;
	// time step for jump animation
	public static float TIMESTEPJUMP = 0.2f;
	// timer
	public static float ZOOMOUTTIME = 1;
	public static float RUNNINGTIME = 3;
	*/
	
	public static float TIMESTEPJUMP = 0.02f;
	// all viewer
	public static Vector3 farShot = new Vector3 (-53, -36, -110);

	public static Vector3 farView1 = new Vector3 (1, -26, -45);
	public static Vector3 closeView1 = new Vector3 (7, -31.5f, -20);
	public static Vector3 farView2 = new Vector3 (-20, -50, -45);
	public static Vector3 closeView2 = new Vector3 (-24, -55.5f, -5);
	public static Vector3 farView3 = new Vector3 (-24, -77, -50);
	public static Vector3 closeView3 = new Vector3 (-34, -81, -32);
	public static Vector3 farView4 = new Vector3 (-70, -58, -70);
	public static Vector3 closeView4 = new Vector3 (-94.5f, -60, -46);
	public static Vector3 closeView50 = new Vector3 (-102, -48.5f, -22);
	public static Vector3 closeView51 = new Vector3 (-70, -24f, -60);
	public static Vector3 closeView5 = new Vector3 (-74, 10, -32);
	public static Vector3 closeView6 = new Vector3 (2.19f, -0.3f, 0);
	public static Vector3 closeView7 = new Vector3 (-33, 13, -18);
	public static Vector3 closeView8 = new Vector3 (2, 26, 1);
	public static Vector3 closeViewCat = new Vector3 (-87, -50, -20);
	// flag for if is mvoeable
	public static bool moveEnabled = true;
	// wait to die
	public static float timeToDie = 15;
	public static float catchDis = 4.5f;

	// shrimps
	public static float starWaitinTime = 0.5f;
	public static float starFallingTime = 0.8f;
	public static float shrimpMin = -91;
	public static float shrimpMax = -73f;
}
