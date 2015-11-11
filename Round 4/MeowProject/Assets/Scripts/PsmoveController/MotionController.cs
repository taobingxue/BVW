using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Actions {
	IDLE,
	MOVER,
	MOVEL,
	ROTATE
}
public class MotionController : MonoBehaviour {
	
	public GameObject right,left,frame,rightHand,leftHand,leftAnchor,rightAnchor;
	public GameObject gemRight,gemLeft;
	public float angleOffset = 1,rotSpeed = 200,handSpeed = 2, shakeOffset = 1;
	public Text debugText;
	private Actions mAction;
	public float shakeTimeOffset = 1f;
	
	private float lastAngle = 0;
	private bool isRight = false;
	private Vector2 handPos,lastPosR,lastPosL;
	public Vector2 range,prevAngle;
	
	
	private static MotionController instance = null;
	
	
	private bool shakeRight,isShake;
	private Vector3 lastFramePos;
	private float lastTime = -1;
	private int shakeCount = 0;

	void calculateShake() {
		if (frame == null) {
			lastTime = Time.time;
			isShake = false;
			return;
		}
		if (frame.transform.position.x + shakeOffset < lastFramePos.x) {
			if (shakeRight) {
				shakeRight = false;
				shakeCount++;
				//Debug.Log ("shake");
				lastTime = Time.time;
				
			}
		} else if (frame.transform.position.x - shakeOffset > lastFramePos.x) {
			if (!shakeRight) {
				shakeCount++;
				shakeRight = true;
				//Debug.Log ("shake right");
				lastTime = Time.time;
				
			}
		} else {
			if(Time.time - lastTime > shakeTimeOffset) {
				isShake = false;
				shakeCount = 0;
				lastTime = Time.time;
			}
		}
		//		Debug.Log (shakeCount);
		if(shakeCount > 4) {
			Debug.Log ("shake");
			isShake = true;
			//lastTime = Time.time;
		}
		lastFramePos = frame.transform.position;
		//lastTime += Time.deltaTime;
	}
	
	public static MotionController getInstance(){
		return instance;
	}
	
	public void setAction(Actions _val) {
		if (!Constant.moveEnabled) {
			mAction = Actions.IDLE;
			return;
		}
		mAction = _val;
	}
	
	public Actions getAction() {
		return mAction;
	}
	
	public bool getShake(){
		return isShake;
	}
		
	
	// Use this for initialization
	void Start () {
		instance = this;
		mAction = Actions.IDLE;
		lastPosR = Vector2.zero;
		lastPosL = Vector2.zero;
		prevAngle = Vector2.zero;
		lastFramePos = frame.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		RotateFrame ();
		MoveFrame ();
		//MoveFrame (true);
		//Debug.Log (mAction);
	}
	
	public void setHandPosition(Vector2 pos) {
		handPos = pos;
	}
	
	void FixedUpdate() {
		//MoveHand (gemRight.transform.position);
		//MoveHand (gemRight.transform.position, lastPosR, rightHand,true);
		//MoveHand (gemLeft.transform.position, lastPosL, leftHand,false);
		MoveHand (true);
		MoveHand (false);
		calculateShake ();
	}
	
	void MoveFrame() {
		if (mAction != Actions.MOVEL && mAction != Actions.MOVER)
			return;
		
		Vector3 position = Vector3.zero;
		if (mAction == Actions.MOVEL) {
			frame = leftHand.GetComponent<PaintTouchCollider> ().getPaintingSelected ();
			position = Camera.main.WorldToScreenPoint(leftHand.transform.position);
		} else if (mAction == Actions.MOVER) {
			frame = rightHand.GetComponent<PaintTouchCollider> ().getPaintingSelected ();
			position = Camera.main.WorldToScreenPoint(rightHand.transform.position);
		}
		if (frame == null)
			return;
		
		position.z = Mathf.Abs(frame.transform.position.z-Camera.main.transform.position.z);
		position = Camera.main.ScreenToWorldPoint (position);
		frame.transform.position = position;
		
	}
	
	void MoveHand(bool isRight) {
		if (isRight) {
			Vector2 offset = rightAnchor.GetComponent<AngleCalculator>().getOffset();
			Vector3 temp;
			temp.x = offset.x * range.x;
			temp.y = offset.y * range.y;
			temp.z = rightHand.transform.localPosition.z;
			rightHand.transform.localPosition = temp;
		} else {
			Vector2 offset = leftAnchor.GetComponent<AngleCalculator>().getOffset();
			Vector3 temp;
			temp.x = offset.x * range.x;
			temp.y = offset.y * range.y;
			temp.z = leftHand.transform.localPosition.z;
			leftHand.transform.localPosition = temp;
		}
	}
	
	void RotateFrame() {
		if (frame == null)
			return;
		
		Vector3 direction = rightHand.transform.position - leftHand.transform.position;
		float angle = Vector3.Angle (direction, Vector3.left);
		float sign = Mathf.Sign (Vector3.Dot (direction, Vector3.up));
		angle = sign * angle;
		if (angle < 0)
			angle = 360 + angle;
		
		if (Mathf.Abs (angle - lastAngle) > angleOffset) {
			isRight = (Mathf.Sign(angle-lastAngle) > 0) ? true : false;
			//debugText.text = (isRight) ? "right" : "left";
			Vector3 tempVec = (!isRight) ? Vector3.forward : Vector3.back;
			if(mAction == Actions.ROTATE) {
				frame.transform.Rotate(tempVec * Time.deltaTime * rotSpeed);
			}
			lastAngle = angle;
		}
	}
	
	void MoveHand(Vector2 pos,bool isRight = true) {
		if (Vector2.Distance (pos, lastPosR) > 0) {
			Vector3 temp = rightHand.transform.position;
			Vector2 position = ((pos - lastPosR) * handSpeed);
			temp.x += position.x;
			temp.y += position.y;
			rightHand.transform.position = temp;
			lastPosR = pos;
		}
		//Debug.Log(handPos);
		
	}
	
	void MoveHand(Vector2 newPos,Vector2 lastPos,GameObject hand, bool isRight) {
		if (Vector2.Distance (newPos, lastPos) > 0) {
			Vector3 temp = hand.transform.position;
			Vector2 position = ((newPos - lastPos) * handSpeed);
			temp.x += position.x;
			temp.y += position.y;
			hand.transform.position = temp;
			temp = hand.transform.localPosition;
			if(Mathf.Abs(temp.x)>range.x){
				temp.x = Mathf.Sign(temp.x) * range.x;
			}
			if(Mathf.Abs(temp.y)>range.y){
				temp.y = Mathf.Sign(temp.y) * range.y;
			}
			hand.transform.localPosition = temp;
			
			if(isRight)
				lastPosR = newPos;
			else
				lastPosL = newPos;
		}
	}
	
	
}

