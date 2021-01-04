using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour,  IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    public bool isChildOfSlotPanel = true; 

    public PointerEventData mouseData;

    public bool draggable = true;

    public bool isDragging = false;

    public Vector3 fixedPosition;//= transform.position;


    public bool removeButtonClicked = false;

    /*void Start(){
    	fixedPosition = transform.position;
    }*/

    public void OnPointerDown(PointerEventData eventData)
    {
        fixedPosition = ((RectTransform)transform).position;
    }

    public void OnDrag(PointerEventData eventData)
    {
    	isDragging = true;
    	mouseData = eventData;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

}
