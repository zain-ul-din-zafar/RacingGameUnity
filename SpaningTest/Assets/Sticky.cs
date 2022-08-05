#define AUTO_DISABLE
 #define DEBUG_DISPLAY
 
 using UnityEngine;
 
 [RequireComponent (typeof (Rigidbody))]
 [RequireComponent (typeof (Collider))]
 public class Sticky : MonoBehaviour
 {
     [Tooltip ("Surface Rigidbody to Stick To.")]
     [SerializeField] Rigidbody _surfaceBody;
     [Tooltip ("Use Force to attract object to surface.\n" +
         "Moves position when off.")]
     [SerializeField] bool _useForce;
     [SerializeField] float _forceMultiplier = 1f;
     [SerializeField] ForceMode _forceMode = ForceMode.VelocityChange;
 
     Collider _surfaceCollider;
 
     Rigidbody _rigidbody;
     Collider _collider;
 
     Vector3 _origin, _destination, _offset, _delta, _target;
 
     void Awake ()
     {
         _rigidbody = GetComponent<Rigidbody>();
         _collider = GetComponent<Collider>();
         _surfaceCollider = _surfaceBody.GetComponent<Collider>();
     }
 
     void FixedUpdate ()
     {
         _destination = _surfaceCollider.ClosestPoint(_rigidbody.position);
         _origin = _collider.ClosestPoint(_destination);
         _delta = _destination - _origin;
         _offset = _origin - _rigidbody.position;
         _target = _destination - _offset;
 
         if (_useForce)
             _rigidbody.AddForce(_delta * _forceMultiplier, _forceMode);
         else
             _rigidbody.MovePosition(_target);
     }
 
     #if AUTO_DISABLE
     void OnCollisionEnter (Collision col)
     {
         if (col.rigidbody == _surfaceBody)
             enabled = false;
     }
 
     void OnCollisionExit (Collision col)
     {
         if (col.rigidbody == _surfaceBody)
             enabled = true;
     }
     #endif
 
     #if DEBUG_DISPLAY
     void OnDrawGizmos ()
     {
         Gizmos.color = Color.yellow;
         Gizmos.DrawWireSphere(_origin, 0.1f);
         Gizmos.color = Color.cyan;
         Gizmos.DrawWireSphere(_destination, 0.1f);
         Gizmos.color = Color.green;
         Gizmos.DrawRay(_origin, _delta);
         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(_target, 0.1f);
     }
     #endif
 }