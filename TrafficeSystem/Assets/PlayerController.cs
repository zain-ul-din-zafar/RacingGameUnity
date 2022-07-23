using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

[RequireComponent(typeof (CharacterController))]
public class PlayerController : MonoBehaviour {
    private CharacterController characterController;
    [SerializeField] private float speed = 6.0f;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<Vector3> List;
    [SerializeField] private float cameraOffSet = 3f;
    private int currentIdx = 1;
    [SerializeField] private float lerpDuration = 0.5f;

    [SerializeField] private Animator _animator;

    [SerializeField] private float slideTime = 2f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpTime = 0.5f;
   
    [SerializeField] private Transform meshTransform;
    
    private float vVelocity;
    private void Awake() {  characterController = GetComponent<CharacterController>(); }

    private IEnumerator Start() {
        while (true) {
            yield return new WaitForSeconds(5f);
            speed += Time.deltaTime;
        }
    }

    private void Update() {

        slideTime -= Time.deltaTime;
        
        if (slideTime > 0) {
            _animator.SetBool("Go", true);
        } else {
            _animator.SetBool("Go", false);
            _animator.SetBool("Idle" , true); // todos!
        }
       
        if (characterController.isGrounded) {
            _animator.SetBool("Jump" , false);
            vVelocity = 0;
            if (Input.GetKeyDown(KeyCode.Space)) 
                Jump();
        } 
        
        
        
        float z = 1f;
        Vector3 move = transform.forward * z * speed * Time.deltaTime;
        vVelocity -= gravity * Time.deltaTime;
        move.y = vVelocity * Time.deltaTime;
        characterController.Move(move);
        
        
        // move player left and right
        if (Input.GetKeyDown(KeyCode.LeftArrow)) 
            // move left
            MoveLeft();
        else if (Input.GetKeyDown(KeyCode.RightArrow)) 
            // move right
            MoveRight();
        else 
            _animator.SetFloat("Direction" , 0);
        

        GetTouchInput();
        
        // lerp to left
        //transform.position = Vector3.Lerp(transform.position, new Vector3 (List[currentIdx].x , transform.position.y , transform.position.z ), Time.deltaTime * lerpSpeed);
        
        // move to x directions
        
        
        // follow player
        
        
        // Add Gravity
        
    }

    
    
    
    private void MoveLeft() {
        if (currentIdx > 0) {
            currentIdx -= 1;
            // _animator.SetFloat("Direction" , -1);

            _animator.SetBool("Idle" , false);
            transform.DOMoveX(List[currentIdx].x , lerpDuration).SetEase(Ease.InOutSine).OnComplete(() => {
                _animator.SetBool("Idle" , true);
            });
                
            // rotate in y direction
            meshTransform.DORotate(new Vector3(0, -45, 0), lerpDuration ).OnComplete(() => {
                meshTransform.DORotate(new Vector3(0, 0, 0), lerpDuration );
            });
        }
    }

    private void MoveRight() {
        if (currentIdx < List.Count - 1) {
            currentIdx += 1;
            // _animator.SetFloat("Direction" , 1);
            _animator.SetBool("Idle" , false);
                
            transform.DOMoveX(List[currentIdx].x , lerpDuration).SetEase(Ease.InOutSine).OnComplete(() => {
                _animator.SetBool("Idle" , true);
            });
               
            // rotate in y direction
            meshTransform.DORotate(new Vector3(0, 45, 0), lerpDuration ).OnComplete(() => {
                meshTransform.DORotate(new Vector3(0, 0, 0), lerpDuration);
            });
        }
    }
    
    [FormerlySerializedAs("MinSwipeDistance")] [SerializeField] private float minSwipeDistance=50f;// In -> px
    [FormerlySerializedAs("MaxSwipeTime")] [SerializeField] private float maxSwipeTime=0.5f;// Max Time Requried to move 
    private float _swipeTime;// Total Swipe Time
  
    // Swipe Time

    private float _swipeEndTime;// Time at Swipe End
    private float _swipeStartTime;// Time at Swipe Start
    private float _swipeLength;// Lenght of Swipe
    // Swipe Pos

    private Vector2 _startSwipePos;// Swipe Start pos
    private Vector2 _endSwipePos;// End pos
    
