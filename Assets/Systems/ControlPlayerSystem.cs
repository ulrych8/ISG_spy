using UnityEngine;
using UnityEngine.UI;
using FYFY;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class ControlPlayerSystem : FSystem {

	private Family _controlableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Controlable)));

	private Family playableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Playable)));


	public NavMeshAgent agent;

	public ThirdPersonCharacter character;

	private bool playerIsMoving = false;

	public ControlPlayerSystem(){
		//only working because only one controlable
		foreach (GameObject go in _controlableGO){
			agent = go.GetComponent<NavMeshAgent>();
			character = go.GetComponent<ThirdPersonCharacter>();
		}

		agent.updateRotation = false;
	}

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
		foreach (GameObject go in playableGO){
			Playable info = go.GetComponent<Playable>();

			if (info.blocMoveToPlaying && !playerIsMoving){
				//get destination
				Debug.Log("Setting new destination ...");
				GameObject destinations = GameObject.Find("Destinations");
				foreach ( Transform child in destinations.transform ){
					if ( child.GetChild(1).GetComponent<Text>().text == info.destinationName){
						Destination coordinate = child.gameObject.GetComponent<Destination>();
						agent.SetDestination(coordinate.destination);
						playerIsMoving=true;
					}
				}
			}
			else if (playerIsMoving){
				if (agent.remainingDistance > agent.stoppingDistance+0.2f){
					character.Move(agent.desiredVelocity,false,false);
				}else{
					character.Move(Vector3.zero,false,false);
					playerIsMoving = false;
					info.blocMoveToPlaying = false;
				}
			}else if (! playerIsMoving){
				character.Move(Vector3.zero,false,false);
			}
		}
		/*
		if (Input.GetMouseButtonDown(0))
		{

			Vector3 mousePosition = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit) == true) {
				//Debug.Log("finaalllyyy");
				agent.SetDestination(hit.point);
			}else{
				Debug.Log("herre we go again");
			}
		}
		if (agent.remainingDistance > agent.stoppingDistance+0.2f){
			character.Move(agent.desiredVelocity,false,false);
		}else{
			character.Move(Vector3.zero,false,false);
		}*/
	}
}