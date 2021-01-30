using UnityEngine;
using UnityEngine.UI;
using FYFY;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections;


public class ControlSystem : FSystem {
	// Has to be after PatrolSystem in the main loop

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
				//guardAgent.SetDestination(infoGuard.patrolPoints[0]);
				infoGuard.patrolIndice += 1;		//authorize only one patrol point ?
				LineRenderer line = go.GetComponent<LineRenderer>();
				line.startWidth = 0.15f;
				line.endWidth = 0.15f;
				line.positionCount = 0;
				line.material = new Material(Shader.Find("Sprites/Default"));
				line.SetColors(Color.red,Color.white);
				DrawPath(infoGuard.patrolPoints.ToArray(), line);
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

			if (info.blocWaitForPlaying && !playerIsMoving){
				character.Move(Vector3.zero,false,false);
				if (info.waitTimeLeft>0){
					info.waitTimeLeft -= Time.deltaTime;
				}else{
					info.waitTimeLeft = 0;
					info.blocWaitForPlaying = false;
				}
			}else if (info.blocDistractInPlaying && !playerIsMoving){
				if (info.crouchingTime>0){
					character.Move(Vector3.zero,true,false);		//true is for crouching
					info.crouchingTime -= Time.deltaTime;
				}else{
					info.crouchingTime = 0f;
					character.Move(Vector3.zero,false,false);		//true is for crouching
					info.blocDistractInPlaying = false;
					GameObject clock = GameObject.Instantiate(info.clockPrefab, character.transform.position, Quaternion.Euler(-90,0,0));
					MainLoop.instance.StartCoroutine(Ring(clock, info.ringTime, info.ringParticle));
				}
			}
			else if (info.blocMoveToPlaying && !playerIsMoving){
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
					//guardAgent.CalculatePath(infoGuard.patrolPoints[infoGuard.patrolIndice], pathToDraw);
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

	void DrawPath(Vector3[] points, LineRenderer line){
	    line.positionCount = 0; 
		//line.SetPosition(0, new Vector3(points[0].x,0.6f,points[0].z));
	    float y = -0.3f;
	    int index = 0;
		for (int i=1; i<= points.Length; i++ ){
	    	NavMeshPath path = new NavMeshPath();
			NavMesh.CalculatePath(points[i-1],points[i%points.Length], NavMesh.AllAreas,path);
	    	line.positionCount += path.corners.Length;
	    	line.SetPosition(index, new Vector3(points[i-1].x, /*points[i].y*/y,points[i-1].z));
	    	index++;
			for(var j = 1; j < path.corners.Length; j++){
	    	    line.SetPosition(index, new Vector3(path.corners[j].x, /*path.corners[j].y*/y, path.corners[j].z)); //go through each corner and set that to the line renderer's position
	    	    index++;
	    	}
		}
    }

    public IEnumerator Ring(GameObject objectToDestroy, float timeToDestroy, ParticleSystem particle){
    	Vector3 pos = objectToDestroy.transform.position;
    	Quaternion rot = objectToDestroy.transform.rotation;
    	Object.Destroy(objectToDestroy, timeToDestroy+particle.main.duration);
    	float timer = timeToDestroy;
    	for (int i = 0; i<10*timer;i++){
    		yield return new WaitForSeconds(.1f);
    	}
    	GameObject.Instantiate(particle,pos, rot);

    	
    }
}
