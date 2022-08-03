using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool canGo ;
    [SerializeField] private float speed = 10f;
    [SerializeField] private Camera followCamera;
    [SerializeField] private float cameraOffset = 0.5f;
    [SerializeField] private float cameraRotation = 0f;
    [SerializeField] private float cameraRotationSpeed = 0.5f;
    [SerializeField] private float cameraRotationLimit = 45f;
    [SerializeField] private float touchSenstivity = 2f;
    private float timeDelay = 2.5f;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        canGo = false;
    }
    float x = 0, z = 0;
    private void FixedUpdate() {
        
        timeDelay -= Time.time;
        if (timeDelay <= 0) {
            canGo = true;
            rb.isKinematic = false;
        }
        if (!canGo) return;
        
        // //Check if the device running this is a handheld
        // if (SystemInfo.deviceType == DeviceType.Handheld) {
        //     
        //        
        // }
        // else {
        //     x = Input.GetAxis("Horizontal");
        //     z = Input.GetAxis("Vertical");
        // }
        
       
        
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Moved) {
                x = touch.deltaPosition.x + touchSenstivity;
                
            } else {
                x = 0;
                // z = 0;
            }

            if (touch.phase == TouchPhase.Began) {
                rb.isKinematic = false;
                canGo = true;
            }
        } 
        else {
            x = Input.GetAxis("Horizontal");
            // z = Input.GetAxis("Vertical");
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            canGo = true;
            rb.isKinematic = false;
        }
        
        z += Time.deltaTime;
        Vector3 movement = new Vector3(x, 0.0f, z);
        rb.AddForce(movement * speed * Time.deltaTime);
        
        // follow player
        followCamera.transform.position = new Vector3(transform.position.x , followCamera.transform.position.y , transform.position.z - cameraOffset);
      //  followCamera.transform.LookAt(transform);
    }
}
