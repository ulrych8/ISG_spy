using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownHandling : MonoBehaviour
{
	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotspot;

	public GameObject mapPointer;

	private int currentOption;
	private char currentMoveToLetter = 'A';

	private Dropdown dropdown;
    // Start is called before the first frame update
	void Start(){
		dropdown = transform.GetComponent<Dropdown>();
		dropdown.options.Clear();
		List<string> optionList = new List<string>();
		//optionList.Add("0");
		optionList.Add("+");
		optionList.Add(" ");
		//fill dropdown options
		foreach (var option in optionList){
			dropdown.options.Add(new Dropdown.OptionData(){ text = option });
		}

		dropdown.onValueChanged.AddListener(delegate{ DropdownOptionSelected(dropdown); });

		dropdown.value = 1;
		currentOption = 1; 

		hotspot = new Vector2(cursorTexture.width/2,cursorTexture.height);
	}

	void DropdownOptionSelected(Dropdown dropdown){
		int index = dropdown.value;
		Debug.Log(" chose option "+index);
		if (index==0){
			Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
			currentOption = 0;

		}
	}

	void Update(){
		if (Input.GetMouseButtonDown(0) && currentOption==0)
		{

			Vector3 mousePosition = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit) == true) {
				Vector3 pointPosition = new Vector3(hit.point.x, mapPointer.transform.position.y, hit.point.z);
				GameObject newMapPointer = Instantiate(mapPointer, pointPosition, mapPointer.transform.rotation);

				newMapPointer.transform.GetChild(1).GetComponent<Text>().text = ""+currentMoveToLetter;

				dropdown.options.Add(new Dropdown.OptionData(){ text = ""+currentMoveToLetter });
				int newIndex = dropdown.options.FindIndex((i) => { return i.text.Equals(""+currentMoveToLetter); });
				Debug.Log("nex index is "+newIndex);
				currentOption = newIndex;
				dropdown.value = newIndex;

				Cursor.SetCursor(null, Vector2.zero, cursorMode);
				currentMoveToLetter++;
			}else{
				Debug.Log("click not on map");
				Cursor.SetCursor(null, Vector2.zero, cursorMode);
				dropdown.value = 1;
				currentOption = 1;
			}
		}
	}

    
}

/*
using UnityEngine;
using UnityEngine.UI;

public class Dropdownable : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotspot;

	public GameObject mapPointer;

	public int currentOption;

	public Dropdown dropdown;

	public Dropdownable(){
		dropdown = transform.GetComponent<Dropdown>();
		dropdown.options.Clear();
		List<string> optionList = new List<string>();
		//optionList.Add("0");
		optionList.Add("+");
		optionList.Add(" ");
		//fill dropdown options
		foreach (var option in optionList){
			dropdown.options.Add(new Dropdown.OptionData(){ text = option });
		}

		dropdown.onValueChanged.AddListener(delegate{ DropdownOptionSelected(dropdown); });

		dropdown.value = 1;
		currentOption = 1;

		hotspot = new Vector2(cursorTexture.width/2,cursorTexture.height);
	}


	void DropdownOptionSelected(Dropdown dropdown){
		Debug.Log(" chose option "+index);
		if (index==0){
			Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
		}
	}

}*/