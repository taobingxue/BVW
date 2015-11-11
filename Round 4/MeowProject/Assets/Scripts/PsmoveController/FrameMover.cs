	using UnityEngine;
using System.Collections;

public enum FrameState {
	IDLE,
	MOVE
}
public class FrameMover : MonoBehaviour {


	// Use this for initialization
	public FrameState mFrameState;
	private Animator mAnim;
	public SpriteRenderer frame;

	void Start () {
		mFrameState  = FrameState.IDLE;
		mAnim = transform.parent.GetComponent<Animator> ();
		if(mAnim != null)
			mAnim.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		 if ((int)(this.transform.eulerAngles.z % 90) != 0 && mFrameState == FrameState.IDLE) {
			Quaternion newRot;
			int remainder = (int)this.transform.eulerAngles.z % 90;
			if (remainder > 45) {
				newRot = Quaternion.Euler (0, 0, this.transform.eulerAngles.z + remainder);
			} else {
				newRot = Quaternion.Euler (0, 0, this.transform.eulerAngles.z - remainder);
			}
			// this.transform.rotation = Quaternion.Lerp (this.transform.rotation, newRot, Time.deltaTime);
		}

		if (mFrameState == FrameState.MOVE) {
			GetComponent<SpriteRenderer> ().sortingOrder = 5;
			frame.sortingOrder = 6;
			if(mAnim != null)
				mAnim.enabled = true;
		} else {
			//GetComponent<SpriteRenderer> ().sortingOrder = 1;
			//frame.sortingOrder = 2;
			if(mAnim != null)
				mAnim.enabled = false;
		}
	}
}
