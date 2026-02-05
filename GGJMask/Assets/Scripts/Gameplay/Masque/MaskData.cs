using UnityEngine;
using UnityEngine.UI;

namespace Masque
{
	[CreateAssetMenu(menuName = "MaskData")]
	public class MaskData : ScriptableObject
	{
		public int ID;
		
		public string Name;
		
		public string Description;

		public Sprite Icon;
		
		public GameObject Prefab;
		
		public GameObject UI;
		
		public MaskEffect Effect;
		
	}
}