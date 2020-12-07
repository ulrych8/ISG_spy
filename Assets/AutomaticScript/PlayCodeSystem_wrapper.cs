using UnityEngine;
using FYFY;

[ExecuteInEditMode]
public class PlayCodeSystem_wrapper : MonoBehaviour
{
	private void Start()
	{
		this.hideFlags = HideFlags.HideInInspector; // Hide this component in Inspector
	}

	public void PressPlay(UnityEngine.GameObject go)
	{
		MainLoop.callAppropriateSystemMethod ("PlayCodeSystem", "PressPlay", go);
	}

}