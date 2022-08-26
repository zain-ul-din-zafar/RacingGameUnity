using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomsUtilities;
using DG.Tweening;


[RequireComponent(typeof (Camera))]
public class CameraController : MonoBehaviour {
    
    public static CameraController Instance {get; private set;}
    [SerializeField] private float cameraOffSet = 10f;
    [SerializeField] private float cameraFieldOfViewOnBoost = 65f;
    [SerializeField] private float cameraPosOnFly = 12f;

    [Space (10)][Header ("Camera Effect")]
    [Tooltip ("Optional Field")][SerializeField]private GameObject warp;
    
    
    private Camera _camera;
    private Transform target;
    private PlayerController playerInstance;
    private float cameraFieldOfView;
    private float lastY;

    private Utilities.ActionHandler playActionHandler, pauseActionHandler;
    private void Awake () {
        if (Instance) Destroy (this); 
        Instance = this;
        playActionHandler = new Utilities.ActionHandler (); 
        pauseActionHandler = new Utilities.ActionHandler ();
    }
    
    private void Start () {
        playerInstance = PlayerController.Instance;
        target = playerInstance.gameObject.transform;
        _camera = GetComponent <Camera> ();
        cameraFieldOfView = _camera.fieldOfView;
        ToggleParticles (false);
        playerInstance.OnFly += (sender, duration) => {
            transform.DOMoveY(cameraPosOnFly, duration + 0.3f);
        };
        lastY = transform.position.y;
    }
    
    // Camera Follow
    private void LateUpdate() {

        if (Input.GetKeyDown(KeyCode.S)) {
            transform.DOMoveY(lastY, 4f);
        }
        
        bool isNitroPlaying = playerInstance.isNitroEffectPlaying;
        
      
        _camera.gameObject.transform.position = new Vector3(_camera.transform.position.x,
                transform.position.y, target.position.z - cameraOffSet);
        
        if (isNitroPlaying) { 
            pauseActionHandler.canInvokeAction = true;
            _camera.fieldOfView = Mathf.Lerp (_camera.fieldOfView, cameraFieldOfViewOnBoost, 5 * Time.deltaTime);
            playActionHandler.PlayOneShot (()=> {
                ToggleParticles ();
                transform.DOShakePosition (playerInstance.BoostTime(), 0.3f, 10, 180, false, false);
            });
            playActionHandler.canInvokeAction = false; 
        } else {
            playActionHandler.canInvokeAction = true; 
            _camera.fieldOfView = Mathf.Lerp (_camera.fieldOfView, cameraFieldOfView, Time.deltaTime);
            pauseActionHandler.PlayOneShot (()=> { 
                ToggleParticles (false); 
            });
            pauseActionHandler.canInvokeAction = false;
        }
    }
    
    private void ToggleParticles (bool state = true) {
        if (!warp) return; 
        warp.SetActive (state);
    }
    
    
    // if (isNitroEffectPlaying) 
        // {
        //  // smoothly follow player in z direction
        //  Vector3 target = new Vector3(_camera.transform.position.x, _camera.transform.position.y, transform.position.z -10f);
        //  _camera.transform.position = Vector3.Lerp(_camera.transform.position, target, Time.deltaTime * 10f);
        // }
        // else 
        // {
        // //  _camera.gameObject.transform.position = new Vector3(_camera.transform.position.x , _camera.transform.position.y, transform.position.z - cameraOffSet);
        //  _camera.gameObject.transform.position = Vector3.Lerp(_camera.transform.position , new Vector3(_camera.transform.position.x , _camera.transform.position.y, transform.position.z - cameraOffSet) , Time.deltaTime * SmoothFollowFactor); 
        //  //new Vector3 (_camera.transform.position.x , _camera.transform.position.y , SmoothDampValue (_camera.transform.position.z , new Vector3(_camera.transform.position.x , _camera.transform.position.y, transform.position.z - cameraOffSet) , 1f));
        // }
}
