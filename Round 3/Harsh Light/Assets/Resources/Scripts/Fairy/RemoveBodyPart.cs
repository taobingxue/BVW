using UnityEngine;
using System.Collections;

public class RemoveBodyPart : MonoBehaviour {
	public ParticleSystem Blood;
	private float slice_amount = 0.1f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Fade (){
		SpriteRenderer graphic = this.gameObject.GetComponent<SpriteRenderer> ();
		graphic.material.color *= new Color (1, 1, 1, 1-slice_amount);
		slice_amount += slice_amount;
		if (graphic.material.color.a > 0) {
			Invoke ("Fade", 0.2f);
		} else {
			graphic.material.color = new Color (0, 0, 0 ,0) ;
		}
	}
	public void LosePart (){
		Blood.Play ();
		Fade ();
	}
}
