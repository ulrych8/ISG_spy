using UnityEngine;

public class Playable : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public bool playButtonClicked = false;

	public bool blocMoveToPlaying = false; 
	public string destinationName;

	
}