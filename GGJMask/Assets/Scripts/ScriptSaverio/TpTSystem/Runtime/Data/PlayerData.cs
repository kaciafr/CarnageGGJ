using UnityEngine;

namespace RunTime.TpTSystem.Data
{
	[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
	public class PlayerData : ScriptableObject
	{ 
		public string Name;
		
		public float PV = 2;
		
		
	}
}