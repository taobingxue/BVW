using UnityEngine;
using System.Collections;

public class DropGenerator : MonoBehaviour {

	public GameObject waterDrop;
	public Vector2 range,genTimeRange;
	bool isActive = true;
	// Use this for initialization
	void Start () {
		StartCoroutine (dropGenerator ());
		// Debug.Log (waterDrop.transform.localScale);
		waterDrop.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.L)) {
			isActive = false;
		}


	}

	IEnumerator dropGenerator() {
		while (isActive) {
			if(MotionController.getInstance() == null)
				yield return null;

			if(MotionController.getInstance().getShake())
				yield return new WaitForSeconds(Random.Range(genTimeRange.x * 0.5f,genTimeRange.y * 0.5f));
			else
				yield return new WaitForSeconds(Random.Range(genTimeRange.x,genTimeRange.y));
			Vector3 temp = this.transform.position;
			temp.x = temp.x+Random.Range (range.x, range.y);
			Instantiate (waterDrop, temp, Quaternion.identity);		
		}
	}
}
