using UnityEngine;
using System.Collections.Generic;

public class Patrolable : MonoBehaviour {
	public int patrolIndice = 0;

	public float waitingTime = 1f;

	public float guardWaiting = 1f;

	public List<Vector3> patrolPoints = new List<Vector3>() ;

	public bool stressMode = false;

}