using System.Collections;
using UnityEngine;
using DG.Tweening;
using Randoms;

public class CollideAction : Randoms.RandomsAction {

    public enum CollideActionType {
        AddForce,
        DestroyOther
    }

    
    
    public CollideActionType collideActionType;
    protected override void Init() {
        collideActionType = CollideActionType.AddForce;
    }
    
    protected override void OnActionTrigger(Transform other) {
        switch (collideActionType) {
            case CollideActionType.AddForce: AddForce(other); break;
            case CollideActionType.DestroyOther: DestroyCarOnCollision (other); break;
        }
    }
    
    private void DestroyCarOnCollision(Transform other) {
        var spawnPos = new Vector3(other.position.x, other.position.y + 2f,
            transform.position.z + 1f);
        ParticleSpawnManager.Instance.InstantiateParticle(ParticleSpawnManager.ParticleType.DestroyEffect, spawnPos,
            transform);
        StressReceiver.Instance.InduceStress(0.5f);
        SoundManager.Instance.PlaySound(SoundManager.SoundClip.DestorySound);
        other.DOMoveY(30, 0.3f);
    }

    private void AddForce (Transform other) {
        SoundManager.Instance.PlaySound(SoundManager.SoundClip.CrashSound);
        RandomsPlayerController.Instance.ChangeRandomLine();
        ParticleSpawnManager.Instance.InstantiateParticle(
            ParticleSpawnManager.ParticleType.HitEffect,
            transform.position,
            gameObject.transform
        );

        // RandomsAnimationManager.Instance.PlayAnimation(RandomsAnimationManager.AnimType.Collide);
        RandomsAnimationManager.Instance.animator.SetBool("IsCollide", true);
        StartCoroutine(AddSlideAnim(0.7f));
        other.DOMoveZ(other.position.z + 50f, 1f).SetEase(Ease.OutFlash);
    }
    
    private IEnumerator AddSlideAnim(float delay) {
        yield return new WaitForSeconds(delay);
        RandomsAnimationManager.Instance.animator.SetBool("IsCollide", false);
        RandomsAnimationManager.Instance.PlayAnimation(RandomsAnimationManager.AnimType.Slide);
        StartCoroutine(RandomsPlayerController.Instance.AddIdleDelay(3f));
    } 
}


