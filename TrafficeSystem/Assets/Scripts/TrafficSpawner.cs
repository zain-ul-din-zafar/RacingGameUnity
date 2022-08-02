using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficSystem {
     public class TrafficSpawner : MonoBehaviour {
          [SerializeField] private List<Vehicle> vehicles;
          [Header("Time interval in sec")][SerializeField] private float spawnInterval = 0.5f;
          [SerializeField] private float spawnPointIncrement = 1f;
          
          private void Start() {
               IEnumerator coroutine  = Spawn();
               StartCoroutine(coroutine);
          }
          
          private IEnumerator Spawn() {
               while (true) {
                    yield return new WaitForSeconds(spawnInterval);
                    vehicles.ForEach(vehicle => {
                         Instantiate(vehicle.vehicle , vehicle.spawnStartPoint.position , Quaternion.identity);
                         spawnPointIncrement += 1f;
                    });
               }
          }
     }

     [System.Serializable]
     public class Vehicle {  
          public GameObject vehicle;
          public Transform spawnStartPoint;
     }
}




