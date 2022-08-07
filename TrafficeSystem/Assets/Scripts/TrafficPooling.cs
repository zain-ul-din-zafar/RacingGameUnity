
using UnityEngine;
using System.Collections.Generic;

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

	[System.Serializable]
	public class TrafficCars{
		public GameObject trafficCar;
		public int frequence = 1;
	}
	
	private List<TrafficCar> _trafficCars = new List<TrafficCar>();

	void Start () {

		reference = Camera.main.transform;
		CreateTraffic();

	}

	void Update(){ if(animateNow) AnimateTraffic();  }

	void CreateTraffic () {
		
		for (int i = 0; i < trafficCars.Length; i++) {

			for (int k = 0; k < trafficCars[i].frequence; k++) {
				
//				GameObject go = (GameObject)GameObject.Instantiate(trafficCars[i].trafficCar, trafficCars[i].trafficCar.transform.position, trafficCars[i].trafficCar.transform.rotation);
				GameObject go = Instantiate(trafficCars[i].trafficCar, Vector3.zero, Quaternion.identity);
				_trafficCars.Add(go.GetComponent<TrafficCar>());
				go.SetActive(false);

			}

		}
		
	}

	void AnimateTraffic () {
		
		for (int i = 0; i < _trafficCars.Count; i++) {
			
			if(reference.transform.position.z > (_trafficCars[i].transform.position.z + 15) || reference.transform.position.z < (_trafficCars[i].transform.position.z - (325)))
				ReAlignTraffic(_trafficCars[i]);
		}
		
	}

	void ReAlignTraffic(TrafficCar realignableObject){

		if(!realignableObject.gameObject.activeSelf)
			realignableObject.gameObject.SetActive(true);

		int randomLine = Random.Range(0, lines.Length );

		realignableObject.currentLine = randomLine;
		realignableObject.transform.position = new Vector3(lines[randomLine].position.x, lines[randomLine].position.y, (reference.transform.position.z + (Random.Range(100, 300))));

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
}
