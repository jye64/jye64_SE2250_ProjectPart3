using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	static public Main S;
	static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

	[Header("Set in Inspector")]
	public GameObject[] prefabEnemies;
	public float enemySpawnPerSecond = 0.5f;
	public float enemyDefaultPadding = 1.5f;
	public WeaponDefinition[] weaponDefinitions;

	[Header("Set in Inspector: Text")]
	public Text scoreText;
	private int _scoreCounter = 0;

	public Text highScoreText;
	static private int _highScoreCounter = 0;
	 
	private BoundsCheck bndCheck;

	void Awake(){
		S = this;
		bndCheck = GetComponent<BoundsCheck> ();
		Invoke ("SpawnEnemy", 1f / enemySpawnPerSecond);

		WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition> ();
		foreach (WeaponDefinition def in weaponDefinitions) {
			WEAP_DICT [def.type] = def;
		}
	}

	public void SpawnEnemy(){
		int ndx = Random.Range (0, prefabEnemies.Length);
		GameObject go = Instantiate<GameObject> (prefabEnemies [ndx]);

		float enemyPadding = enemyDefaultPadding;
		if (go.GetComponent<BoundsCheck> () != null) {
			enemyPadding = Mathf.Abs (go.GetComponent<BoundsCheck> ().radius);
		}

		Vector3 pos = Vector3.zero;
		float xMin = -bndCheck.camWidth + enemyPadding;
		float xMax = bndCheck.camWidth + enemyPadding;
		pos.x = Random.Range (xMin, xMax);
		pos.y = bndCheck.camHeight + enemyPadding;
		go.transform.position = pos;

		Invoke ("SpawnEnemy", 1f / enemySpawnPerSecond);

	}

	public void DelayedRestart(float delay){
		Invoke ("Restart", delay);
	}

	public void Restart(){
		SceneManager.LoadScene ("_Scene_0");
	}

	/// <summary>
	/// Static funciotn that gets a WeaponDefinition from the WEAP_DICT static
	/// protected field of the Main class.
	/// </summary>
	/// <returns>The WeaponDefinition or , if there is no WeaponDefinition with
	/// the WeaponType passed in, returns a new WeaponDefinition with a 
	/// WeaponType of none..</returns>
	/// <param name="wt">The WeaponType of the desired WeaponDefinition </param>


	static public WeaponDefinition GetWeaponDefinition (WeaponType wt){
		if (WEAP_DICT.ContainsKey (wt)) {
			return (WEAP_DICT [wt]);
		}
		return (new WeaponDefinition());
	}

	// Use this for initialization
	void Start () {
		setScoreText (0);
		setHighScoreText ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void setScoreText(int input){
		_scoreCounter += input;
		scoreText.text = "Score: " + _scoreCounter.ToString ();
	}

	public void setHighScoreText(){
		if (_scoreCounter > _highScoreCounter) {
			_highScoreCounter = _scoreCounter;
		}
		highScoreText.text = "High Score: " + "\n" +_highScoreCounter.ToString ();
	}


}
