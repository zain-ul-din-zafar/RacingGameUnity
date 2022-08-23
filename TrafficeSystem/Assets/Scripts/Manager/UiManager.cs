using UnityEngine;
using TMPro;

public partial class UiManager : MonoBehaviour {
    
    [Space (10)] [Header ("Ui Manager")]
    [SerializeField] private string coinCountUIName;
    [SerializeField] private string energyCountUIName;
    [SerializeField] private SceneLoaderManager.SceneName gamePlaySceneName;
    
    private TextMeshProUGUI coinCountUI;
    private TextMeshProUGUI energyCountUI;
}



