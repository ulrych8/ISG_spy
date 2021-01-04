using UnityEngine;
using FYFY;

public class PatrolSystem : FSystem {
	// Has to be before ControlSystem in the main loop
	// Use this to update member variables when system pause. 
	

	private Family patroler = FamilyManager.getFamily(new AllOfComponents(typeof(Patrolable)));

	public PatrolSystem(){
		foreach (GameObject go in patroler){
			Patrolable infoPatrol = go.GetComponent<Patrolable>();
			GameObject Patrol = GameObject.Find("PatrolPoints");
			foreach (Transform child in Patrol.transform)
			{
				infoPatrol.patrolPoints.Add( child.position );
			}
		}
	}
}