using UnityEngine;
using System.Collections;
using System;

public class mazeControler : MonoBehaviour {
	// disappear speed and raising speed
	const float disSpeed = 0.6f;
	const float raiSpeed = 300f;
	// which level it is now
	int levelNumber;
	// prefabs for wall and plane
	public GameObject wallPrefab, planePrefab;
	
	// plane and walls
	GameObject plane, plane2;
	GameObject[] walls, walls2;
	// if it is winning
	bool winFlag;
	// colors;
	Color[] colors;
	// data loaded
	string[] coordinates;
	// player
	GameObject player;
	// init location for player
	int playerX, playerY;
	// speak object
	GameObject speak;
	// random
	System.Random rand;
	
	// Use this for initialization
	void Start () {
		levelNumber = Constant.levelnow;
		winFlag = false;
		player = GameObject.FindGameObjectWithTag("player");
		speak = GameObject.Find("Speak");
		// generate
		rand = new System.Random();
		// wall colors
		colors = new Color[4];
		colors[0] = new Color(255, 255, 0, 0);
		colors[1] = new Color(255, 0, 0, 0);
		colors[2] = new Color(0, 0, 255, 0);
		colors[3] = new Color(255, 0, 230, 0);

		if (levelNumber < Constant.levelMax) createLevel(levelNumber, 0);
		else generateLevel(0);

		StartCoroutine( playerSpeak());
	}
	
	// Update is called once per frame
	void Update () {
		fading();
		// cheated
		if (Input.GetKeyUp (KeyCode.Return)) player.transform.position = new Vector3 (-500, 0, -500);
		// begin raise
		if (winFlag == false && check()) {
			playerWin();
		} else if (winFlag) {
			// finish raise
			if (plane2 != null && plane2.transform.position.y > 500) {
				restart();
			} else {
			// raise everything
				float delta = raiSpeed * Time.deltaTime;
				raise(ref player, delta);
				raise(ref plane, delta); raise(ref plane2, delta);

				for (int w = 0; w < 4; w++) {
					GameObject[] objs = GameObject.FindGameObjectsWithTag("" + w);
					for (int i = 0; i < objs.Length; i++) raise(ref objs[i], delta);
				}
			}
		}
		// make ball fall faster
		if (winFlag == false && player.transform.position.y > 20) player.GetComponent<Rigidbody>().AddForce(Vector3.down * 400f, ForceMode.VelocityChange);
	}
	
	// create walls and plane
	void createLevel(int levelnumber, int height) {
		// load data
		TextAsset text = Resources.Load("" + levelnumber) as TextAsset;
		//Debug.Log(text.text);
		coordinates= text.text.Split(';');
		
		// Get coordinates for player
		string s = coordinates[0].Substring(0, coordinates[0].Length - 1);
		string[] numbs = s.Split(' ');
		playerX = Convert.ToInt32(numbs[0]); playerY = Convert.ToInt32(numbs[1]);
		if (!check()) player.transform.position = new Vector3(playerX + Constant.xMin + 10, Constant.lightHeight, playerY + Constant.yMin + 10);
		// Create walls
		walls = new GameObject[4];
		for (int i=0; i < 4; i++) StartCoroutine( creatWalls (i, height));
		// Create plane
		plane = Instantiate (planePrefab);
		plane.transform.position = new Vector3(planePrefab.transform.position.x, height, planePrefab.transform.position.z);
	}
	
	// create walls
	IEnumerator creatWalls (int label, int height){
		// initialize the father object
		walls[label] = new GameObject("wall " + label);
		walls[label].tag = label + "" + label;
		
		//get all coordinates
		string s = coordinates[label + 1].Substring(0, coordinates[label + 1].Length - 1);
		string[] coors = s.Split(',');
		
		foreach (string coor in coors) {
			if (coor.Length == 0) break;
			string[] numbs = coor.Substring(1, coor.Length-1).Split(' ');
			
			// new wall piece
			GameObject wallnow = Instantiate (wallPrefab);
			wallnow.tag = "" + label;
			
			// set position
			int x = Convert.ToInt32(numbs[0]); int y = Convert.ToInt32(numbs[1]);
			// Debug.Log(x+"+"+y);
			wallnow.transform.position = new Vector3(Constant.xMin + x, height, Constant.yMin + y);
			wallnow.transform.parent = walls[label].transform;
			wallnow.GetComponent<Renderer>().material.color = colors[label];
		}		
		yield return null;
	}
	
