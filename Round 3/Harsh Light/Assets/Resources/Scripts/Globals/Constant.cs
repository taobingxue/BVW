using UnityEngine;
using System.Collections;

public class Constant {

	// Scenes
	public static int SCENE_CALIBRATION 	= 0;
	public static int SCENE_TITLE 			= 1;
	public static int SCENE_GAME 			= 2;
	public static int SCENE_NAME 			= 3;
	public static int SCENE_RESTART 		= 4;
	public static int SCENE_SCRIPT			= 5;
	public static int SCENE_VICTORY			= 6;

	// background speed
	public static float bgspeed = 1.8f;
	// coroutine step
	public static float timestep = 0.02f;
	// limitation for all traps
	public static Rect spacelimit = new Rect (-25f, -25f, 50, 50);
	// parameter for sightview
	public static float distencelimit = 0.1f;
	public static float decreaseratio = 0.995f;
	public static float standtimelimit = 0.2f;
	public static float sizelowerbound = 0.45f;

	public static int counttime = 3;
	public static float fadeouttime = 1;

	// rank && score
	public static int ranklength = 8;
	public static int score = 0;
	public static int rank = -1;
	public static string nameFist = "";
	public static string nameSecond = "延轩";

	public static bool winorlose = true;
}
