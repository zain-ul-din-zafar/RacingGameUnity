using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour {
    public static LevelSpawner Instance { get; private set; }

    #region SpawnLogic
    
    [SerializeField] private List <GameObject> levels;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Vector3 incrementVector;
    private int currentSpawnLevel ;
    private Stack<GameObject> levelsStack;

    private void Awake() {
        if (Instance != null) Destroy(this);
        Instance = this;
        levelsStack = new Stack<GameObject>();
    }
    private void Start() { levelsStack.Push(Instantiate(levels[currentSpawnLevel] , spawnPosition , Quaternion.identity)); }    
    
    #endregion
    
    /// <summary>
    /// Spawns a new level and destroy previous one. 
    /// </summary>
    public void SpawnNextLevel () {
        Destroy(levelsStack.Pop() , 1f);
        currentSpawnLevel = (currentSpawnLevel + 1) % levels.Count;
        spawnPosition += incrementVector;
        levelsStack.Push(Instantiate(levels[currentSpawnLevel] , spawnPosition , Quaternion.identity));
    }
    
    // Testing
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) SpawnNextLevel();
    }
    
}
