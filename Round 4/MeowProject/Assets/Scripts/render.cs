using UnityEngine;
using System.Collections;

public class render : MonoBehaviour {
	public Material inkmaterial; 
	public Material inkwaterboiled;
    int lengthOfLineRenderer = 116;
	public float inkwidth=1f;
	public bool boiled;
	GameObject[] go;

	// Initialize the linerenderer

	void Start () {
		LineRenderer lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(lengthOfLineRenderer);
		lineRenderer.SetWidth(inkwidth,inkwidth);
		lineRenderer.material=inkmaterial;
		lineRenderer.sortingOrder = 4;
		/*
		go=GameObject.FindGameObjectsWithTag("bubble");
		foreach (GameObject bubbleelemet in go)
			bubbleelemet.SetActive (false);*/

	}


	void Update () {
		if (boiled) {
			BoilingWater ();
		} else
			normalwater ();
	}

	//normal water waves
	void normalwater () {
		int i = 0;
		LineRenderer lineRenderer = GetComponent<LineRenderer> ();
		while (i < lengthOfLineRenderer) {
			lineRenderer.material=inkmaterial;
			Vector3 pos = new Vector3(0.5f*i,Mathf.Sin(0.25f*i + Time.time)/3+Mathf.Cos(Time.time)/7, 0);
			lineRenderer.SetPosition(i, pos);
			i++;
		}
		//if do not use the prefab,keep this on.
		//bubble activate and deactivate when boiled and not.
		/*
		foreach (GameObject bubbleelemet in go)
			bubbleelemet.SetActive (false);*/
	        }


	void BoilingWater()
	{
		LineRenderer lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.material = inkwaterboiled;
		int i = 0;
		while (i < lengthOfLineRenderer) {
			Vector3 pos = new Vector3 (0.5f * i, Mathf.Sin (0.5f*i + Time.time) / 3 + Mathf.Cos (Time.time) / 2, 0);
			lineRenderer.SetPosition (i, pos);
			i++;
		}
		/*
		foreach (GameObject bubbleelemet in go)
			bubbleelemet.SetActive (true);
			*/
	}



}
