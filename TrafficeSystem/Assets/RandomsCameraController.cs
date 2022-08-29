using System;
using UnityEngine;
using DG.Tweening;
using Randoms;
using RandomsUtilities;

public class RandomsCameraController : MonoBehaviour {
    [SerializeField] private GameObject target;
    [SerializeField] private float zOffSet = 10;
    [SerializeField] private float cameraFieldOfViewOnBoost = 60;
    [SerializeField] private Transform boostParticles;
    [SerializeField] private float yDeadZone = 5f;
    [SerializeField] private float yFollowSpeed = 5f;
    
    private float _cameraFieldOfView;
    private Camera _camera;
    private float _yOffSet;
    private float _lastYPos;
    private Transform _camChild;
    
    public static RandomsCameraController Instance { get; private set; }
    [HideInInspector] public bool canUseBoost;

    private void Awake() {
        Instance = this;
        ToggleParticles(false);
        _camera = GetComponentInChildren<Camera>();
        _cameraFieldOfView = _camera.fieldOfView;
        _yOffSet = transform.position.y - target.transform.position.y;
        _lastYPos = transform.position.y;
        _camChild = transform.GetChild(0);
    }
    
    private void LateUpdate() {
        if (canUseBoost) {
            _camera.fieldOfView = Mathf.Lerp (_camera.fieldOfView, cameraFieldOfViewOnBoost, 5 * Time.deltaTime);
        }
        else {
            _camera.fieldOfView = Mathf.Lerp (_camera.fieldOfView, _cameraFieldOfView, Time.deltaTime);
        }
        

        var targetYPos = target.transform.position.y;
        
        if (targetYPos > yDeadZone) {
            var newYPos = Mathf.Lerp (transform.position.y, targetYPos + _yOffSet, yFollowSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newYPos, target.transform.position.z - zOffSet);
        }
        else {
            var newYPos = Mathf.Lerp (transform.position.y, _lastYPos, yFollowSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newYPos, target.transform.position.z - zOffSet);
        }
    }
    
    /// <summary>
    /// Enable Camera shake for certain duration
    /// </summary>
    /// <param name="duration"></param>
    public void DoCameraShake(float duration) {
        _camChild.DOShakePosition(duration, 0.3f, 10, 180, false, false);
    }
    
    /// <summary>
    /// Set Particles to active or inactive
    /// </summary>
    /// <param name="state"></param>
    public void ToggleParticles(bool state) {
        if (!boostParticles) return;
        boostParticles.gameObject.SetActive(state);
    }
}
