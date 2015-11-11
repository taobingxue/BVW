using UnityEngine;
using System.Collections;

public class BananaMachine : MonoBehaviour {

	public LayerMask layer;
	public GameObject bananaPrefab;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(1) || Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 50, layer)) {
				GameObject.Instantiate(bananaPrefab, hit.point, Quaternion.identity);
			}
		}
	}
}
