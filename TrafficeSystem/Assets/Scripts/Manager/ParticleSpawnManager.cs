using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawnManager : MonoBehaviour {
    
    public  void InstantiateParticle (ParticleType particleType , Vector3 pos )  {
       particlesDictionary [particleType].transform.position = pos;
    //    particlesDictionary [particleType].SetActive (true);
       particlesDictionary[particleType].GetComponent <ParticleSystem>().Play();
    } 
    
    public  void InstantiateParticle (ParticleType particleType , Vector3 pos , Transform parent)  {
       particlesDictionary [particleType].transform.position = pos;
       particlesDictionary [particleType].transform.SetParent (parent);
       particlesDictionary [particleType].SetActive (true);
       particlesDictionary[particleType].GetComponent <ParticleSystem>().Play();
    } 
    
    public void DiableParticle (ParticleType particleType) => particlesDictionary [particleType].GetComponent <ParticleSystem>().Stop();
    
#region PRIVATE 
    // TODOS : Add Particle Name here
    public enum ParticleType {
        CoinHitEffect,
        HitEffect,
        NitroEffect,
        DestoryEffect,
        BoostPickUpEffect 
    }
    
    public static ParticleSpawnManager Instance {get; private set;}
    private Dictionary <ParticleType , GameObject> particlesDictionary;
    private const string PATH = "Particles/";

    private void Awake () {
        if (Instance) Destroy (this);
        Instance = this;
        particlesDictionary = new Dictionary<ParticleType, GameObject> ();
        // Load Particles from Resources
        foreach (ParticleType particleType in System.Enum.GetValues (typeof (ParticleType))) {
           var loadedParticle = Resources.Load <GameObject> (PATH + particleType.ToString());
           var particle = Instantiate (loadedParticle);
           Debug.Log (Quaternion.identity);
           particlesDictionary [particleType] = particle;
           particle.GetComponent <ParticleSystem>().Stop();
        }
    }
#endregion
}
