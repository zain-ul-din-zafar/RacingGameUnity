using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
   public bool isPressing {get; private set;}
   private void Awake () => isPressing = false;
   //@ overiders
   public void OnPointerDown(PointerEventData eventData)  => isPressing = true;
   public void OnPointerUp  (PointerEventData eventData)  => isPressing = false;
}


