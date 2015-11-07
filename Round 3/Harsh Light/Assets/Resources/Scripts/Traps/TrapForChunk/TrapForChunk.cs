using UnityEngine;
using System.Collections;

public class TrapForChunk : MonoBehaviour {

	public enum TrapForChunkType
	{
		None = 0,
		Pillar,
		Hand,
		SwingBall,
		Bat
	}

	public Vector3 trapPosition;
	public Vector3 trapRotation;
	
	[SerializeField]
	TrapForChunkType type;

	protected GameObject _trap;

	// Use this for initialization
	void Start () {
		CreateTrapPrefab();
		CreateTrap ();
	}
	
	void CreateTrapPrefab(){
		switch(type){
		case TrapForChunkType.Pillar:
			_trap = Resources.Load("Prefab/pillar") as GameObject;
			break;
		case TrapForChunkType.Hand:
			_trap = Resources.Load("Prefab/hand") as GameObject;
			break;
		case TrapForChunkType.SwingBall:
			_trap = Resources.Load("Prefab/swingingball") as GameObject;
			break;
		case TrapForChunkType.Bat:
			_trap = Resources.Load("Prefab/bat_circle") as GameObject;
			break;
		default:
			break;
		}
	}

	public virtual void CreateTrap(){
		if(_trap != null){
			Quaternion rotation = Quaternion.Euler(trapRotation);
			GameObject trapObject = Instantiate(_trap) as GameObject;
			trapObject.transform.parent = this.transform.parent;
			trapObject.transform.localPosition =  trapPosition;
			trapObject.transform.rotation = rotation;
		}
	}
}
