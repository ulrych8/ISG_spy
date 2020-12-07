using UnityEngine;
using FYFY;

[ExecuteInEditMode]
public class DragSystem_wrapper : MonoBehaviour
{
	private void Start()
	{
		this.hideFlags = HideFlags.HideInInspector; // Hide this component in Inspector
	}

	public void removeOnClick(UnityEngine.GameObject go)
	{
		MainLoop.callAppropriateSystemMethod ("DragSystem", "removeOnClick", go);
	}

}