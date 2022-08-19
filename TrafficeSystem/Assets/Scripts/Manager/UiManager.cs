using UnityEngine;
using TMPro;

public partial class UiManager : MonoBehaviour {
    
    [Space (10)] [Header ("Gameplay Scene")]
    [SerializeField] private string coinCountUIName;
    [SerializeField] private SceneLoaderManager.SceneName gamePlaySceneName;
    
    private TextMeshProUGUI coinCountUI;
}
