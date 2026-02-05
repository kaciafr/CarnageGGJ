using Gameplay;
using Gameplay.Player;
using UnityEngine;

public class ScriptEntermachine : MonoBehaviour
{
	[SerializeField] private SlotMachine slotMachine;
	public bool enter = false;
	public void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name);
		enter = true;
	}

	public void OnTriggerExit(Collider other)
	{
		enter = false;
		Debug.Log(other.gameObject.name);
	}
}
