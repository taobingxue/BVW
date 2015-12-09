using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonRename : MonoBehaviour {

	public float		x_min, x_max, y_min, y_max;
	MovementViewer		_viewer;

	string[] firstName = {"Meowski", "Justin", "Whacky", "Lady", "General", "Sam", "Doctor", "Captain", "Majestic", "Jiawen", "Justin", "Carrie", "JuiceTin", "Eric", "Dasol", "Velvet", "lemonjello", "Richard", "Jaworski", "Lighten", "Ronald", "Neit", "Stubby", "Norman", "Dudley", "Bassington", "Melvin", "Quentin", "Hives", "Ramzey", "Molly", "Bluett", "Ludikalo", "Doodle", "Cinnamon", "Emerald", "Billy", "Andreas", "Gimli", "Buzz", "Bong", "Ping", "Chaaang", "Harry", "Draco", "Bella", "Ben", "San'", "Bonly", "Robert", "Barbara", "Pearl", "Annette", "Shada", "Rusty", "Lorry", "Ima", "Daisy", "Seymour", "Nosmo", "Sansa", "Xzayvian", "Sirjames", "Adeline", "Levaeh", "Tokyo", "Pilot", "Fifi", "Apple", "Destry", "Ocean", "Audio", "Moon", "Moxie", "Tu", "Briar", "Wyatt", "Bear", "Egypt", "Buddy", "Lolitta" };
	string[] lastName = { "Piggs", "Nutters", "Jelly", "Demon", "Clutterbuck", "Greedy", "Hardmeat", "Hogwood", "Hiscock", "Steer", "Bracegirdle", "Bonefat", "Turtle", "Cornfoot", "Rattlebag", "Bottom", "Pigfat", "Willy", "Swindells", "Baum", "Zapel", "Bino", "Fresco", "Seltzer", "De'Ath", "Conda", "Sasin", "Fartley", "Graff", "Boring", "Doohan", "Jass", "Liam", "Furst", "Loewe", "Music", "Rupp", "Deere", "Schauer", "Zenz", "Waite", "Crash", "Forrest", "Flay", "Pohl", "Boatman", "Leeves", "Camner", "Cupp", "Fillerup", "Ouyang", "Yang", "Park", "Liang", "vonFischer", "Inspektor", "Trixibelle", "Science", "Government", "Kutcher", "Winslet", "Silverstein", "Coppola", "Ky", "Wentz", "Mustard", "Sirius", "Boo", "Jewell", "Yoga", "Demon", "Phoenix", "Bubbles", "BigMac", "Vixen", "Snowball", "Pocky", "Makey" };

	AudioSource _audioS;
	Rect		_startSpace;
	float		_standtime;

	void Awake() {
		_audioS = this.GetComponent<AudioSource> ();
		_startSpace = new Rect (x_min, y_min, x_max - x_min, y_max - y_min);
	}
	// Use this for initialization
	void Start () {
		_viewer = GameObject.Find ("viewer").GetComponent<MovementViewer> ();
		Constant.nameFist = "Harsh Light";
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 cursorPosition = _viewer.getPosition();
		if (_startSpace.Contains(cursorPosition)) {
			float newtime = _standtime + Time.deltaTime;
			for (int i = 1; i < 3; i ++) {
				if (_standtime < i && newtime >= i) {
					_audioS.Play();
					if (i == 2) push();
				}
			}
			_standtime = newtime;
		} else _standtime = 0;	
	}
	void push() {
		Constant.nameFist = firstName [(int)Random.Range (0, firstName.Length - 0.01f)] + " " + lastName [(int)Random.Range (0, lastName.Length - 0.01f)];
		GameObject.Find ("hide").GetComponent<InputField> ().Select ();
		GameObject.Find ("InputField").GetComponent<InputField> ().text = Constant.nameFist;
	
		Debug.Log (Constant.nameFist);
		Debug.Log (GameObject.Find ("name").GetComponent<Text> ().text);
		_standtime = -0.01f;
	}
}
