using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using UnityEngine;

namespace Masque
{

	public abstract class MaskEffect :  ScriptableObject
	{
		public abstract void ApplyEffect(UtilitiesCards  cards);
		public abstract void RemoveEffect(UtilitiesCards  cards);
		
	}
}