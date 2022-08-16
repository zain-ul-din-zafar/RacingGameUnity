using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomsUtilities;

public class Testing : MonoBehaviour {
    private List <int> list = new List<int> ();

    private void Awake () {
        list.Add (3);
        list.Add (1);
        list.Add (9);
        list.Add (5);
        list.Add (7);

        Utilities.QuickSort<int> (0 , list.Count -1 , ref list , (int lhs , int rhs)=>{ return lhs < rhs; });
        
        foreach (var i in list)
          Debug.Log (i);
    }


}