	// let walls fading if they exist
	void fading() {
		// Debug.Log("fading");
		if (walls == null || walls[3] == null) return ;
		Color c;
		for (int w = 0; w < 4; w++) {
			c = colors[w];
			foreach (Transform child in walls[w].transform) {
				float alpha = child.GetComponent<Renderer>().material.color.a;
				if (alpha > 0) {
					alpha -= disSpeed * Time.deltaTime;
					c.a = alpha;
					child.GetComponent<Renderer>().material.color = c;
				} else if (alpha < 0) {
					c.a = 0;
					child.GetComponent<Renderer>().material.color = c;
				} else break;
			}
		}
	}
	
	// player win, start raise
	void playerWin() {
		levelNumber ++;
		Constant.levelnow ++;
		Constant.levelstart ++;
		for (int i = 0; i < 4; i++) {
			GameObject[] objs = GameObject.FindGameObjectsWithTag("" + i);
			Color c = colors[i]; c.a = 1;
			for (int j = 0; j < objs.Length; j++)
				objs[j].GetComponent<Renderer>().material.color = c;
		}
		// goto next level
		if (levelNumber < Constant.levelMax - 1) {
			winFlag = true;
			plane2 = plane; walls2 = walls;
			createLevel(levelNumber, -500);
		} else if (levelNumber == Constant.levelMax - 1){
			Debug.Log("finish!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			for(int i = 0; i < 100000; i++);
			Constant.levelnow = 0;
			Constant.levelstart = 0;
			Application.LoadLevel (2);
		} else if (levelNumber == Constant.levelMax + 1) {
			winFlag = true;
			plane2 = plane; walls2 = walls;
			generateLevel(-500);
			levelNumber --;
			Constant.levelnow --;
			Constant.levelstart --;
		}
	}
	
	// restart: destroy old things
	void restart() {
		winFlag = false;
		player.transform.position = new Vector3(playerX + Constant.xMin + 10, Constant.lightHeight, playerY + Constant.yMin + 10);
		// remove walls for last level
		for (int i = 0; i < 4; i ++) GameObject.Destroy(walls2[i]);
		Destroy(plane2);
		StartCoroutine( playerSpeak());
	}
	
	// speak for a new level
	IEnumerator playerSpeak() {
		yield return new WaitForSeconds(1.3f);
		switch(levelNumber) {
			case 0: speak.SendMessage("speaking", "Where am I ?", SendMessageOptions.RequireReceiver);break;
			case 1: speak.SendMessage("speaking", "Again ???", SendMessageOptions.RequireReceiver);	break;
			case 2: speak.SendMessage("speaking", "Anyone else here ???", SendMessageOptions.RequireReceiver);break;
			case 3: speak.SendMessage("speaking", "Is this infinite ???", SendMessageOptions.RequireReceiver);break;
			case 4: speak.SendMessage("speaking", "Oh, I miss my parents ... and my girlfriend.", SendMessageOptions.RequireReceiver);break;
			case 5: speak.SendMessage("speaking", "Let me out !! Let me out !!", SendMessageOptions.RequireReceiver);break;
			case 6: speak.SendMessage("speaking", "En? Something is different here !", SendMessageOptions.RequireReceiver);break;
			default : break;
		}	
	}

	// if is winning
	bool check() {
		Vector3 pos = player.transform.position;
		if (pos.x < Constant.xMin - 30 || pos.x > Constant.xMax + 30 || pos.z < Constant.yMin - 30 || pos.z > Constant.yMax + 30) return true;
		return false;
	}
	
	// raise gameobject by the adjustment
	void raise(ref GameObject obj, float delt) {
		Vector3 pos = obj.transform.position;
		obj.transform.position = new Vector3(pos.x, pos.y + delt, pos.z);
	}

	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	// generate part
	int[, ] tw = new int[, ]{{0, 1}, {1, 0}, {0, -1}, {-1, 0}};

	int[, ] map;
	int[] line;
	int startx, starty;
	int hh = Constant.mazeHeight / 32, ww = Constant.mazeWidth / 32;
	
	void fillline(int st, int en) {
		for (int i = st; i <= en; i++) line[i] = 7;
		int ii = rand.Next(1, 3);
		for (int i = 0; i < ii; i++) {
			int p = rand.Next(st, en + 1);
			int l = rand.Next(1, 3);
			for (int j = 0; j <= l; j++) line[p + j] = 0;
		}
		
		int c = rand.Next(1, 5);
		for (int i = st; i <= en; i++)
			if (line[i] == 7) line[i] = c;
			else c = rand.Next(1, 5);
	}

	void work(int hi, int ha, int wi, int wa) {
		if (rand.Next(0, 2) == 0 && ha - hi >= 2 && wa >= wi) {
			fillline(wi, wa);
			int i = rand.Next(hi, ha + 1);
			for (int j = wi; j <= wa; j++) map[i, j] = line[j];

			work(hi, i-1, wi, wa);
			work(i+1, ha, wi, wa);
		} else if (wa - wi >= 2 && ha >= hi ) {
			fillline(hi, ha);
			int i = rand.Next(wi, wa + 1);
			for (int j = hi; j <= ha; j++) map[j, i] = line[j];

			work(hi, ha, wi, i-1);
			work(hi, ha, i+1, wa);		
		}
	}

	void selectstart() {
		int xx = rand.Next(0, 58);
		if (xx < 17) {startx = 0; starty = xx;}
		else if (xx < 34) {startx = hh-1; starty = xx - 17;}
		else if (xx < 46) {startx = xx - 34; starty = 0;}
		else {startx = xx - 46; starty = ww-1;}
	}

	bool checkmaze() {
		int[, ] m = new int[25, 25];
		int[] qx = new int[400], qy = new int[400];
		int st = 0, en = 0;
		for (int i = 0; i < hh; i++)
			for (int j = 0; j < ww; j++) m[i, j] = -1;
			
		qx[0] = startx; qy[0] = starty; m[startx, starty] = 0;
		for ( ; st <= en; st ++) {
			for (int i = 0; i < 4; i++) {
				int nx = qx[st] + tw[i, 0], ny = qy[st] + tw[i, 1];
				if (nx >= 0 && nx < hh && ny >= 0 && ny < ww && map[nx, ny] == 0 && m[nx, ny] == -1) {
					qx[++en] = nx;
					qy[en] = ny;
					m[nx, ny] = m[qx[st], qy[st]] + 1;
				}
			}
		}
		if (m[qx[en], qy[en]] < 20) return false;
		else {
			map[qx[en], qy[en]] = 8;
			map[startx, starty] = 0;
			return true;
		}
	}

	void generateLevel(int height) {
		Debug.Log("generate");
		map = new int[25, 25];
		line = new int[25];
		
		while (true) {
			// initial
			for (int i = 0; i < hh; i ++) {
				for (int j = 1; j < ww-1; j++)
					if (i == 0 || i == hh-1) map[i, j] = rand.Next(1, 5);
					else map[i, j] = 0;
				map[i, 0] = rand.Next(1, 5);
				map[i, ww-1] = rand.Next(1, 5);
			}
			// start and end
			work(1, hh-2, 1, ww-2);
			selectstart();
			if (checkmaze()) break;
		}
		
		// create plane
		plane = Instantiate (planePrefab);
		plane.transform.position = new Vector3(planePrefab.transform.position.x, height, planePrefab.transform.position.z);	
		// create walls
		walls = new GameObject[4];
		for (int i=0; i < 4; i++) {
			walls[i] = new GameObject("wall " + i);
			walls[i].tag = i + "" + i;
		}
		for (int i = 0; i < hh; i++)
			for (int j = 0; j < ww; j++)
				if (map[i, j] == 8) {
					playerX = j * 32;
					playerY = (14 - i) * 32;
					if (!check()) player.transform.position = new Vector3(playerX + Constant.xMin + 10, Constant.lightHeight, playerY + Constant.yMin + 10);
				} else if (map[i, j] > 0) {
					int label = map[i, j] - 1;
					// new wall piece
					GameObject wallnow = Instantiate (wallPrefab);
					wallnow.tag = "" + label;
					
					// set position
					int x = j * 32; int y = (14 - i) * 32;
					// Debug.Log(x+"+"+y);
					wallnow.transform.position = new Vector3(Constant.xMin + x, height, Constant.yMin + y);
					wallnow.transform.parent = walls[label].transform;
					wallnow.GetComponent<Renderer>().material.color = colors[label];
				}
		Debug.Log("generate finish");
	}
}








































