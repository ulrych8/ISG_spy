using UnityEngine;
using FYFY;
using UnityEngine.AI;

public class ControlPlayerSystem : FSystem {

	private Family _controlableGO = FamilyManager.getFamily(new AllOfComponents(typeof(Controlable)));

	public NavMeshAgent agent;

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

			foreach (GameObject go in _controlableGO){
				agent = go.GetComponent<NavMeshAgent>();
			}

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			//if ( Physics.Raycast(ray, out hit,1000) ) 
			if ( Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit,Mathf.Infinity,0) ) 
			{
				agent.SetDestination(hit.point);
				Debug.Log("destination defined");
			}else{
				Debug.Log("nothing ma men "+hit.point);
			}
			Debug.DrawRay(Camera.main.transform.position,Camera.main.transform.forward, Color.red);
			Debug.DrawRay(new Vector3(1f,1f,1f), Vector3.up, Color.blue);
			Debug.Log("camera position etc. :  "+Camera.main.transform.position+" and as well"+Camera.main.transform.forward);

		}
	}
}