using UnityEngine;
using System.Collections;

// EyeGaze Wrapper
// EyeTracker input class for Unity3D
// Ken Hilf - khilf@andrew.cmu.edu
// Written for Fall BVW 2012
//
// This object should be placed in the initial scene, it is flagged as persistent and will
// clean itself up at the end of the program.
public class EyeTracker : MonoBehaviour
{
	// These must be set by hand, the Unity editor returns the size of the Game tab's window on 
	// screen.width and screen.height, and the eyegaze unit wants full run of the screen.
	public int HorizontalResolution = 1920;
	public int VerticalResolution = 1080;
	
	// This controls how much data to include into the smoothing buffer.  The more data you 
	// include the smoother it can get, but will add some delay.  So far testing at 12 seems good.
	public int SmoothBufferSize = 12;
	private int SmoothBufferSizeMin = 2;
	private int SmoothBufferSizeMax = 20;
	
	// 0,0 is the top left corner of the primary monitor
	public int GazePointX = 0;
	public int GazePointY = 0;
	
	// Diameter is in milimeters
	//@NOTE: This seems to be broken, wrapper always returns 0.0f, need to 
	// contact pnorloff05@eyegaze.com to find out why...
	public float PupilDiameter = 0.0f;
	
	public bool FoundGaze = false;

	public bool UseEmulator = false;
	// This keeps the emulator from being toggled on or off in the editor during gameplay.
	private bool UsingEmulator = false;
	
	private int[] XBuffer;
	private int[] YBuffer;
	private int SmoothIndex = 0;
	
	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
		UsingEmulator = UseEmulator;
		
		if (!UsingEmulator)
		{
			EyeWrapper.InitEyeTracking(HorizontalResolution, VerticalResolution);
			EyeWrapper.SetTrackingState(true);
		}
		
		SmoothBufferSize = Mathf.Clamp(SmoothBufferSize, SmoothBufferSizeMin, SmoothBufferSizeMax);
		
		XBuffer = new int[SmoothBufferSize];
		YBuffer = new int[SmoothBufferSize];
		
		for (int i = 0; i < SmoothBufferSize; i++)
		{
			XBuffer[i] = 0;
			YBuffer[i] = 0;
		}
	}
	
	//@NOTE: at default project settings, this was running at a painful 4fps with the emulator DLL!
	// Changed the Time settings for the project to 0.016 for Fixed Timestep and Max Allowed Timestep 
	// and that seemed to help the FPS, and it didn't seem to affect the responsiveness of the data shown
	// to be coming out of the eye tracking wrapper, but this could end up being a big problem once we
	// have real hardware.  May have to totally rethink how we access the hardware, separate thread or server
	// and TCP/IP maybe.
	//Update 9/8/12 - Now that we have an actual kit to work with, it seems to be pulling 120+ fps no problem, but 
	// will try to keep an eye on performance.
	void FixedUpdate()
	{
		if (!UsingEmulator)
		{
			// Request the latest data from the eye tracking system
			FoundGaze = EyeWrapper.UpdateEyeTrackingData();
			
			// Update the public vars based on the latest data.
			// If the last data was no good (did not see the user, eyes were closed, etc) the
			// wrapper returns the last known good values for the gaze point.
			UpdateEyePos();
			PupilDiameter = EyeWrapper.GetPupilSize();
		}
		else // use mouse position to simulate gaze point
		{
			// Hold down F2 to simulate losing track of the last eye position.  The emulator will
			// continue to return the last known good value.
			if (Input.GetKey(KeyCode.F2))
			{
				FoundGaze = false;
				return;
			}
			
			FoundGaze = true;
			GazePointX = Mathf.Clamp((int)Input.mousePosition.x, 0, HorizontalResolution);
			GazePointY = Mathf.Clamp(VerticalResolution - (int)Input.mousePosition.y, 0, VerticalResolution);
		}
	}
	
	private void UpdateEyePos()
	{
		XBuffer[SmoothIndex] = Mathf.Clamp(EyeWrapper.GetEyeX(), 0, HorizontalResolution);
		YBuffer[SmoothIndex] = Mathf.Clamp(EyeWrapper.GetEyeY(), 0, VerticalResolution);
		SmoothIndex++;
		SmoothIndex = SmoothIndex % SmoothBufferSize;
		
		int finalX = 0;
		int finalY = 0;
		for (int i = 0; i < SmoothBufferSize; i++)
		{
			finalX += XBuffer[i];
			finalY += YBuffer[i];
		}
		
		GazePointX = finalX / SmoothBufferSize;
		GazePointY = finalY / SmoothBufferSize;
	}
	
	void OnApplicationQuit()
	{
		if (UsingEmulator)
			return;
		
		EyeWrapper.SetTrackingState(false);
		
		// If we are in the editor, shutting down prematurely will cause Unity to crash.
		// I'm afraid this means that it will also cause it to leak since we're not shutting
		// it down, but we'll have to investigate further.
		if (!Application.isEditor)
			EyeWrapper.ShutDownEyeTracking();
	}
}
