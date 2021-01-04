using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FYFY;
using UnityEditor;

public class ViewSystem : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

	private Family viewer = FamilyManager.getFamily(new AllOfComponents(typeof(FieldOfView)));

	public struct ViewCastInfo{
		public bool hit;
		public Vector3 point;
		public float dist;
		public float angle;

		public ViewCastInfo( bool _hit, Vector3 _point, float _dist, float _angle){
			hit = _hit;
			point = _point;
			dist = _dist;
			angle = _angle;
		}
	}

	public ViewSystem(){
		foreach (GameObject go in viewer){
			FieldOfView viewInfo = go.GetComponent<FieldOfView>();
			viewInfo.viewMesh = new Mesh();
			viewInfo.viewMesh.name = "View of "+go.name;
			viewInfo.viewMeshFilter.mesh = viewInfo.viewMesh;

			viewInfo.targetMask = LayerMask.GetMask("Player");
			viewInfo.wallMask = LayerMask.GetMask("Wall");
		}
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		foreach (GameObject go in viewer){
			FieldOfView viewInfo = go.GetComponent<FieldOfView>();

			Transform currentTransform = go.transform;
			float viewRadius = viewInfo.viewRadius; 
			float viewAngle = viewInfo.viewAngle;
			float meshResolution = viewInfo.meshResolution; 
			LayerMask wallMask = viewInfo.wallMask; 
			Mesh viewMesh = viewInfo.viewMesh; 
			int edgeResolveIteration = viewInfo.edgeResolveIteration;
			float edgeDistThreshold = viewInfo.edgeDistThreshold;

			Vector3 viewAngleA = DirFromAngle(-viewInfo.viewAngle/2, false, go.transform);
			Vector3 viewAngleB = DirFromAngle(viewInfo.viewAngle/2, false, go.transform);

			Debug.DrawLine(go.transform.position, go.transform.position + viewAngleA*viewInfo.viewRadius);
			Debug.DrawLine(go.transform.position, go.transform.position + viewAngleB*viewInfo.viewRadius);

			//DrawFieldOfView(go.transform, viewInfo.viewRadius, viewInfo.viewAngle, viewInfo.meshResolution, viewInfo.wallMask, viewInfo.viewMesh, viewInfo.edgeResolveIteration);
			//---Draw field of view

			int stepCount = Mathf.RoundToInt(viewAngle*meshResolution);
			float stepAngleSize = viewAngle/stepCount;
			List<Vector3> viewPoints = new List<Vector3>();
			ViewCastInfo oldViewCast = new ViewCastInfo();

			for (int i = 0; i<= stepCount; i++){
				float angle = currentTransform.eulerAngles.y - viewAngle/2 + stepAngleSize*i;
				Debug.DrawLine(currentTransform.position, currentTransform.position + DirFromAngle(angle,true,currentTransform)*viewRadius);
				ViewCastInfo newViewCast = ViewCast(currentTransform, angle, viewRadius, wallMask);
				//hangle corner obstacle in view
				if (i>0){
					bool edgeDistThresholdExceeded = Mathf.Abs(oldViewCast.dist - newViewCast.dist) > edgeDistThreshold;
					if (oldViewCast.hit != newViewCast.hit ||  (oldViewCast.hit && newViewCast.hit && edgeDistThresholdExceeded)){
						EdgeInfo edge = FindEdge( oldViewCast, newViewCast, currentTransform, viewRadius, wallMask, edgeResolveIteration, edgeDistThreshold);
						if (edge.pointA != Vector3.zero){
							viewPoints.Add(edge.pointA);
						}
						if (edge.pointB != Vector3.zero){
							viewPoints.Add(edge.pointB);
						}
					}
				}

				viewPoints.Add(newViewCast.point);
				oldViewCast = newViewCast;
			}

			int vertexCount = viewPoints.Count+1;
			Vector3[] vertices = new Vector3[vertexCount];
			int[] triangles = new int[(vertexCount-2)*3];

			vertices[0] = Vector3.zero;
			for (int i = 0; i<vertexCount-1;i++ ){
				vertices[i+1] = currentTransform.InverseTransformPoint(viewPoints[i]);

				if (i<vertexCount-2){
					triangles[i*3] = 0;
					triangles[i*3+1] = i+1;
					triangles[i*3+2] = i+2;
				}
			}

			viewMesh.Clear();
			viewMesh.vertices = vertices;
			viewMesh.triangles = triangles;
			viewMesh.RecalculateNormals();

			//---
			FindVisibleTargets(go.transform, viewInfo.viewRadius, viewInfo.viewAngle, viewInfo.targetMask, viewInfo.wallMask);
		}
		
	}

	public void FindVisibleTargets(Transform currentTransform, float viewRadius, float viewAngle, LayerMask targetMask, LayerMask wallMask){
		Collider[] targetsInViewRadius = Physics.OverlapSphere (currentTransform.position, viewRadius, targetMask);

		for (int i=0; i < targetsInViewRadius.Length; i++){
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - currentTransform.position).normalized;
			if (Vector3.Angle(currentTransform.forward, dirToTarget)< viewAngle / 2){
				float distToTarget = Vector3.Distance(currentTransform.position, target.position);

				if (!Physics.Raycast(currentTransform.position,dirToTarget,distToTarget,wallMask)){
					//we see the tarfet
					//Debug.Log("I see the target");
					Debug.DrawLine(currentTransform.position, target.position);
				}
			}	
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal, Transform trsf){
		if (!angleIsGlobal){
			angleInDegrees += trsf.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	/*public void DrawFieldOfView(Transform currentTransform, float viewRadius, float viewAngle, float meshResolution, LayerMask wallMask, Mesh viewMesh, int edgeResolveIteration){
		int stepCount = Mathf.RoundToInt(viewAngle*meshResolution);
		float stepAngleSize = viewAngle/stepCount;
		List<Vector3> viewPoints = new List<Vector3>();
		ViewCastInfo oldViewCast = new ViewCastInfo();

		for (int i = 0; i<= stepCount; i++){
			float angle = currentTransform.eulerAngles.y - viewAngle/2 + stepAngleSize*i;
			Debug.DrawLine(currentTransform.position, currentTransform.position + DirFromAngle(angle,true,currentTransform)*viewRadius);
			ViewCastInfo newViewCast = ViewCast(currentTransform, angle, viewRadius, wallMask);
			//hangle corner obstacle in view
			if (i>0){
				bool 
				if (oldViewCast.hit != newViewCast.hit){
					EdgeInfo edge = FindEdge( oldViewCast, newViewCast, currentTransform, viewRadius, wallMask, edgeResolveIteration);
					if (edge.pointA != Vector3.zero){
						viewPoints.Add(edge.pointA);
					}
					if (edge.pointB != Vector3.zero){
						viewPoints.Add(edge.pointB);
					}
				}
			}

			viewPoints.Add(newViewCast.point);
			oldViewCast = newViewCast;
		}

		int vertexCount = viewPoints.Count+1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount-2)*3];

		vertices[0] = Vector3.zero;
		for (int i = 0; i<vertexCount-1;i++ ){
			vertices[i+1] = currentTransform.InverseTransformPoint(viewPoints[i]);

			if (i<vertexCount-2){
				triangles[i*3] = 0;
				triangles[i*3+1] = i+1;
				triangles[i*3+2] = i+2;
			}
		}

		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}*/

	public ViewCastInfo ViewCast(Transform trsf, float globalAngle, float viewRadius, LayerMask wallMask){
		Vector3 dir = DirFromAngle(globalAngle,true,trsf);
		RaycastHit hit;
		if (Physics.Raycast(trsf.position, dir, out hit, viewRadius, wallMask)){
			return new ViewCastInfo( true, hit.point, hit.distance, globalAngle);
		}
		else{
			return new ViewCastInfo( false, trsf.position + dir*viewRadius, viewRadius, globalAngle);
		}
	}

	public struct EdgeInfo{
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pA, Vector3 _pB){
			pointA = _pA;
			pointB = _pB;
		}

	}

	public EdgeInfo FindEdge( ViewCastInfo minViewCast, ViewCastInfo maxViewCast, Transform trsf, float viewRadius, LayerMask wallMask, int edgeResolveIteration, float edgeDistThreshold){
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i<edgeResolveIteration; i++){
			float angle = (minAngle + maxAngle ) /2;
			ViewCastInfo newViewCast = ViewCast(trsf, angle, viewRadius, wallMask);

			bool edgeDistThresholdExceeded = Mathf.Abs(minViewCast.dist - newViewCast.dist) > edgeDistThreshold;

			if (newViewCast.hit == minViewCast.hit && !edgeDistThresholdExceeded){
				minAngle  = angle;
				minPoint = newViewCast.point;
			}else{
				maxAngle = angle;
				maxPoint = newViewCast.point;
			}
		}

		return new EdgeInfo(minPoint, maxPoint);
	}
}