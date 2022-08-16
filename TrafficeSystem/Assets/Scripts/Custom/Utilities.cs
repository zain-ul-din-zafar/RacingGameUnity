using System;
using System.Collections;
using System.Collections.Generic;
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
    
    /// <summary>
    /// Quick Sort
    /// CallBack : Action (T lhs , T rhs) => bool | USAGE :- lhs < rhs
    /// Example : Utilities.QuickSort<int> (0 , list.Count -1 , ref list , (int lhs , int rhs)=>{ return lhs < rhs; });
    /// TODOS:- where T : IComparable ?optional
    /// </summary>
    public static void QuickSort <T>(int startIdx, int lastIdx, ref List<T> list , Func < T , T , bool> callBack)  {
      if (startIdx >= lastIdx) return;

      int pivot = startIdx;
      int left = startIdx + 1;
      int right = lastIdx;

      while (left <= right) {
        // Do Qucik Sort
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
    
  }

  
}

