using UnityEngine;
using Randoms;
using DG.Tweening;

public class FlyAction : RandomsAction {

    [SerializeField] private float moveUpHeight = 5f;
    [SerializeField] private float moveUpDuration = 1f;
    [SerializeField] private float rotationDuration = 1f;
    
    protected  override  void OnActionTrigger (Transform other) { 
      other.gameObject.SetActive(false);
      RandomsPlayerController.Instance.SetGravity(false);    
      RandomsPlayerController.Instance.CanChangeLines(false);
      RandomsPlayerController.Instance.OverRideQuaternion(true);
      transform.DOMoveY (transform.position.y + moveUpHeight, moveUpDuration).SetEase (Ease.InFlash).OnComplete(() => {
          RandomsPlayerController.Instance.CanChangeLines(true);
          RandomsPlayerController.Instance.OverRideQuaternion(false);
      });
      transform.DORotate (new Vector3 (0,0 , 360), rotationDuration , RotateMode.FastBeyond360).SetEase (Ease.InOutSine);
      RandomsAnimationManager.Instance.animator.SetBool("IsFlying", true);
    }
    
    protected override void OnActionEnd () {
        RandomsPlayerController.Instance.SetGravity(true);
        RandomsAnimationManager.Instance.animator.SetBool("IsFlying", false);
    }
}
