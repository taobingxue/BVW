using UnityEngine;
using System.Collections;

public class Constant  : MonoBehaviour {
	// maze size
	public static int mazeWidth = 640;
	public static int mazeHeight = 480;
	// total levels, levelrunning, levelrecord
	public static int levelMax = 8, levelnow = 0, levelstart = 0;
	public static int xMin = -300, xMax = 300, yMin = -220, yMax = 220;
	// light height
	public static int lightHeight = 480; 
	// that is the wall
	public static int flag;

	// Use this for initialization
	void Start () {	}
	// Update is called once per frame
	void Update () { }
}
