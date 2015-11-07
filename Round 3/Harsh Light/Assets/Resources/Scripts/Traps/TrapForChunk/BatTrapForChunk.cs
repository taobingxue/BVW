using UnityEngine;
using System.Collections;

public class BatTrapForChunk : TrapForChunk {
	
	public int numOfBats;
	public float minSpeed, maxSpeed, size, speed;

	//public int numofbat;
	//public float speed_min, speed_max, size, speed;

	public override void CreateTrap(){
		if(_trap != null){
			Quaternion rotation = Quaternion.Euler(trapRotation);
			GameObject trapObject = Instantiate(_trap) as GameObject;
			trapObject.transform.parent = this.transform.parent;
			trapObject.transform.localPosition =  trapPosition;
			trapObject.transform.rotation = rotation;

			if(trapObject != null){
				trapObject.GetComponent<Bat_Circle>().init (numOfBats, minSpeed, maxSpeed, size, speed);
			}
		}
	}
}
