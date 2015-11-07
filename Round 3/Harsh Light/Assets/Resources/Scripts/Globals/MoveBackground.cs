using UnityEngine;
using System.Collections;

public class MoveBackground : MonoBehaviour {
	public float width = 17.59f;
	public float speed;
	GameObject piece0, piece1;
	// Use this for initialization
	void Start () {
		piece1 = Instantiate (this.gameObject);
		piece1.transform.position += width * Vector3.right;
		MoveBackground comp = piece1.GetComponent<MoveBackground> ();
		Destroy (comp);
		piece0 = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		piece0.transform.position += Vector3.left * Time.deltaTime * speed;
		piece1.transform.position += Vector3.left * Time.deltaTime * speed;
		if (piece0.transform.position.x <= -width) {
			piece0.transform.position += width * 2 * Vector3.right;
			GameObject obj = piece0;
			piece0 = piece1;
			piece1 = obj;
		}
	}
}
