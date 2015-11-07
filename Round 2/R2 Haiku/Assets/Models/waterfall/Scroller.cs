using UnityEngine;

public class Scroller : MonoBehaviour
{
	//these variables will be visible in the Inspector
	public float xScrollSpeed;
	public float yScrollSpeed;
	
	//these variables will hold the offset calculations
	private float xOffset;
	private float yOffset;
	
	// Update is called once per frame
	void Update()
	{
		//this section calculates the speed and applies it the 
		//offset of the material every frame
		xOffset = Time.time * xScrollSpeed;
		yOffset = Time.time * yScrollSpeed;
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(xOffset, yOffset);
	}
}