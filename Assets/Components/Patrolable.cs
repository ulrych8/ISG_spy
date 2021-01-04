using UnityEngine;
using System.Collections.Generic;

public class Patrolable : MonoBehaviour {
	public int patrolIndice = 0;

	public float waitingTime = 1f;

	public float guardWaiting = 1f;

	public List<Vector3> patrolPoints = new List<Vector3>() ;

	/*void Awake(){
		GameObject Patrol = GameObject.Find("PatrolPoints");
		foreach (Transform child in Patrol.transform)
		{
			patrolPoints.Add( child.position );
		}
		Debug.Log(patrolPoints);

	}*/
}