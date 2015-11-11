using UnityEngine;
using System.Collections;

public class LiquidBanana : MonoBehaviour {

	public AudioClip waterdrop;


	public GameObject splash;//particle system
	public Material mat; //line renderer
	public GameObject watermesh; //water mesh
	public Transform boatPos;

	//llllllllllllllllllllllllllluna
	bool soundflag=true;

	Vector3 newWaterPos;

	//location, speed and acceleration of nodes
	float[] xpositions;
	float[] ypositions;
	float[] velocities;
	float[] accelerations;
	LineRenderer Body;

	//water
	GameObject[] meshobjects;
	Mesh[] meshes;

	//the collider of water
	GameObject[] colliders;

	//all constant
	const float springconstant = 0.05f;
	const float damping = 0.07f;
	const float spread = 0.05f;

	public float width;
	public float height;

	
	//scale of water
	float baseheight;
	float left;
	float bottom;
	float oldHeight;
	int edgecount;
	float z;
	float originalHeight;

	bool nextflag = false;
	bool isHeightUpdated = false;
	// Use this for initialization
	void Start () {
		z = transform.position.z;
		SpawnWater (0, width, height, 0);
		newWaterPos = this.transform.position;
		originalHeight = this.transform.position.y;
		soundflag = true;
	}
	
	// Update is called once per frame
	void Update () {



		IncreaseWater ();


	}

	void IncreaseWater() {
		this.transform.position = Vector3.Lerp (this.transform.position,newWaterPos,Time.deltaTime);
	}

	public void SetHeightUpdated(bool _val){
		isHeightUpdated = _val;
	}

	public void reset(){
		Vector3 temp = this.transform.position;
		temp.y = originalHeight;
		this.transform.position = temp;
		newWaterPos = temp;
		nextflag = false;
	}

