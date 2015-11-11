using UnityEngine;
using System.Collections;

public class PaintTouchCollider : MonoBehaviour {

	//=================
	public AudioClip grabsound;
	public AudioClip putitback;

	private GameObject painting = null;
	private bool isTouch = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (MotionController.getInstance ().getAction() == Actions.IDLE) {
			isTouch = false;
		} else {
			isTouch = true;
		}
	}

	void FixedUpdate() {
		CheckHitObject ();
	}
	
	public GameObject getPaintingSelected(){
		return painting;
	}

	void CheckHitObject(){
		Vector3 temp = Camera.main.WorldToScreenPoint (this.transform.position);
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(temp);
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.gameObject.tag == "movePaint") {
				painting = hit.collider.gameObject;
				if(isTouch){
					{
						//========================================
						if (painting.GetComponent<FrameMover>().mFrameState == FrameState.IDLE)
							SoundManager.instance.PlayMove(grabsound);
					painting.GetComponent<FrameMover>().mFrameState = FrameState.MOVE;
					Debug.Log (hit.collider.gameObject.name);
					}
				}else
				{
					//=====================================
					if (painting.GetComponent<FrameMover>().mFrameState == FrameState.MOVE)
						SoundManager.instance.PlayMove(putitback);
					painting.GetComponent<FrameMover>().mFrameState = FrameState.IDLE;
				}
			} else {
				if(painting !=  null) {
					painting.GetComponent<FrameMover>().mFrameState = FrameState.IDLE;
					painting = null;
				}
			}
		} else {
			if(painting !=  null) {
				painting.GetComponent<FrameMover>().mFrameState = FrameState.IDLE;
				painting = null;
			}
		}
	}
}
