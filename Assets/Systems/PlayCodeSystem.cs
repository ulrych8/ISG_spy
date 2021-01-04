using UnityEngine;
using UnityEngine.UI;
using FYFY;

public class PlayCodeSystem : FSystem {

	private Family playableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Playable)));

	private bool playingCode = false;


	private GameObject currentSlot;

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	public void PressPlay(GameObject go){
		currentSlot = GameObject.Find("Canvas").transform.Find("CodePanel").Find("SlotPanel").GetChild(0).gameObject;
		go.GetComponent<Playable>().playButtonClicked = true;
		playingCode = true;
		Debug.Log("Button Play pressed");
	}	

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go in playableGO){
			Playable info = go.GetComponent<Playable>();

			/*if (info.playButtonClicked && !playingCode){
				GameObject firstSlot = GameObject.Find("Canvas").transform.Find("CodePanel").Find("SlotPanel").GetChild(0).gameObject;
				currentSlot = firstSlot;				
				string slotName = firstSlot.name.Split(' ')[0].Split('(')[0];
				Debug.Log("slot name is "+slotName);

				if (slotName=="MoveSlot"){
					Transform label = firstSlot.transform.Find("Dropdown").Find("Label");
					info.destinationName = label.GetComponent<Text>().text;
					
					info.blocMoveToPlaying = true;
					playingCode = true;
				}
				info.playButtonClicked = false;
			}else if ( playingCode && !info.blocMoveToPlaying){
				//Debug.Log("currentSlot.transform.GetSiblingIndex()  is  "+currentSlot.transform.GetSiblingIndex());
				int index = currentSlot.transform.GetSiblingIndex();
				currentSlot = currentSlot.transform.parent.GetChild(index+1).gameObject;
				string slotName = currentSlot.name.Split(' ')[0].Split('(')[0];
				Debug.Log("slotname = "+slotName);
				if (slotName=="EndSlot"){
					playingCode = false;
				}
				if (slotName=="MoveSlot"){
					Debug.Log("knowing new slot blocMoteTo is Playing : "+info.blocMoveToPlaying );
					Transform label = currentSlot.transform.Find("Dropdown").Find("Label");
					info.destinationName = label.GetComponent<Text>().text;
					
					info.blocMoveToPlaying = true;
					
					Debug.Log("now Playing : "+info.blocMoveToPlaying );

				}
			}*/
			bool slotPlaying = (info.blocMoveToPlaying || info.blocWaitForPlaying);
			//if no slot playing and code ongoing then get new slot
			if (!slotPlaying && playingCode) {
				//int index = currentSlot.transform.GetSiblingIndex();
				//currentSlot = currentSlot.transform.parent.GetChild(index+1).gameObject;
				string slotName = currentSlot.name.Split(' ')[0].Split('(')[0];
				Debug.Log("slotname = "+slotName);
				if (slotName=="EndSlot"){
					playingCode = false;
					break;
				}
				else if (slotName=="MoveSlot"){
					Debug.Log("knowing new slot blocMoteTo is Playing : "+info.blocMoveToPlaying );
					Transform label = currentSlot.transform.Find("Dropdown").Find("Label");
					info.destinationName = label.GetComponent<Text>().text;
					
					info.blocMoveToPlaying = true;
					
					Debug.Log("now Playing MoveTo");
				}else if (slotName=="WaitSlot"){
					info.blocWaitForPlaying = true;
					Transform input = currentSlot.transform.Find("InputField").Find("Text");
					//handle exception
					float time = float.Parse(input.GetComponent<Text>().text);
					//Invoke("waitFinish",time);
					Debug.Log(time);
					info.waitTimeLeft = time;
				}
				int index = currentSlot.transform.GetSiblingIndex();
				currentSlot = currentSlot.transform.parent.GetChild(index+1).gameObject;
			}

		}

	}

}