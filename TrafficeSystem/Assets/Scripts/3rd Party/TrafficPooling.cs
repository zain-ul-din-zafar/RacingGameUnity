using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RandomsUtilities;

public class TrafficPooling : MonoBehaviour {

	#region SINGLETON PATTERN
	public static TrafficPooling instance;
	public static TrafficPooling Instance{
		get{
			if(instance == null)
				instance = GameObject.FindObjectOfType<TrafficPooling>();
			return instance;
		}
	}
	#endregion
    
	public TrafficType trafficType;
	private Transform reference;

	public Transform[] lines;
	
	public bool animateNow = true;
	
	public TrafficCars[] trafficCars;
	public List <PowerUps> powerUps;
    
	[System.Serializable]
	public class TrafficCars{
		public GameObject trafficCar;
		public int frequence = 1;
	}
	
	[System.Serializable]
	public class PowerUps {
		public GameObject powerUp;
		public int frequence = 1;
	}

	private List<TrafficCar> _trafficCars = new List<TrafficCar>();
    private List<PowerUps> _powerUps = new List<PowerUps> ();
	
    private const float SPAWN_DELAY = 10f;
	private float spawnDelay ;
    
	private Stack <int> randomLinesHistory = new Stack<int> ();
	public float yPos;

	void Start () {
		spawnDelay = SPAWN_DELAY;
		reference = Camera.main.transform;
		yPos = lines [0].position.y;
		CreateTraffic();
	}

	void Update(){ 
		if(animateNow) { AnimateTraffic(); DestoryAllCarsBehindPlayer(); }  
	}

	void CreateTraffic () {
		
		for (int i = 0; i < trafficCars.Length; i++) {

			for (int k = 0; k < trafficCars[i].frequence; k++) {
//				GameObject go = (GameObject)GameObject.Instantiate(trafficCars[i].trafficCar, trafficCars[i].trafficCar.transform.position, trafficCars[i].trafficCar.transform.rotation);
				GameObject go = Instantiate(trafficCars[i].trafficCar, Vector3.zero, Quaternion.identity);
				_trafficCars.Add(go.GetComponent<TrafficCar>());
				go.SetActive(false);
			}

		}
		
		// creates Power Ups
		for (int i = 0 ; i < powerUps.Count ; i += 1) {
			for (int j = 0 ; j < powerUps[i].frequence ; j += 1) {
				var powerUp = Instantiate (powerUps[i].powerUp , Vector3.zero , Quaternion.identity) as GameObject;
				powerUp.SetActive (false);
				_powerUps.Add (new PowerUps{powerUp = powerUp , frequence = powerUps[i].frequence });
			}
		}

		
	}
    
	

	void AnimateTraffic () {
		
		for (int i = 0; i < _trafficCars.Count; i++) {
			if(reference.transform.position.z > (_trafficCars[i].transform.position.z + 15) || reference.transform.position.z < (_trafficCars[i].transform.position.z - (325)))
				ReAlignTraffic(_trafficCars[i]);
		}
		
		for (int i = 0 ; i < _powerUps.Count ; i += 1) {
			if (
				reference.transform.position.z > (_powerUps[i].powerUp.transform.position.z + 14) 
			    ||
				reference.transform.position.z < (_powerUps[i].powerUp.transform.position.z - (325))
			) {
				spawnDelay -= Time.deltaTime;
				if (spawnDelay < 0) {
				 SpawnPowerUps (_powerUps[i].powerUp ); 
                 spawnDelay = SPAWN_DELAY;
				}
			}
		}
	}

    void SpawnPowerUps (GameObject powerUp) {
	   powerUp.SetActive (true);
       // don't spawn cars on the same line
	   int randomLine = Random.Range (0 , lines.Length);

	   if (randomLinesHistory.Count > 0) {
		while (randomLinesHistory.Peek () == randomLine) {
			randomLine = Random.Range (0 , lines.Length);
		}
		randomLinesHistory.Pop();
	   }

	   randomLinesHistory.Push (randomLine); // adds new random line to history
       powerUp.transform.position =  new Vector3(lines[randomLine].position.x, lines[randomLine].position.y + 1.5f, reference.transform.position.z + Random.Range(100, 300));
	}
	
	void ReAlignTraffic(TrafficCar realignableObject){

		if(!realignableObject.gameObject.activeSelf)
			realignableObject.gameObject.SetActive(true);
        
		int randomLine = Random.Range(0, lines.Length );

		realignableObject.currentLine = randomLine;
		realignableObject.transform.position = new Vector3(lines[randomLine].position.x, yPos, (reference.transform.position.z + (Random.Range(100, 300))));
        
		realignableObject.transform.rotation = Quaternion.identity;
		
		switch(trafficType){
		
		case(TrafficType.OneWay):
				realignableObject.transform.rotation = Quaternion.identity;
				break;
		case(TrafficType.TwoWay):
			if(realignableObject.transform.position.x <= 0f)
				realignableObject.transform.rotation = Quaternion.identity * Quaternion.Euler(0f, 180f, 0f);
			else
				realignableObject.transform.rotation = Quaternion.identity;
			break;
		case(TrafficType.TimeAttack):
			realignableObject.transform.rotation = Quaternion.identity;
			break;
		case(TrafficType.Bomb):
			realignableObject.transform.rotation = Quaternion.identity;
			break;
		}

		realignableObject.SendMessage("OnReAligned");

		if(CheckIfClipping(realignableObject.triggerCollider))
			realignableObject.gameObject.SetActive(false);
	}

	bool CheckIfClipping(BoxCollider trafficCarBound){
         
		for (int i = 0; i < _trafficCars.Count; i++) {

			if(!trafficCarBound.transform.IsChildOf(_trafficCars[i].transform) && _trafficCars[i].gameObject.activeSelf){
				
				if(ContainBounds(trafficCarBound.transform, trafficCarBound.bounds, _trafficCars[i].triggerCollider.bounds))
					return true;

			}
			
		}

		return false;

	}

	
	public enum TrafficType {
		OneWay,
		TwoWay,
		TimeAttack,
		Bomb
	}
	
	/* Helper Functions */
	private static bool ContainBounds(Transform t, Bounds bounds, Bounds target){

		if(bounds.Contains(target.ClosestPoint(t.position))){
			return true;
		}

		return false;

	}
    
    private void SpawnCleaner () {
     foreach (var powerUp in  _powerUps) 
	  if(PlayerController.Instance.gameObject.transform.position.z > powerUp.powerUp.transform.position.z + 20f) {
		powerUp.powerUp.SetActive(false);
	  }
    }
    
	private void DestoryAllCarsBehindPlayer () {
		
		// use special powers
		// Utilities.QuickSort <TrafficCar> (0 , _trafficCars.Count - 1 , ref _trafficCars , (TrafficCar lhs , TrafficCar rhs)=>{
		// 	return lhs.gameObject.transform.position.z < rhs.gameObject.transform.position.z;
		// });
        
       for (int i = 0; i < _trafficCars.Count; i++) {
			if(PlayerController.Instance.gameObject.transform.position.z > _trafficCars[i].transform.position.z + 15f)
				_trafficCars[i].gameObject.SetActive(false);
		}
	}
    
    
}
