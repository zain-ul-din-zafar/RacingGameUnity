using UnityEngine;

namespace Randoms {
    [RequireComponent(typeof(RandomsPlayerController))]
    public class RandomsAction : MonoBehaviour {
        
        [SerializeField] protected float actionDuration;
        private float _actionDuration;
        
        private void Update () {
            if (!isPlaying) return;
            if (actionDuration >= 0) actionDuration -= Time.deltaTime;
            if (actionDuration <= 0) { OnActionEnd(); }
            isPlaying = actionDuration > 0;
            
            ActionUpdate();
        }
        
        private  void Awake() {
            isPlaying = false;
            _actionDuration = actionDuration;
            Init();
        }

        #region Inheritor APIS
        protected virtual void Init () {} 
        protected virtual void OnActionTrigger(Transform other) { }
        protected virtual void OnActionEnd () { }
        protected virtual void ActionUpdate () { }
        #endregion

        #region APIS
        public bool isPlaying { get; private set; }
        
        /// <summary>
        /// Trigger Action , call only once
        /// </summary>
        public void TriggerAction (Transform other) {
            if (isPlaying) return;
            actionDuration = isPlaying ? (actionDuration + _actionDuration) : _actionDuration;
            isPlaying = true;
            OnActionTrigger(other);
        }
        
        #endregion
    }
}
