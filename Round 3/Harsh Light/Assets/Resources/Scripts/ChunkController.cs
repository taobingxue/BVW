using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkController : MonoBehaviour {
	public List<GameObject> chunk_list;
	[SerializeField]
	float offset = 17.8f;
	// Use this for initialization
	void Start () {
		StartCoroutine ("GenerateChunk");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator GenerateChunk (){
		for (int i = 0; i < chunk_list.Count; i++) {
			Instantiate (chunk_list[i], new Vector3(offset,  0, 0), Quaternion.identity);
			Debug.Log("Check Generated: "+chunk_list[i]);
			yield return new WaitForSeconds (6);
		}
	}
}
