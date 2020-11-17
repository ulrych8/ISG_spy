using UnityEngine;
using FYFY;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class ControlPlayerSystem : FSystem {

	private Family _controlableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Controlable)));

	public NavMeshAgent agent;

	public ThirdPersonCharacter character;

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
		if (Input.GetMouseButtonDown(0))
		{

			Vector3 mousePosition = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit) == true) {
				Debug.Log("finaalllyyy");
				agent.SetDestination(hit.point);
			}else{
				Debug.Log("herre we go again");
			}
		}
		if (agent.remainingDistance > agent.stoppingDistance+0.2f){
			character.Move(agent.desiredVelocity,false,false);
		}else{
			character.Move(Vector3.zero,false,false);
		}
	}
}