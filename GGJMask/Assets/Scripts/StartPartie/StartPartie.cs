using UnityEngine;

public class StartPartie : MonoBehaviour
{
	public bool canStartPartie = false;
	public void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name);
		canStartPartie = true;
	}

	public void OnTriggerExit(Collider other)
	{
		canStartPartie = false;
		Debug.Log(other.gameObject.name);
	}
}
