using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

// EyeGaze Wrapper
// DLL Interface class
// Ken Hilf - khilf@andrew.cmu.edu
// Written for Fall BVW 2012
public static class EyeWrapper
{
	/// <summary>
	/// Runs the external calibration program then starts up the Eyegaze system.
	/// </summary>
	/// <param name='screenWidth'>
	/// Screen width.
	/// </param>
	/// <param name='screenHeight'>
	/// Screen height.
	/// </param>
	[DllImport ("EyeDLL")]
	public static extern void InitEyeTracking(int screenWidth, int screenHeight);

	/// <summary>
	// Shuts down the Eyegaze system.
	/// </summary>
	[DllImport ("EyeDLL")]
	public static extern void ShutDownEyeTracking();
	
	/// <summary>
	/// Turns the tracking system on or off after it has been initialized.
	/// </summary>
	/// <param name='state'>
	/// True to enable, false to disable.
	/// </param>
	[DllImport ("EyeDLL")]
	public static extern void SetTrackingState(bool state);
	
	/// <summary>
	/// Requests the latest data from the eye tracking system, must be called each frame before any data is
	/// requested from the Eyegaze unit.
	/// </summary>
	/// <returns>
	/// True if the eye can be seen, otherwise false.
	/// </returns>
	[DllImport ("EyeDLL")]
	public static extern bool UpdateEyeTrackingData();

	/// <summary>
	/// Gets the last known good X coordinate of the gaze point
	/// </summary>
	/// <returns>
	/// The X screen coordinate of the last known good gaze position.
	/// </returns>
	[DllImport ("EyeDLL")]
	public static extern int GetEyeX();

	/// <summary>
	/// Gets the last known good Y coordinate of the gaze point
	/// </summary>
	/// <returns>
	/// The Y screen coordinate of the last known good gaze position.
	/// </returns>
	[DllImport ("EyeDLL")]
	public static extern int GetEyeY();
	
	/// <summary>
	/// Gets the last known good size in mm of the pupil
	/// @TODO: This currently always returns 0.0f, not sure why.
	/// </summary>
	/// <returns>
	/// The pupil size in mm.
	/// </returns>
	[DllImport ("EyeDLL")]
	public static extern float GetPupilSize();
}
