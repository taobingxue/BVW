using UnityEngine;
using System.Collections;

public class SwingBallTrapForChunk : TrapForChunk {
	
	public float angleLimit;
	public float speedLimit;

	public override void CreateTrap(){
		if(_trap != null){
			Quaternion rotation = Quaternion.Euler(trapRotation);
			GameObject trapObject = Instantiate(_trap) as GameObject;
			trapObject.transform.parent = this.transform.parent;
			trapObject.transform.localPosition =  trapPosition;
			trapObject.transform.rotation = rotation;

			if(trapObject != null){
				trapObject.GetComponent<Ball>().init(angleLimit, speedLimit);
			}
		}
	}
}
