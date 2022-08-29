using System;
using UnityEngine;
using RandomsUtilities;
using TMPro;
using UnityEngine.Serialization;

namespace Randoms {
    
    [RequireComponent(typeof(Animator))]
    public class RandomsAnimationManager : MonoBehaviour {
        public Animator animator { get; private set; }
        public GameObject UI;
        private const string PARAMNAME = "BasicCombatFlag";
        
        public static RandomsAnimationManager Instance { get; private set; }
        
        public enum AnimType {
            Idle,
            Slide,
            Boost
        }

        public AnimType animationType;
         
        private void Awake() {
            Instance = this;
            animator = GetComponent<Animator>();
        }
        
        public void PlayAnimation (AnimType animType) {
            switch (animType) {
                case AnimType.Idle:
                    animator.SetFloat(PARAMNAME , 0);
                break;    
                case AnimType.Slide:
                    animator.SetFloat(PARAMNAME , 1);
                break;
                case AnimType.Boost:
                    animator.SetFloat(PARAMNAME, 2);
                break;
            }
        }

        public void StartGame() {
            UI.SetActive(true);
        }
        
    }
}

