using UnityEngine;
using Randoms;
using DG.Tweening;

public class MagnetAction : RandomsAction {

    private Animator _animator;

    protected override void Init() {
        _animator = GetComponentInChildren <Animator>();
    }
    
    protected override void OnActionTrigger(Transform other) {
        other.gameObject.SetActive(false);
        ParticleSpawnManager.Instance.InstantiateParticle(
            ParticleSpawnManager.ParticleType.MagnetEffect,
            transform.position + Vector3.up * 0.5f + Vector3.forward * 0.3f,
            transform
        );
    }

    protected override void ActionUpdate() {
        TrafficPooling.Instance.IfAnyOfpowerUps(transform, (Transform player, Transform powerUp) => {
            float megnetFieldRange = player.position.z + 25f;
            float powerUpPosZ = powerUp.position.z;
            return powerUpPosZ <= megnetFieldRange && player.position.z < powerUpPosZ + 3f;
        }, (powerUp) => {
            powerUp.DOMove(transform.position, 0.3f);
        });
        _animator.SetBool("Boost", true);
        _animator.SetBool("Idle", false);
        _animator.SetBool("Go", false);
    }
    
    protected override void OnActionEnd() {
        ParticleSpawnManager.Instance.DiableParticle(ParticleSpawnManager.ParticleType.MagnetEffect);
    }
}
