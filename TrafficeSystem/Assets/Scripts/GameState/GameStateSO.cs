using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObject/GameState")]
class GameStateSO : ScriptableObject {
    public int testVariable;
    public int unLockPlayer;
}

