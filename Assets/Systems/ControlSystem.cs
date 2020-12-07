using UnityEngine;
using UnityEngine.UI;
using FYFY;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class ControlSystem : FSystem {

	private Family _controlableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Controlable)));

	private Family playableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Playable)));


	public NavMeshAgent agent;

	public ThirdPersonCharacter character;

	private bool playerIsMoving = false;

	public ControlSystem(){
		//only working because only one controlable
		foreach (GameObject go in _controlableGO){
			if ( go.GetComponent<Controlable>().controlable ){			//only player meet this condition
				agent = go.GetComponent<NavMeshAgent>();
				character = go.GetComponent<ThirdPersonCharacter>();
				agent.updateRotation = false;
			}else{
				NavMeshAgent guardAgent = go.GetComponent<NavMeshAgent>();
				guardAgent.updateRotation = false;	
				Patrolable infoGuard = go.GetComponent<Patrolable>();
				guardAgent.SetDestination(infoGuard.patrolPoints[0]);
				infoGuard.patrolIndice += 1;		//authorize only one patrol point ?
			}
		}

		
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

		foreach (GameObject go in _controlableGO){
			if (! go.GetComponent<Controlable>().controlable){	//if not player character
				NavMeshAgent guardAgent = go.GetComponent<NavMeshAgent>();
				ThirdPersonCharacter guardCharacter = go.GetComponent<ThirdPersonCharacter>();
				Patrolable infoGuard = go.GetComponent<Patrolable>();


				//animation part
				if (infoGuard.guardWaiting>0){
					guardCharacter.Move(Vector3.zero,false,false);
					infoGuard.guardWaiting -= Time.deltaTime;
				}else if (infoGuard.guardWaiting<0){
					infoGuard.guardWaiting = 0;
					//set new patrol point
					guardAgent.SetDestination(infoGuard.patrolPoints[infoGuard.patrolIndice]);
					infoGuard.patrolIndice = (infoGuard.patrolIndice+1)%infoGuard.patrolPoints.Count;
				}
				else if (guardAgent.remainingDistance > guardAgent.stoppingDistance+0.2f){
					guardCharacter.Move(guardAgent.desiredVelocity,false,false);
				}
				else{
					guardCharacter.Move(Vector3.zero,false,false);
					
					infoGuard.guardWaiting = infoGuard.waitingTime;
				}

			}
		}

		
	}

}