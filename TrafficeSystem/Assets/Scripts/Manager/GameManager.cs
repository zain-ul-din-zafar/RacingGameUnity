using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region APIS
     
    #endregion
    
    public static GameManager Instance {get; private set;} 
    private GameManager () {}
    public GameStateSO gameState {get; private set;}
    private const string GAMESTATE_PATH = "GameState";

    private void Awake () {
        if (Instance) Destroy (this);
        Instance = this;
        gameState = Resources.Load <GameStateSO> (GAMESTATE_PATH);
    }

}

