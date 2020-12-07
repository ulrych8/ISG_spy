using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour,  IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    public bool isChildOfSlotPanel = true; 

    public PointerEventData mouseData;

    public bool draggable = true;
    //public bool isPointerDown = false;
    public bool isDragging = false;
    //public bool isPointerUp = true;

    public Vector3 fixedPosition;

    public Transform originalTransform;

    public bool removeButtonClicked = false;

    void Start(){
    	fixedPosition = transform.position;
    	originalTransform = transform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    	//isPointerDown = true;
    	//isPointerUp = false;
        fixedPosition = ((RectTransform)transform).position;			//transform au lieu de currentTransform
        //totalChild = mainContent.transform.childCount;
	    /*if (!isChildOfCodePanel){
	        	Instantiate(this.gameObject,this.transform);
	    }*/
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
