using UnityEngine;
using System.Collections;

public class Constant : MonoBehaviour {
	public static Vector3 floorsize = new Vector3 (4, 4, 4);
	public static Vector3 wallsize = new Vector3 (3.3f, 3.3f, 3.3f);
	
	public static Rect limitZoneFloor = new Rect (-200, 5, 375, 110);
	public static Rect limitZoneWall = new Rect (-200, 5, 375, 135);
	public static Rect limitZonebackWall = new Rect (-130, 57, 305, 35);
	public static Rect limitZoneKing = new Rect (-120, 10, 215, 95);
	public static int xMin = -199, xMax = 174;
	public static int yMin = 6, yMax = 139;
	public static int zMin = 6, zMax = 114;
	public static int floory = 2;
	public static int wallz = 5;
	
	public static int turnstep = 25;
	public static bool sizerandom = true;
	public static float normalspeed = 20;
	
	// check once for each window in this length
	public static int handcheckl = 3;
	public static double handspeed = 0.6;
	public static float foot_delta = 0.07f;

	//happiness
	public static float happyupbound = 100;
	public static float happyinit = 30;
	public static float hitdamage = 0.5f;
	public static float touchcooling = 6;
	public static float roachdamage = 2f;
	public static float killinc = 1f;
	public static Color roachonGF = new Color(120.0f / 255, 120.0f /255, 120.0f /255);

	// damage
	public static float roacheyesight = 2.0f;
	public static int steprange = 13;
	public static int pushradius = 13;
	public static float probTofall = 0.5f; //0.3f;
	
	// show limbs
	public static int showframesize = 1;
	public static int handz = 55;
	public static int footz = 80;
	public static int footy = 6;

	// roach king
	public static int kinglife = 10;
	public static Vector3 babyroachsize = Vector3.one * 1.2f;
	public static int birthsum = 10;

	// gf moving
	public static float gfspeed = 15;
	public static float gfxMin = -112;
	public static float gfxMax = 75;
	public static float[] timelength = new float[2]{4, 5};

	// photos
	public static Color32[][] photos;
	// ending time
	public static float endinglightingtime = 1.0f;
	// final score
	public static int finalscore = 0;
	// dot product
	public static float check(float x1, float y1, float x2, float y2) {
		return x1 * y2 - x2 * y1;
	}
	// trans X 	change -1~1 => 175~-200
	public static float transX(float x) { return (1.0f - x) / 2.0f * 375.0f - 200.0f;}
	// is a part of GR?
	public static bool isGF(GameObject obj) {
		if (obj.name == "Girlfriend") return true;
		if (obj.name == "Girlfriend_tex") return true;
		if (obj.name.Length >= 5 && obj.name.Substring(0, 5) == "joint") return true;
		return false;
	}
}
