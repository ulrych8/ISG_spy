using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdownable : MonoBehaviour 
{
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotspot;

	public GameObject mapPointerPrefab;

	public char currentMoveToLetterInSystem = 'A';

	public Dropdown dropdown;

	void Awake(){
		Debug.Log("B-A is "+(int)('B'-'A'));
		dropdown = transform.GetComponent<Dropdown>();
		dropdown.options.Clear();
		List<string> optionList = new List<string>();
		//optionList.Add("0");
		optionList.Add("+");
		Debug.Log("currentMoveToLetterInSystem is "+currentMoveToLetterInSystem);
		for (var i='A'; i<currentMoveToLetterInSystem; i++){
			optionList.Add(""+i);
		}
		optionList.Add(" ");
		//fill dropdown options
		foreach (var option in optionList){
			dropdown.options.Add(new Dropdown.OptionData(){ text = option });
		}

		dropdown.onValueChanged.AddListener(delegate{ DropdownOptionSelected(dropdown); });

		dropdown.value = 1 + currentMoveToLetterInSystem - 'A';

		hotspot = new Vector2(cursorTexture.width/2,cursorTexture.height);
	}


	void DropdownOptionSelected(Dropdown dropdown){
		Debug.Log(" chose option "+dropdown.value);
		if (dropdown.value==0){
			Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
		}
	}

}