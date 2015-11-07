using UnityEngine;
using System.Collections;

public class Pillar : MonoBehaviour {

	public AudioSource sfxPillarOpen;
	public AudioSource sfxPillarClose;
	public float Pillarmovement = 2f;
	public float fadint = 0.3f;
	public bool once = false;
	public float closet, opent;
	GameObject pillarup, pillardown, light;
	Vector3 uppos, downpos;
	Color color;

	// Use this for initialization
	void Start () {
		// find son objs
		foreach (Transform trans in this.transform) {
			if (trans.gameObject.name == "pillar_up")
				pillarup = trans.gameObject;
			else if (trans.gameObject.name == "pillar_down")
				pillardown = trans.gameObject;
			else if (trans.gameObject.name == "pillar_light")
				light = trans.gameObject;
		}	
		// set transform
		uppos = pillarup.transform.localPosition;
		downpos = pillardown.transform.localPosition;
		pillarup.transform.localPosition += Vector3.up * Pillarmovement;
		pillardown.transform.localPosition += Vector3.down * Pillarmovement;
		// light
		color = light.GetComponent<SpriteRenderer> ().color;
		if (once) {
			color.a = 0;
			light.GetComponent<SpriteRenderer> ().color = color;
		}
		StartCoroutine("showpillar");
	}
	
	// Update is called once per frame
	void Update () {
		if (once) return ;
		this.transform.position += Vector3.left * Constant.bgspeed * Time.deltaTime;
		if (!Constant.spacelimit.Contains (new Vector2 (this.transform.position.x, this.transform.position.y)))
			Destroy (this.gameObject);	
	}

	IEnumerator showpillar() {
		// fadein
		if (once) {
			for (float rest_time = fadint; rest_time > 0; rest_time -= Constant.timestep) {
				color.a = 1 - rest_time / fadint;
				light.GetComponent<SpriteRenderer> ().color = color;
				yield return new WaitForSeconds(Constant.timestep);
			}
		}
		while (true) {
			// close
			for (float rest_time = closet; rest_time > 0; rest_time -= Constant.timestep) {
				float ratio = rest_time / closet;
				Debug.Log ("ratio = " + ratio);
				pillarup.transform.localPosition = uppos + Vector3.up * ratio * Pillarmovement;
				Debug.Log("x = " + pillarup.transform.localPosition.x + ", y = "+pillarup.transform.localPosition.y);
				pillardown.transform.localPosition = downpos + Vector3.down * ratio * Pillarmovement;

				if(ratio > 0.5f){
					// Play close sound
					PlayPillarCloseSfx();
				}

				yield return new WaitForSeconds(Constant.timestep);
			}
			// wait
			yield return new WaitForSeconds(0.1f);
			// open
			for (float rest_time = opent; rest_time > 0; rest_time -= Constant.timestep) {
				float ratio = 1 - rest_time / opent;
				pillarup.transform.localPosition = uppos + Vector3.up * ratio * Pillarmovement;
				pillardown.transform.localPosition = downpos + Vector3.down * ratio * Pillarmovement;
				yield return new WaitForSeconds(Constant.timestep);
				PlayPillarOpenSfx();
			}
			// fadein
			if (once) {
				for (float rest_time = fadint; rest_time > 0; rest_time -= Constant.timestep) {
					color.a = rest_time / fadint;
					light.GetComponent<SpriteRenderer> ().color = color;
					yield return new WaitForSeconds(Constant.timestep);
				}
				Destroy(this.gameObject);
			}
		}
	}

	void PlayPillarCloseSfx(){
		if(!sfxPillarClose.isPlaying){
			sfxPillarOpen.Stop();
			sfxPillarClose.Play();
		}
	}

	void PlayPillarOpenSfx(){
		if(!sfxPillarOpen.isPlaying){
			sfxPillarOpen.Play();
		}
	}
}
