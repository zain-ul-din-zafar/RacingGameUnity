using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Randoms;

[RequireComponent(typeof (CollideAction))]
public class BoostAction : RandomsAction {
    [SerializeField] private RandomsCameraController cameraInstance;
    [SerializeField] private float speedIncrementOnBoost;
    private CollideAction _collideAction;
    private float lastSpeed;

    protected override void Init() {
        _collideAction = GetComponent<CollideAction>();
    }
    
    protected override void OnActionTrigger(Transform other) {
        other.gameObject.SetActive(false);
        ParticleSpawnManager.Instance.InstantiateParticle(
            ParticleSpawnManager.ParticleType.BoostPickUpEffect,
            transform.position + transform.forward * 0.5f,
            transform
        );
        ParticleSpawnManager.Instance.InstantiateParticle(
            ParticleSpawnManager.ParticleType.NitroEffect,
            transform.position,
            transform
        );
        lastSpeed = RandomsPlayerController.Instance.Speed();
        RandomsPlayerController.Instance.SetMaxSpeed (lastSpeed + speedIncrementOnBoost);
        _collideAction.collideActionType = CollideAction.CollideActionType.DestroyOther;
        var cam = RandomsCameraController.Instance;
        cam.DoCameraShake(actionDuration);
        cam.ToggleParticles(true);
        cam.canUseBoost = true;
        RandomsAnimationManager.Instance.PlayAnimation(RandomsAnimationManager.AnimType.Boost);
    }
    
    protected override void OnActionEnd () {
        RandomsPlayerController.Instance.SetMaxSpeed (lastSpeed);
        _collideAction.collideActionType = CollideAction.CollideActionType.AddForce;
        var cam = RandomsCameraController.Instance;
        cam.ToggleParticles(false);
        cam.canUseBoost = false;
        RandomsAnimationManager.Instance.PlayAnimation(RandomsAnimationManager.AnimType.Idle);
        ParticleSpawnManager.Instance.DiableParticle(ParticleSpawnManager.ParticleType.NitroEffect);
    }
}
