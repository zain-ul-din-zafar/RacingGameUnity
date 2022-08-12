using System.Collections.Generic;
using UnityEngine;


public class LevelSpawner : MonoBehaviour {

    private static LevelSpawner Instance { get; set; }
    
    #region SpawnLogic
    
    [SerializeField] 
    private List<GameObject> levelPrefabs;
    [Space(4)][Tooltip("Initial Position of the first level")] [SerializeField] 
    private Vector3 spawnPosition;
    [Tooltip("Increment in Position when spawning a new level")] [SerializeField] 
    private Vector3 incrementVector;
    [SerializeField] 
    private GameObject playerFollowCamera;
    [Tooltip("how far camera should be when to spawn next level")] [SerializeField]
    private float endPointOffSet;
    
    private Transform _cameraTransform;
    private int _currentSpawnLevel;
    private float _endPoint;
    private List<Level> _levels;
    
    private void Awake() {
        if (Instance != null) Destroy(this);
        Instance = this;
        _endPoint = incrementVector.z;
        _levels = new List<Level>();
    }

    private void Start() {
        // creates pool
        levelPrefabs.ForEach(levelPrefab => {
            var prefab = Instantiate(levelPrefab, spawnPosition, Quaternion.identity);
            var endPointRef = new GameObject("EndPoint");
            float total = 0;
            endPointRef.transform.position = new Vector3(0, 0, _endPoint);
            endPointRef.transform.parent = prefab.transform;
            prefab.SetActive(false);
            _levels.Add(new Level{levelPrefab = prefab , endPoint = endPointRef.transform});
        });
        
        _levels[_currentSpawnLevel].levelPrefab.SetActive(true);
        _cameraTransform = playerFollowCamera.transform;
    }

    private void Update() {
        var cameraEndPoint = endPointOffSet + _cameraTransform.position.z;
        if (cameraEndPoint > _endPoint) SpawnNextLevel();
    }

    /// <summary>
    /// Spawns a new level and disable previous one. 
    /// </summary>
    private void SpawnNextLevel() {
        _currentSpawnLevel = (_currentSpawnLevel + 1) % levelPrefabs.Count;
        spawnPosition += incrementVector;
        _endPoint += incrementVector.z;
        _levels[_currentSpawnLevel].levelPrefab.transform.position = spawnPosition;
        _levels[_currentSpawnLevel].levelPrefab.SetActive(true);

        // Disable previous levels
        _levels.ForEach(level => {
            var cameraPosZ = _cameraTransform.position.z;
            if (level.endPoint.position.z < cameraPosZ)
                level.levelPrefab.SetActive(false);
        });
    }
    
    #endregion
    
    [System.Serializable]
    public class Level {  public GameObject levelPrefab; public Transform endPoint; }
}