	public void SpawnWaterUpdate(float Left, float Width, float Top, float Bottom)
	{
		//find the number of nodes we need
		edgecount = Mathf.RoundToInt(Width) * 5;
		int nodecount = edgecount + 1;
		
		//use line renderer to render water
		Body = gameObject.GetComponent<LineRenderer>();

		//create nodes, set default value

		baseheight = Top;
		bottom = Bottom;
		left = Left;
		
		//set value for nodes
		for (int i = 0; i < nodecount; i++)
		{
			ypositions[i] = Top;
			xpositions[i] = Left + Width * i / edgecount;
			accelerations[i] = 0;
			velocities[i] = 0;
			Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
		}
		
		//create water mesh
		for (int i = 0; i < edgecount; i++)
		{
			//meshes[i] = new Mesh();
			Vector3[] Vertices = new Vector3[4];
			Vertices[0] = new Vector3(xpositions[i], ypositions[i], 0);
			Vertices[1] = new Vector3(xpositions[i + 1], ypositions[i + 1], 0);
			Vertices[2] = new Vector3(xpositions[i], bottom, 0);
			Vertices[3] = new Vector3(xpositions[i+1], bottom, 0);
			
			//use uv to set texture
			Vector2[] UVs = new Vector2[4];
			UVs[0] = new Vector2(0, 1);
			UVs[1] = new Vector2(1, 1);
			UVs[2] = new Vector2(0, 0);
			UVs[3] = new Vector2(1, 0);
			
			//creat the first area of water
			//int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };
			//set value
			meshes[i].vertices = Vertices;
			meshes[i].uv = UVs;
			//meshes[i].triangles = tris;
			
			//render gameobject
			//meshobjects[i] = Instantiate(watermesh,Vector3.zero,Quaternion.identity) as GameObject;
			meshobjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
			meshobjects[i].transform.parent = transform;
			meshobjects[i].transform.localPosition = Vector3.zero;
			
			//set collider
			colliders[i].transform.parent = transform;
			colliders[i].transform.localPosition = new Vector3(Left + Width * (i + 0.5f) / edgecount, Top - 0.5f, 0);
			colliders[i].transform.localScale = new Vector3(Width / edgecount, 1, 1);
		}
	}
	public void SpawnWater(float Left, float Width, float Top, float Bottom)
	{
		//find the number of nodes we need
		edgecount = Mathf.RoundToInt(Width) * 5;
		int nodecount = edgecount + 1;

		//use line renderer to render water
		Body = gameObject.AddComponent<LineRenderer>();
		Body.material = mat;
		Body.material.renderQueue = 1000;
		Body.SetVertexCount(nodecount);
		Body.SetWidth(0.1f, 0.1f);

		//create nodes, set default value
		xpositions = new float[nodecount];
		ypositions = new float[nodecount];
		velocities = new float[nodecount];
		accelerations = new float[nodecount];
		
		meshobjects = new GameObject[edgecount];
		meshes = new Mesh[edgecount];
		colliders = new GameObject[edgecount];
		
		baseheight = Top;
		bottom = Bottom;
		left = Left;

		//set value for nodes
		for (int i = 0; i < nodecount; i++)
		{
			ypositions[i] = Top;
			xpositions[i] = Left + Width * i / edgecount;
			accelerations[i] = 0;
			velocities[i] = 0;
			Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
		}

		//create water mesh
		for (int i = 0; i < edgecount; i++)
		{
			meshes[i] = new Mesh();
			Vector3[] Vertices = new Vector3[4];
			Vertices[0] = new Vector3(xpositions[i], ypositions[i], 0);
			Vertices[1] = new Vector3(xpositions[i + 1], ypositions[i + 1], 0);
			Vertices[2] = new Vector3(xpositions[i], bottom, 0);
			Vertices[3] = new Vector3(xpositions[i+1], bottom, 0);

			//use uv to set texture
			Vector2[] UVs = new Vector2[4];
			UVs[0] = new Vector2(0, 1);
			UVs[1] = new Vector2(1, 1);
			UVs[2] = new Vector2(0, 0);
			UVs[3] = new Vector2(1, 0);

			//creat the first area of water
			int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };
			//set value
			meshes[i].vertices = Vertices;
			meshes[i].uv = UVs;
			meshes[i].triangles = tris;

			//render gameobject
			meshobjects[i] = Instantiate(watermesh,Vector3.zero,Quaternion.identity) as GameObject;
			meshobjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
			meshobjects[i].transform.parent = transform;
			meshobjects[i].transform.localPosition = Vector3.zero;

			//set collider
			colliders[i] = new GameObject();
			colliders[i].name = "Trigger";
			colliders[i].AddComponent<BoxCollider2D>();
			colliders[i].transform.parent = transform;
			colliders[i].transform.localPosition = new Vector3(Left + Width * (i + 0.5f) / edgecount, Top - 0.5f, 0);
			colliders[i].transform.localScale = new Vector3(Width / edgecount, 1, 1);
			colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
			colliders[i].AddComponent<LiquidDetector>();
		}
	}

	void UpdateMeshes() //update when water move
	{
	  for (int i = 0; i < meshes.Length; i++)
		{
			Vector3[] Vertices = new Vector3[4];
			Vertices[0] = new Vector3(xpositions[i], ypositions[i], 0);
			Vertices[1] = new Vector3(xpositions[i+1], ypositions[i+1], 0);
			Vertices[2] = new Vector3(xpositions[i], bottom, 0);
			Vertices[3] = new Vector3(xpositions[i+1], bottom, 0);
			meshes[i].vertices = Vertices;
		}

	}

	void FixedUpdate()
	{	

		for (int i = 0; i < xpositions.Length ; i++)
		{
			float force = springconstant * (ypositions[i] - baseheight) + velocities[i]*damping;
			accelerations[i] = -force;
			ypositions[i] += velocities[i];
			velocities[i] += accelerations[i];
			Vector3 pos = transform.position + (new Vector3(xpositions[i], ypositions[i], 0.0f));
			Body.SetPosition(i, pos);
		}
		float[] leftDeltas = new float[xpositions.Length];
		float[] rightDeltas = new float[xpositions.Length];

		for (int j = 0; j < 8; j++)
		{
			for (int i = 0; i < xpositions.Length; i++)
			{
				if (i > 0)
				{
					leftDeltas[i] = spread * (ypositions[i] - ypositions[i-1]);
					velocities[i - 1] += leftDeltas[i];
				}
				if (i < xpositions.Length - 1)
				{
					rightDeltas[i] = spread * (ypositions[i] - ypositions[i + 1]);
					velocities[i + 1] += rightDeltas[i];
				}
			}
		}

		for (int i = 0; i < xpositions.Length; i++)
		{
			if (i > 0) 
			{
				ypositions[i-1] += leftDeltas[i];
			}
			if (i < xpositions.Length - 1) 
			{
				ypositions[i + 1] += rightDeltas[i];
			}
		}

		UpdateMeshes ();

	}
	public void Splash(float xpos, float velocity)
	{
		//Debug.Log ("spawn");
		if (!isHeightUpdated) {
			if (this.transform.position.y > boatPos.position.y && (!nextflag)) {
				GameObject.Find("MainController").GetComponent<MainController>().Next();
				nextflag = true;
				soundflag=false;
			} else if (this.transform.position.y < boatPos.position.y)  {
				newWaterPos = this.transform.position;
				newWaterPos.y += 1f;
				//===============================================
				if(soundflag)
				SoundManager.instance.PlayDialoge(waterdrop);
			}

			//SpawnWaterUpdate (0, width, height, 0);
			isHeightUpdated = true;
		}
		xpos -= transform.position.x;
		if (xpos >= xpositions [0] && xpos <= xpositions [xpositions.Length - 1]) {
			xpos -= xpositions [0];
			int index = Mathf.RoundToInt ((xpositions.Length - 1) * (xpos / (xpositions [xpositions.Length - 1] - xpositions [0])));
			velocities [index] = velocity;
			float lifetime = 0.93f + Mathf.Abs (velocity) * 0.07f;
			splash.GetComponent<ParticleSystem> ().startSpeed = 8 + 2 * Mathf.Pow (Mathf.Abs (velocity), 0.5f);
			splash.GetComponent<ParticleSystem> ().startSpeed = 9 + 2 * Mathf.Pow (Mathf.Abs (velocity), 0.5f);
			splash.GetComponent<ParticleSystem> ().startLifetime = lifetime;
			Vector3 position = new Vector3 (xpositions [index], ypositions [index] - 0.35f, 0);
			Quaternion rotation = Quaternion.LookRotation (new Vector3 (xpositions [Mathf.FloorToInt (xpositions.Length / 2)], baseheight + 8, 5) - position);
			GameObject splish = Instantiate(splash,position += transform.position,rotation) as GameObject;
			Destroy(splish, lifetime+0.3f);
		}
	}

}



