using UnityEngine;
using UnityEngine.UI;
using FYFY;

public class DropdownSystem : FSystem {

	private Family dropdownGO = FamilyManager.getFamily(new AllOfComponents(typeof(Dropdownable)));

	//prefab use

	private char currentMoveToLetter = 'A';

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		if (Input.GetMouseButtonDown(0))
		{
			foreach (GameObject dd in dropdownGO){

				Dropdownable ddInfo = dd.GetComponent<Dropdownable>();

				if (!dd.active || ddInfo.dropdown.value!=0) continue;

				Vector3 mousePosition = Input.mousePosition;
				Ray ray = Camera.main.ScreenPointToRay(mousePosition);
				RaycastHit hit;

				if(Physics.Raycast(ray, out hit) == true) {
					Vector3 pointPosition = new Vector3(hit.point.x, ddInfo.mapPointerPrefab.transform.position.y, hit.point.z);
					GameObject newMapPointer = Object.Instantiate(ddInfo.mapPointerPrefab, pointPosition, ddInfo.mapPointerPrefab.transform.rotation, GameObject.Find("Destinations").transform);
					FYFY.GameObjectManager.bind(newMapPointer);

					newMapPointer.transform.GetChild(1).GetComponent<Text>().text = ""+currentMoveToLetter;
					newMapPointer.GetComponent<Destination>().destination = hit.point;
					//add new option to every dropdown
					foreach (GameObject ddGO in dropdownGO){
						if (!ddGO.active) continue;
						Dropdownable ddGOInfo = ddGO.GetComponent<Dropdownable>();
						ddGOInfo.dropdown.options.Add(new Dropdown.OptionData(){ text = ""+currentMoveToLetter });
					}

					int newIndex = ddInfo.dropdown.options.FindIndex((i) => { return i.text.Equals(""+currentMoveToLetter); });
					Debug.Log("nex index is "+newIndex);
					ddInfo.dropdown.value = newIndex;

					Cursor.SetCursor(null, Vector2.zero, ddInfo.cursorMode);
					currentMoveToLetter++;
					//add new option to every dropdown
					foreach (GameObject ddGO in dropdownGO){
						Dropdownable ddGOInfo = ddGO.GetComponent<Dropdownable>();
						ddGOInfo.currentMoveToLetterInSystem = currentMoveToLetter;
					}
				}else{
					Debug.Log("click not on map");
					Cursor.SetCursor(null, Vector2.zero, ddInfo.cursorMode);
					ddInfo.dropdown.value = 1;
				}
			}
		}
	}
}