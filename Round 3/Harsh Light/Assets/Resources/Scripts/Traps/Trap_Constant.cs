using UnityEngine;
using System.Collections;

public class Trap_Constant {
	// x range
	public static float x_min = -7;
	public static float x_max = 7;
	// y range
	public static float y_min = -10.5f;
	public static float y_max = 10.5f;
	public static float y_norm = 0;
	// angle
	public static float angle_limit = 50;

	// probabilities
	public static float prob_random = 0.02f;
	public static float prob_focus = 0;
	public static float prob_close = 0;

	// process
	public static float appear_time = 0.5f;
	public static float grow_time = 0.25f;
	public static float grow_startratio = 0.1f;
	public static float hand_time = 0.2f;
	public static float keep_time = 0.2f;
	public static float fade_time = 0.4f;
}
