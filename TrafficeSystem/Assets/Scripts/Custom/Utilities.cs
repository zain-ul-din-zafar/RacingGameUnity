using System;
using System.Collections.Generic;
using UnityEngine;

// Dependencies: NONE
namespace RandomsUtilities {
  public static class Utilities {
    
    /// <summary>
    /// Returns Player behind gameObject 
    /// </summary>
    public static bool IsPlayerBehindGameObject (Transform player , Transform gameObject, float backStabOffSet = .2f) {
        Vector3 toTarget = (gameObject.position - player.position).normalized;
        return Vector3.Dot(toTarget, player.transform.forward) > 0; // GameObject is in the front of player
    }
    
    /// <summary>
    /// Quick Sort
    /// CallBack : Action (T lhs , T rhs) => bool | USAGE :- lhs < rhs
    /// Example : Utilities.QuickSort<int> (0 , list.Count -1 , ref list , (int lhs , int rhs)=>{ return lhs < rhs; });
    /// TODOS:- where T : IComparable ? optional
    /// </summary>
    public static void QuickSort <T>(int startIdx, int lastIdx, ref List<T> list , Func < T , T , bool> callBack)  {
      if (startIdx >= lastIdx) return;

      int pivot = startIdx;
      int left = startIdx + 1;
      int right = lastIdx;

      while (left <= right) {
        // Do Quick Sort
        var pivotElement = list [pivot];
        if ( !callBack(list[pivot] , pivotElement) && callBack(list[right] , pivotElement)) 
          (list[left] , list[right]) = (list[right] , list[left]);
        if (!callBack (pivotElement , list[left])) left += 1;
        if (callBack(pivotElement , list[right])) right -= 1;
      }
       
      (list[pivot] , list[right]) = (list[right] , list[pivot]);
      
      QuickSort<T> (0 , right -1, ref list, callBack);
      QuickSort<T> (right + 1, lastIdx, ref list, callBack);
    }
    


    public class ActionHandler {
      public ActionHandler () { canInvokeAction = false; }
      public bool canInvokeAction ;
      public void PlayOneShot (Action action) { 
        if (!canInvokeAction) return;
        action (); 
      }
    }
    
    /* Vector simplifier */
    public static Vector3 Vector (float x, float y, float z) => new Vector3 (x, y, z);
    public static Vector3 VectorX (Transform transform, float x) => new Vector3 (x, transform.position.y, transform.position.z);
    public static Vector3 VectorY (Transform transform, float y) => new Vector3 (transform.position.x, y, transform.position.z);
    public static Vector3 VectorZ (Transform transform, float z) => new Vector3 (transform.position.x, transform.position.y, z);
    public static Vector3 VectorXY (Transform transform, float x, float y) => new Vector3 (x, y, transform.position.z);
    public static Vector3 VectorXZ (Transform transform, float x, float z) => new Vector3 (x, transform.position.y, z);
    public static Vector3 VectorYZ (Transform transform, float y, float z) => new Vector3 (transform.position.x, y, z);
        
    public static Vector3 VectorX (Vector3 vector, float x) => new Vector3 (x, vector.y, vector.z);
    public static Vector3 VectorY (Vector3 vector, float y) => new Vector3 (vector.x, y, vector.z);
    public static Vector3 VectorZ (Vector3 vector, float z) => new Vector3 (vector.x, vector.y, z);
    public static Vector3 VectorXY (Vector3 vector, float x, float y) => new Vector3 (x, y, vector.z);
    public static Vector3 VectorXZ (Vector3 vector, float x, float z) => new Vector3 (x, vector.y, z);
    public static Vector3 VectorYZ (Vector3 vector, float y, float z) => new Vector3 (vector.x, y, z);
    
    // Float Utilities
    public static class FloatUtil {

      public static bool IsEqual(float a, float b, float tolerance = 0.001f) => Math.Abs(a - b) < tolerance;

      public static bool IsGreater(float a, float b) => a > b; 

      public static bool IsLess(float a, float b) => a < b;
      
    }
  }
  
}

