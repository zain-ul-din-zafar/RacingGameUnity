using System.Collections.Generic;
using UnityEngine;


namespace TrafficSystem {
     
     public class TrafficSpawner : MonoBehaviour {
          // public static TrafficSpawner instance { get; private set;  }

          [SerializeField] private List<Vehicle> vehicles;
          [SerializeField] private List<SpawnPosition> spawnPositions;
          [SerializeField] private float spawnInterval;
          private List<GameObject> _trafficPool;

          // private void Awake () {
          //      // if (instance != null) Destroy(this);
          //      // instance = this;
          // }
          
          private void CreatePool () {
               _trafficPool = new List<GameObject>();
               
               foreach (var vehiclePrefab in vehicles) {
                    Debug.Log("Vehicle : " + vehiclePrefab.vehicle.name);
                    Debug.Log(vehiclePrefab.count + " prefab in vheicle list");    
                    for (int i = 0; i < vehiclePrefab.count; i++) {
                         var vehicle = Instantiate(vehiclePrefab.vehicle, Vector3.zero , Quaternion.identity);
                         vehicle.SetActive(false);
                         _trafficPool.Add(vehicle);
                    }
               }

               Debug.Log("Pool created");
          }
          
          private void OnEnable() {
               CreatePool();
               // for (;;) {
                    Debug.Log(_trafficPool.Count + " vehicles in pool");
                    // yield return new WaitForSeconds(spawnInterval);
                    for (var i = 0; i < _trafficPool.Count; i += 1) {
                         var spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count)];
                         SpawnVehicle(_trafficPool[i],
                              spawnPos.spawnInitialPosition);
                         spawnPos.IncrementSpawnPosition();
                    }
               // }
          }
          
          private void SpawnVehicle(GameObject vehicle , Vector3 spawnPosition) { 
               vehicle.SetActive(true);
               vehicle.transform.position = spawnPosition;
          }
          
     }

     
     [System.Serializable]
     public class Vehicle {
          public GameObject vehicle;
          [Range(0 , 20)][Tooltip("Count in pool")] public int count = 1;
     }
     
     [System.Serializable]
     public class SpawnPosition {
          public Vector3 spawnInitialPosition;
          [SerializeField] private Vector3 minIncrementInPosition, maxIncrementInPosition;
          
          public void IncrementSpawnPosition() =>
               spawnInitialPosition += GetRandomIncrementInPosition(minIncrementInPosition , maxIncrementInPosition);
          
          private static Vector3 GetRandomIncrementInPosition(Vector3 minIncrementInPosition, Vector3 maxIncrementInPosition) =>
               new Vector3(
                    Random.Range(minIncrementInPosition.x, maxIncrementInPosition.x),
                    Random.Range(minIncrementInPosition.y, maxIncrementInPosition.y),
                    Random.Range(minIncrementInPosition.z, maxIncrementInPosition.z)
               );
     }
}




