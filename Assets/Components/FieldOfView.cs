using UnityEngine;

public class FieldOfView : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).

	public float viewRadius = 10;
	public float viewAngle = 70;

	public float meshResolution = 0.3f;

	public MeshFilter viewMeshFilter;
	public Mesh viewMesh;

	public int edgeResolveIteration = 7;
	public float edgeDistThreshold = 0.5f;

	public LayerMask targetMask;
	public LayerMask wallMask;

	/*void Awake(){
		viewMesh = new Mesh();
		viewMesh.name = "View of "+gameObject.name;
		viewMeshFilter.mesh = viewMesh;

		targetMask = LayerMask.GetMask("Player");
		wallMask = LayerMask.GetMask("Wall");
	}*/
}