using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Randoms;

// Scene Manager Implementations
public partial class UiManager : MonoBehaviour {
    private void Awake () {
        var currentSceneName = SceneManager.GetActiveScene().name.ToLower();
        if (currentSceneName == gamePlaySceneName.ToString().ToLower()) {
         ManageGamePlaySceneUI ();
         return;   
        }
    }

    private void Start () {
        var currentSceneName = SceneManager.GetActiveScene().name.ToLower();
        if (currentSceneName == gamePlaySceneName.ToString().ToLower()) {
         ListenToGamePlaySceneEvents ();
         return;   
        }
    }

    private void ManageGamePlaySceneUI () {
      coinCountUI = GameObject.Find (coinCountUIName).GetComponent <TextMeshProUGUI>();
      energyCountUI = GameObject.Find (energyCountUIName).GetComponent <TextMeshProUGUI> ();
    }

    private void ListenToGamePlaySceneEvents () {
      coinCountUI.text = $"Coins : {GameManager.Instance.gameState.coins}";  
      RandomsPlayerController.Instance.OnHitCoin += (System.Object sender , float coinsValue) => 
        coinCountUI.text = $"Coins : {GameManager.Instance.gameState.coins}";  

      energyCountUI.text = $"Energy : {100}";
      PlayerController.Instance.OnEnergyChange += (System.Object sender, float energyValue) =>
        energyCountUI.text = $"Energy : {energyValue}";
    }

    
}
