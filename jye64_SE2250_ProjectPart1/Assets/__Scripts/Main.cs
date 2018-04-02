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
	public GameObject prefabPowerUp;

	// Here is the array of available PowerUps
	public WeaponType[] powerUpFrequency = new WeaponType[]{
		WeaponType.simple, WeaponType.blaster, WeaponType.blaster, WeaponType.shield
	};
		
	[Header("Set in Inspector: Text")]
	public Text scoreText;
	private int _scoreCounter = 0;

	public Text levelText;
	private int _levelCounter = 1;

	public Text highScoreText;
    public Text BombCountText;

    private int _BombCount = 0;

	public Text nextLevelText;
	private int currentLevel = 1;

	private BoundsCheck bndCheck;
	private float spawnTimer;
	private bool toSpawn;

	public void ShipDestroyed(Enemy e){
		if (Random.value <= e.powerUpDropChance){      // manage PowerUp drop chance, set in inspector
			//choose which powerup to drop
			int ndx = Random.Range(0, powerUpFrequency.Length);
			WeaponType puType = powerUpFrequency [ndx];
			GameObject go = Instantiate (prefabPowerUp) as GameObject;
			PowerUp pu = go.GetComponent<PowerUp> ();
			pu.SetType (puType);
			pu.transform.position = e.transform.position;
		}
	}
		

	void Awake(){
		S = this;
		//set bndCheck to reference the BoundsCheck component on this GameObject
		bndCheck = GetComponent<BoundsCheck> ();
		//Invoke ("SpawnEnemy", 1f / enemySpawnPerSecond);

		// A generic Dictionary with WeaponType as the key
		WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition> ();
		foreach (WeaponDefinition def in weaponDefinitions) {
			WEAP_DICT [def.type] = def;
		}
		toSpawn = true;
	}

	public void SpawnEnemy(){
		int ndx;
		if(currentLevel==1){
			ndx = Random.Range (0, prefabEnemies.Length-1);
		}else{
			ndx = prefabEnemies.Length-1;
		}
			
		GameObject go = Instantiate<GameObject> (prefabEnemies [ndx]);
		if (ndx == 4)
			toSpawn = false;     //spawn only one Boss

		//position the Enemy above the screen with a random x position
		float enemyPadding = enemyDefaultPadding;
		if (go.GetComponent<BoundsCheck> () != null) {
			enemyPadding = Mathf.Abs (go.GetComponent<BoundsCheck> ().radius);
		}

		//set initial position for the spawned Enemy
		Vector3 pos = Vector3.zero;
		float xMin = -bndCheck.camWidth + enemyPadding;
		float xMax = bndCheck.camWidth + enemyPadding;
		pos.x = Random.Range (xMin, xMax);
		pos.y = bndCheck.camHeight + enemyPadding;
		go.transform.position = pos;

		//Invoke ("SpawnEnemy", 1f / enemySpawnPerSecond);

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
		setLevelText ();
        setBombCountText(0);
	}
	
	// Update is called once per frame
	void Update () {
		setHighScoreText ();
		if((spawnTimer >= 1f / enemySpawnPerSecond) && toSpawn ){
			SpawnEnemy ();
			spawnTimer = 0; 
		}
		spawnTimer += Time.deltaTime;
	}

	public void setScoreText(int input){
		_scoreCounter += input;
		scoreText.text = "Score: " + _scoreCounter.ToString ();
	}

	public void setHighScoreText(){
		if (_scoreCounter > PlayerPrefs.GetInt("_highScoreCounter")) {
			PlayerPrefs.SetInt("_highScoreCounter", _scoreCounter);
		}
		highScoreText.text = "High Score: " + "\n" + PlayerPrefs.GetInt("_highScoreCounter");
	}
    public void setBombCountText(int input)
    {
        _BombCount += input;
        BombCountText.text = "Bomb: " + _BombCount.ToString();
    }

    public void setLevelText(){
		if (_scoreCounter > 1200){
			_levelCounter = 2;
			levelText.text = "Level: " + _levelCounter.ToString();
		}else{
			levelText.text = "Level : 1";
		}
	}

	//used when switching level, clear remaining enemies on screen and stop spawning
	void clearEnemy(){
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach(GameObject go in enemies){
			Destroy (go);
		}
		toSpawn = false;
	}

	public void setNextLevelText(){
		if(_scoreCounter>1200){
			currentLevel = 2;
			nextLevelText.text = "Next Level";
			clearEnemy ();
			Invoke ("resetSpawn", 2f);
		}
	}
		
	void resetSpawn(){
		toSpawn = true;
		clearNextLevelText ();
	}

	void clearNextLevelText(){
		nextLevelText.text = "";
	}


}
