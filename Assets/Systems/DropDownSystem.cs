using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FYFY;

public class DropdownSystem : FSystem {

	//famille d'objet utilisant un dropdown
	private Family dropdownGO = FamilyManager.getFamily(new AllOfComponents(typeof(Dropdownable)));

	
	private char currentMoveToLetter = 'A';

	public DropdownSystem(){
		dropdownGO.addEntryCallback(Callback2);

		//pour chaque gameobject de la famille on initialise le dropdown
		foreach (GameObject dd in dropdownGO){
			Dropdownable info = dd.GetComponent<Dropdownable>();
			Dropdown dropdown = dd.transform.GetComponent<Dropdown>();
			dropdown.options.Clear();
			List<string> optionList = new List<string>();
			optionList.Add("+");
			for (var i='A'; i<currentMoveToLetter; i++){
				optionList.Add(""+i);
			}
			optionList.Add(" ");
			//fill dropdown options
			foreach (var option in optionList){
				dropdown.options.Add(new Dropdown.OptionData(){ text = option });
			}
			//Je pourrais dans le onprocess regarder a chaque fois si la valeur change pour tous les membres de la famille
			//------------ AddListener
			dropdown.onValueChanged.AddListener(delegate{ DropdownOptionSelected(dropdown,info); });
			//------------

			dropdown.value = 1 + currentMoveToLetter - 'A';

			info.hotspot = new Vector2(info.cursorTexture.width/2,info.cursorTexture.height);
			info.dropdown = dropdown;
		}

	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		if (Input.GetMouseButtonDown(0))
		{
			foreach (GameObject dd in dropdownGO){

				Dropdownable ddInfo = dd.GetComponent<Dropdownable>();
				//si le dropdown et active et à l'option voulu lors du clic on ajoute la coordonnée au dropdown
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
						//if (!ddGO.active) continue;
						Dropdownable ddGOInfo = ddGO.GetComponent<Dropdownable>();
						ddGOInfo.dropdown.options.Add(new Dropdown.OptionData(){ text = ""+currentMoveToLetter });
					}

					int newIndex = ddInfo.dropdown.options.FindIndex((i) => { return i.text.Equals(""+currentMoveToLetter); });
					Debug.Log("nex index is "+newIndex);
					ddInfo.dropdown.value = newIndex;

					Cursor.SetCursor(null, Vector2.zero, ddInfo.cursorMode);
					currentMoveToLetter++;
					
				}else{
					Debug.Log("click not on map");
					Cursor.SetCursor(null, Vector2.zero, ddInfo.cursorMode);
					ddInfo.dropdown.value = 1;
				}
			}
		}
	}

	void DropdownOptionSelected(Dropdown dropdown, Dropdownable info){
		Debug.Log(" clikos !! chose option "+dropdown.value);
		if (dropdown.value==0){
			Cursor.SetCursor(info.cursorTexture, info.hotspot, info.cursorMode);
		}
	}

	void Callback2(GameObject go){
		Dropdownable info = go.GetComponent<Dropdownable>();
		Dropdown dropdown = go.transform.GetComponent<Dropdown>();
		dropdown.onValueChanged.AddListener(delegate{ DropdownOptionSelected(dropdown,info); });
	}
}