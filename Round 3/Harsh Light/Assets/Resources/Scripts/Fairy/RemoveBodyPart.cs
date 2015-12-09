using UnityEngine;
using System.Collections;

public class RemoveBodyPart : MonoBehaviour {
	public ParticleSystem particleSystem;
	public SpriteRenderer spriteObject;

	private float slice_amount = 0.1f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Fade (){
		spriteObject.material.color *= new Color (1, 1, 1, 1-slice_amount);
		slice_amount += slice_amount;
		if (spriteObject.material.color.a > 0) {
			Invoke("Fade", 0.2f);
		} else {
			spriteObject.material.color = new Color (0, 0, 0 ,0) ;
		}
	}
	public void LosePart (){
		particleSystem.Play();
		Fade ();
	}
}
