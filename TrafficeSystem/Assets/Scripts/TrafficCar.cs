
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class TrafficCar : MonoBehaviour {
	
	// Getting an Instance of TrafficPooling.
	#region TrafficPooling Instance

	private TrafficPooling _trafficPoolingInstance;
	private TrafficPooling TrafficPooling {
		get {
			if (_trafficPoolingInstance == null) 
				_trafficPoolingInstance = TrafficPooling.Instance;
			return _trafficPoolingInstance;
		}
	}

	#endregion

	private Rigidbody rigid;

	private bool immobilized = false;
	public BoxCollider bodyCollider;
	internal BoxCollider triggerCollider;

	public ChangingLines changingLines;
	public enum ChangingLines{Straight, Right, Left}
	internal int currentLine = 0;
	
	public float maximumSpeed = 10f;
	private float _maximumSpeed = 10f;
	private float desiredSpeed;
	public float distance = 0f;
	private Quaternion steeringAngle = Quaternion.identity;

	public Transform[] wheelModels;
	private float wheelRotation = 0f;

	[Header("Just Lights. All of them will work on ''NOT Important'' Render Mode.")]
	public Light[] headLights;
	public Light[] brakeLights;
	public Light[] signalLights;

	private bool headlightsOn = false;
	private bool brakingOn = false;

	private SignalsOn signalsOn;
	private enum SignalsOn{Off, Right, Left, All}
	private float signalTimer = 0f;
	private float spawnProtection = 0f;

	[Space(10)]

	public AudioClip engineSound;
	private AudioSource engineSoundSource;
	
	void Awake () {

		rigid = GetComponent<Rigidbody>();
		rigid.drag = 1f;
		rigid.angularDrag = 4f;
		rigid.maxAngularVelocity = 2.5f;

		Light[] allLights = GetComponentsInChildren<Light>();

		foreach (Light l in allLights) {
			l.renderMode = LightRenderMode.ForceVertex;
			l.cullingMask = 0;
		}

		distance = 50f;

		if (!bodyCollider) {
			//Debug.LogWarning (transform.name + "is missing collider in TrafficCar script. Select your vehicle collider. Assigning collider automatically now, but it may select wrong collider...");
			bodyCollider = GetComponentInChildren<BoxCollider> ();
		}

		GameObject triggerColliderGO = new GameObject ("TriggerVolume");
		triggerColliderGO.transform.position = bodyCollider.bounds.center;
		triggerColliderGO.transform.rotation = bodyCollider.transform.rotation;
		triggerColliderGO.transform.SetParent(transform, true);
		triggerColliderGO.transform.localScale = bodyCollider.transform.localScale;
		triggerColliderGO.AddComponent<BoxCollider> ();
		triggerColliderGO.GetComponent<BoxCollider>().isTrigger = true;
		triggerColliderGO.GetComponent<BoxCollider> ().size = bodyCollider.size;
		triggerColliderGO.GetComponent<BoxCollider> ().center = bodyCollider.center;

		triggerCollider = triggerColliderGO.GetComponent<BoxCollider>();
		triggerCollider.size = new Vector3(bodyCollider.size.x * 1.5f, bodyCollider.size.y, bodyCollider.size.z + (bodyCollider.size.z * 3f));
		triggerCollider.center = new Vector3(bodyCollider.center.x, 0f, bodyCollider.center.z + (triggerCollider.size.z / 2f) - (bodyCollider.size.z / 2f));

		
		// 	headlightsOn = true;
		// else
		// 	headlightsOn = false;

		engineSoundSource = CreateAudioSource.NewAudioSource(gameObject, "Engine Sound", 2f, 5f, 1f, engineSound, true, true, false);
		//engineSoundSource.transform.localPosition = new Vector3(engineSoundSource.transform.localPosition.x, GameObject.FindObjectOfType<CarCamera>().height, -GameObject.FindObjectOfType<CarCamera>().distance);
		engineSoundSource.pitch = 1.5f;

		rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

		_maximumSpeed = maximumSpeed;

		Transform[] allTransforms = GetComponentsInChildren<Transform>();

		foreach (Transform t in allTransforms) {
			t.gameObject.layer = (int)Mathf.Log (HighwayRacerProperties.Instance.trafficCarsLayer.value, 2);
		}

		triggerCollider.gameObject.layer = LayerMask.NameToLayer("TrafficCarVolume");
	
	}

	void Start(){

		InvokeRepeating("SpeedUp", 4f, 4f);
		InvokeRepeating("ChangeLines", Random.Range(15, 45), Random.Range(15, 45));

		for (int i = 0; i < headLights.Length; i++) {

			headLights [i].renderMode = LightRenderMode.ForceVertex;

		}

		for (int i = 0; i < brakeLights.Length; i++) {

			brakeLights[i].renderMode = LightRenderMode.ForceVertex;

		}

		for (int i = 0; i < signalLights.Length; i++) {

			signalLights[i].renderMode = LightRenderMode.ForceVertex;

		}

	}

	void Update () {

		spawnProtection += Time.deltaTime;

		Lights();
		Wheels ();

	}

	void Lights(){

		signalTimer += Time.deltaTime;

		for (int i = 0; i < signalLights.Length; i++) {

			if(signalsOn == SignalsOn.Off){
				signalLights[i].intensity = 0f;
			}

			if(signalsOn == SignalsOn.Left){
				if(signalTimer >= .5f){
					if(signalLights[i].transform.localPosition.x < 0f)
						signalLights[i].intensity = 0f;
				}else{
					if(signalLights[i].transform.localPosition.x < 0f)
						signalLights[i].intensity = 1f;
				}
				if(signalTimer >= 1f)
					signalTimer = 0f;
			}

			if(signalsOn == SignalsOn.Right){
				if(signalTimer >= .5f){
					if(signalLights[i].transform.localPosition.x > 0f)
						signalLights[i].intensity = 0f;
				}else{
					if(signalLights[i].transform.localPosition.x > 0f)
						signalLights[i].intensity = 1f;
				}
				if(signalTimer >= 1f)
					signalTimer = 0f;
			}

			if(signalsOn == SignalsOn.All){
				if(signalTimer >= .5f){
					signalLights[i].intensity = 0f;
				}else{
					signalLights[i].intensity = 1f;
				}
				if(signalTimer >= 1f)
					signalTimer = 0f;
			}

		}
		 
		for (int i = 0; i < headLights.Length; i++) {

			if(!headlightsOn)
				headLights[i].intensity = 0f;
			else
				headLights[i].intensity = 1f;
			
		}

		for (int i = 0; i < brakeLights.Length; i++) {

			if(brakingOn){

				brakeLights[i].intensity = 1f;

			}else{

				if(!headlightsOn)
					brakeLights[i].intensity = 0f;
				else
					brakeLights[i].intensity = .3f;

			}

		}

	}

	void Wheels(){

		for (int i = 0; i < wheelModels.Length; i++) {

			wheelRotation += desiredSpeed * 20 * Time.deltaTime;
			wheelModels [i].transform.localRotation = Quaternion.Euler (wheelRotation, 0f, 0f);

		}

	}

	void FixedUpdate(){

		if(!immobilized){
			desiredSpeed = Mathf.Clamp(maximumSpeed - Mathf.Lerp(maximumSpeed, 0f, (distance - 10f) / 50f), 0f, maximumSpeed);
		}else{
			desiredSpeed = Mathf.Lerp(desiredSpeed, 0f, Time.fixedDeltaTime);
		}

		if(distance < 50)
			brakingOn = true;
		else
			brakingOn = false;

		rigid.velocity = Vector3.Slerp(rigid.velocity, transform.forward * desiredSpeed, Time.fixedDeltaTime * 3f);
		rigid.angularVelocity = Vector3.Slerp(rigid.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 10f);

		if(!immobilized){

			switch(changingLines){

			case ChangingLines.Straight:
				steeringAngle = Quaternion.identity;
				break;

			case ChangingLines.Left:

				if(currentLine == 0){
					changingLines = ChangingLines.Straight;
					break;
				}

				if(transform.position.x <= TrafficPooling.lines[currentLine - 1].position.x + .5f){
					currentLine --;
					signalsOn = SignalsOn.Off;
					changingLines = ChangingLines.Straight;
				}else{
					steeringAngle = Quaternion.identity * Quaternion.Euler(0f, -5f, 0f);
					signalsOn = SignalsOn.Left;
				}
				break;

			case ChangingLines.Right:
				
				if(currentLine == (TrafficPooling.lines.Length - 1)){
					changingLines = ChangingLines.Straight;
					break;
				}

				if(transform.position.x >= TrafficPooling.lines[currentLine + 1].position.x - .5f){
					currentLine ++;
					signalsOn = SignalsOn.Off;
					changingLines = ChangingLines.Straight;
				}else{
					steeringAngle = Quaternion.identity * Quaternion.Euler(0f, 5f, 0f);
					signalsOn = SignalsOn.Right;
				}
				break;

			}

		}

	}

	void OnTriggerStay(Collider col){

		if((1 << col.gameObject.layer) != HighwayRacerProperties.Instance.trafficCarsLayer.value || col.isTrigger)
			return;

		distance = Vector3.Distance(transform.position, col.transform.position);

	}

	void OnTriggerExit(Collider col){

		if((1 << col.gameObject.layer) != HighwayRacerProperties.Instance.trafficCarsLayer.value)
			return;

	}

	void OnCollisionEnter(Collision col){

		if(immobilized || spawnProtection < .5f)
			return;

		immobilized = true;
		signalsOn = SignalsOn.All;

	}

	void OnReAligned(){

		immobilized = false;
		spawnProtection = 0f;
		rigid.velocity = Vector3.zero;
		rigid.angularVelocity = Vector3.zero;
		signalsOn = SignalsOn.Off;
		changingLines = ChangingLines.Straight;
		maximumSpeed = Random.Range(_maximumSpeed, _maximumSpeed * 1.5f);
		distance = 50f;

	}

	void SpeedUp(){

		distance = 50f;

	}

	void ChangeLines(){

		if(changingLines == ChangingLines.Left || changingLines == ChangingLines.Right)
			return;
        
		int randomNumber = Random.Range(0, 2);

		changingLines = randomNumber == 0 ? ChangingLines.Left : ChangingLines.Right;
		
	}

}
