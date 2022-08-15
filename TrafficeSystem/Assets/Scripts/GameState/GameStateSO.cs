using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObject/GameState")]
public class GameStateSO : ScriptableObject {
    public long coins;
    private int level;

    public enum ActionType {
        Coin ,
        Level
    }

    public void SetState  (ActionType actionType , object payload) {
        switch (actionType) {
            case ActionType.Coin:
                // 
                try {
                 coins = Convert.ToInt32 (payload);
                } catch (System.OverflowException exce) {

                }
            break;
            case ActionType.Level :
            //   level = payload;
            break;
        }
    }
}




