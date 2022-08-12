using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    
    public enum SoundClip {
      CoinSound ,
    }

    public static SoundManager Instance {get; private set;} 
    public void PlaySound (SoundClip soundClip) => audioSource.PlayOneShot (soundArray [soundClip]);
    
    #region Helper
     [SerializeField] private AudioSource audioSource;
     private const string LOADEDPATH = "Audio/";
     private Dictionary <SoundClip , AudioClip> soundArray;

     private void Awake () {
       Instance = this;
       if (!audioSource)
        audioSource = FindObjectOfType<AudioSource>();
       soundArray = new Dictionary<SoundClip, AudioClip> ();  
       LoadSoundClips ();
     }
    
     private void LoadSoundClips () {
        foreach (SoundClip soundClip in System.Enum.GetValues (typeof (SoundClip))) 
            soundArray [soundClip] = Resources.Load <AudioClip> (LOADEDPATH + soundClip.ToString());
     }

     private SoundManager () {}
    #endregion
}
