using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Randoms;

public class InvisibleAction : RandomsAction {

     [SerializeField] private string shieldLayerMask;
     [SerializeField] private string playerLayerMask;
    
     protected override void OnActionTrigger (Transform other) {
          other.gameObject.SetActive(false);
          ParticleSpawnManager.Instance.InstantiateParticle(
               ParticleSpawnManager.ParticleType.ShieldEffect,
               transform.position + Vector3.up * -0.67f,
               transform
          );
          RevertPlayerLayer(shieldLayerMask);
     }

     protected override void OnActionEnd() {
          ParticleSpawnManager.Instance.DiableParticle(ParticleSpawnManager.ParticleType.ShieldEffect);
          RevertPlayerLayer(playerLayerMask);
     }
     
     private void RevertPlayerLayer (string layerMaskName) { gameObject.layer = LayerMask.NameToLayer(layerMaskName); }
}