    private void GetTouchInput()
    {
       
        if (Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0); // Take 1st Touch by User
            if(touch.phase==TouchPhase.Began)// Touch starts 
            {
                _swipeStartTime = Time.time;
                _startSwipePos = touch.position;
                // Get Time & Position of touch
            }
            else if(touch.phase==TouchPhase.Ended)// ?? Touch ended
            {
                _swipeEndTime = Time.time;
                _endSwipePos = touch.position;
                // Get Time & pos where Touch ended

                _swipeTime = _swipeEndTime - _swipeStartTime;//Check how long User Swipe
                _swipeLength = (_endSwipePos - _startSwipePos).magnitude;// Check Lenght
                
                if(_swipeTime >= maxSwipeTime && _swipeLength > minSwipeDistance)// Time & Distance 
                {
                    SwipeControl();
                }
            }
        }
        else {
            _animator.SetFloat("Direction" , 0);
        }
    }// getinput <-
    
    
    void SwipeControl() {
       
        Vector2 Distance = _endSwipePos - _startSwipePos;// get pos of Touch 
        float xDistance = Mathf.Abs(Distance.x);
        float yDistance = Mathf.Abs(Distance.y);
        // Abs return's Positive value

        // -> for x Movement
        if(xDistance>yDistance) {
            if(Distance.x>0)// Swipe Right
    {
                MoveRight();
            }
            if(Distance.x<0)// Swipe Left
            {
                MoveLeft();
            }       
        }

        // -> for Y Movement
        
        if(yDistance>xDistance)
        {
            if (characterController.isGrounded) {
                _animator.SetBool("Jump" , false);
                vVelocity = 0;
                if(Distance.y>0) // Swipe up
                    Jump();
            }
            if(Distance.y<0)// Swipe Down
            {
                // do noting
            }
        }
    }// <- Swipe control



    private void Jump() {
        transform.DOMoveY(jumpHeight, jumpTime);
        _animator.SetBool("Jump" , true);
        // vVelocity = jumpSpeed;
    }
    
    private void LateUpdate() =>
        _camera.gameObject.transform.position = new Vector3(_camera.transform.position.x , _camera.transform.position.y, transform.position.z - cameraOffSet);
    
}



/*
 * Swipe Control working : DONE
 * 
 * TODOS:
 * -> make code better
 * -> Add Jump
 * -> Improve Gravity
 * -> Add cine-machine 
 */

// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening;
//
// [RequireComponent(typeof (CharacterController))]
// public class PlayerController : MonoBehaviour {
//     private CharacterController characterController;
//     [SerializeField] private float speed = 6.0f;
//     [SerializeField] private Camera _camera;
//     [SerializeField] private List<Vector3> List;
//     [SerializeField] private float cameraOffSet = 3f;
//     private int currentIdx = 1;
//     [SerializeField] private float lerpSpeed = 0.5f;
//
//     [SerializeField] private Animator _animator;
//
//     [SerializeField] private float slideTime = 2f;
//     [SerializeField] private float gravity = 9.8f;
//     [SerializeField] private float jumpSpeed = 10f;
//     
//     private float vVelocity;
//     private void Awake() {  characterController = GetComponent<CharacterController>(); }
//     
//     private void Update() {
//
//         slideTime -= Time.deltaTime;
//         
//         if (slideTime > 0) {
//             _animator.SetBool("Go", true);
//         } else {
//             _animator.SetBool("Go", false);
//             _animator.SetBool("Idle" , true);
//         }
//        
//         if (characterController.isGrounded) {
//             _animator.SetBool("Jump" , false);
//             vVelocity = 0;
//             if (Input.GetKeyDown(KeyCode.Space)) {
//                 vVelocity = jumpSpeed;
//                 _animator.SetBool("Jump" , true);
//             }
//             
//         } 
//         
//         
//         
//         float z = 1f;
//         Vector3 move = transform.forward * z * speed * Time.deltaTime;
//         vVelocity -= gravity * Time.deltaTime;
//         move.y = vVelocity * Time.deltaTime;
//         Debug.Log(move);
//         characterController.Move(move);
//         
//         // move player left and right
//         if (Input.GetKeyDown(KeyCode.LeftArrow)) {
//             // move left
//             if (currentIdx > 0) {
//                 currentIdx -= 1;
//                 _animator.SetFloat("Direction" , -1);
//                 transform.DOMoveX(List[currentIdx].x , lerpSpeed).SetEase(Ease.InOutSine);
//               
//             }
//         }
//         else if (Input.GetKeyDown(KeyCode.RightArrow)) {
//             // move right
//             if (currentIdx < List.Count - 1) {
//                 currentIdx += 1;
//                 _animator.SetFloat("Direction" , 1);
//                 transform.DOMoveX(List[currentIdx].x , lerpSpeed).SetEase(Ease.InOutSine);
//                
//             }
//         }
//         else {
//             _animator.SetFloat("Direction" , 0);
//         }
//
//         // lerp to left
//         //transform.position = Vector3.Lerp(transform.position, new Vector3 (List[currentIdx].x , transform.position.y , transform.position.z ), Time.deltaTime * lerpSpeed);
//         
//         // move to x directions
//         
//         
//         // follow player
//         
//         
//         // Add Gravity
//     }
//
//
//     private void LateUpdate() {
//         _camera.gameObject.transform.position = new Vector3(_camera.transform.position.x , _camera.transform.position.y, transform.position.z - cameraOffSet);
//
//     }
// }
