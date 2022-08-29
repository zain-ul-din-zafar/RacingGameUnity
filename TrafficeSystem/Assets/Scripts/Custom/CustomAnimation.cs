using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomAnimation : MonoBehaviour {
    private enum AnimationType {
        Rotate
    }

    [SerializeField] private AnimationType animationType;
    [SerializeField] private float duration = 1f;
    
    private void Update() {
        switch (animationType) {
            case AnimationType.Rotate:
                // rotate in y in given duration
                transform.eulerAngles += new Vector3(0, 360f * Time.deltaTime / duration, 0);
                break;
        }
    }
}
