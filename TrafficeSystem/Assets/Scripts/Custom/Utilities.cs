using UnityEngine;

namespace RandomsUtilities {
  public static class Utilities {
    
    /// <summary>
    /// Returns Player behind gameObject 
    /// </summary>
    public static bool IsPlayerBehindGameObject (Transform player , Transform gameObject, float backStabOffSet = .2f) {
        Vector3 dirDiff = (player.position - gameObject.position).normalized; 
        float dotProduct = Vector3.Dot (player.forward , gameObject.forward);
        // return dotProduct <= -1 + backStabOffSet;
        return dotProduct == 1;
    }
  }
}

