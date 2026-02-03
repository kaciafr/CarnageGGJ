using UnityEngine;

namespace Masque
{
	[CreateAssetMenu(menuName = "MaskData")]
	public class MaskData : ScriptableObject
	{
		public string Name;
		
		public GameObject Prefab;
		
		public GameObject UI;
		
		public MaskEffect Effect;
		
	}
}