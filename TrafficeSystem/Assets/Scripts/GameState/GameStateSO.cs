using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObject/GameState")]
public class GameStateSO : ScriptableObject {
    public int coins;
    private int level;

    private void Awake () {
        // load data
        coins = PlayerPrefs.GetInt ("GAMESTATE_COINS");
    }

    private void OnDestory () {
        PlayerPrefs.SetInt ("GAMESTATE_COINS" , coins);
    }
}





