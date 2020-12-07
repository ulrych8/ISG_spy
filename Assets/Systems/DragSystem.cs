using UnityEngine;
using FYFY;

public class DragSystem : FSystem {

	private Family _draggableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Draggable)));

	//public RectTransform slotTransform;
    //public bool isChildOfSlotPanel = true; //set to false

    private Transform mainContent;
    //private Vector3 currentPosition;

    private int totalChild;
    private const float extraSpace = 11f;
    private const int closeButtonIndex = 1;
    private const int dropDownIndex = 3;

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

    public DragSystem(){
    	mainContent = GameObject.Find("Canvas").transform.Find("CodePanel").Find("SlotPanel");
    	totalChild = mainContent.childCount;
    }

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	public void removeOnClick(GameObject go){
        go.GetComponent<Draggable>().removeButtonClicked = true;
        Debug.Log("remove button clicked");
    }


	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject slot in _draggableGO){
			//agent = go.GetComponent<NavMeshAgent>();
			Draggable slotInfo = slot.GetComponent<Draggable>();

			RectTransform slotTransform = (RectTransform)slot.transform;


			if (slotInfo.isDragging){
				//change on x axis allowed
				slotTransform.position = new Vector3(slotInfo.mouseData.position.x, slotInfo.mouseData.position.y, slotTransform.position.z);

				if (!slotInfo.isChildOfSlotPanel){
		        	int distance = (int) Vector3.Distance(slotTransform.position,
		                    mainContent.position/*+Vector3.left*((RectTransform)mainContent).rect.width/2*/ );
		        	//Debug.Log("test distance is : "+distance);
		        	if (distance < 50){
		        		//if near slotPanel give a place above EndSlot
		        		Transform endSlot = mainContent.GetChild(totalChild-1);

		        		slot.transform.SetParent(mainContent);
		        		slotInfo.fixedPosition = endSlot.position;
		        		slotTransform.SetSiblingIndex(endSlot.GetSiblingIndex());
		        		endSlot.position = endSlot.position + Vector3.down*( slotTransform.rect.height - extraSpace );

		        		slotInfo.isChildOfSlotPanel = true;
		        		totalChild++;
		        		//able close button
		        		slotTransform.GetChild(closeButtonIndex).gameObject.SetActive(true);
		        		//if moveSlot able dropDown
		        		if (slot.name.Split(' ')[0].Split('(')[0] == "MoveSlot"){
		        			slotTransform.GetChild(dropDownIndex).gameObject.SetActive(true);
		        		}

		        		//duplication
		        		GameObject originalSlot = GameObject.Find("Canvas").transform.Find("SelectionPanel").Find(slot.name.Split(' ')[0].Split('(')[0]).gameObject;
		        		GameObject clone = UnityEngine.Object.Instantiate(originalSlot, originalSlot.transform.parent.transform, true); 
		        		FYFY.GameObjectManager.bind(clone);
		        	}

		        }else	//only if child of panel
		        {
			        for (int i = 0; i < totalChild-1; i++)			//minus 1 is to avoid picking the endSlot
			        {
			            if (i != slotTransform.GetSiblingIndex())
			            {
			                Transform otherTransform = mainContent.GetChild(i);
			                Draggable otherInfo = otherTransform.gameObject.GetComponent<Draggable>();

			                //int distance = (int) Vector3.Distance(slotTransform.position,otherInfo.fixedPosition);
			                int distance = (int) Mathf.Abs(slotTransform.position.y - otherInfo.fixedPosition.y);

			                if (distance <= 10)
			                {
			                    Vector3 otherOldPosition = otherInfo.fixedPosition;
			                    otherInfo.fixedPosition = new Vector3(otherInfo.fixedPosition.x, slotInfo.fixedPosition.y,
			                        otherInfo.fixedPosition.z);
			                    //currentTransform.position = new Vector3(currentTransform.position.x, otherTransformOldPosition.y,
			                    //    currentTransform.position.z);
			                    slotTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
			                    //fixedPosition = currentTransform.position;
			                    slotInfo.fixedPosition = new Vector3(otherOldPosition.x, otherOldPosition.y,
			                        otherOldPosition.z);
			                }
			            }
			        }

			    }
        	}else //fin draggin
        	{
        		slotTransform.position = slotInfo.fixedPosition;
        	}
        	if (slotInfo.removeButtonClicked){
        		Debug.Log("button clicked and I know it");
        		//update 
        		Vector3 previousPosition = slotInfo.fixedPosition;
        		Vector3 tempPosition;

        		for (int i = slotTransform.GetSiblingIndex()+1; i < totalChild-1; i++)			//minus 1 is to avoid picking the endSlot
		        {
		            Transform otherTransform = mainContent.GetChild(i);
		            Draggable otherInfo = otherTransform.gameObject.GetComponent<Draggable>();
		            tempPosition = otherInfo.fixedPosition;
		            otherInfo.fixedPosition = previousPosition;
		            previousPosition = tempPosition;
		        }

        		Transform endSlot = mainContent.GetChild(totalChild-1);

        		endSlot.position = previousPosition;

        		FYFY.GameObjectManager.unbind(slot);
    			UnityEngine.Object.Destroy(slot);
    			totalChild--;
        	}

        }

			
		
	}
}