using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.UI;
using UnityEngine;

namespace Masque
{

	public abstract class MaskEffect :  ScriptableObject
	{
		public abstract void ApplyEffect(CardPlayerUI player);
		public abstract void RemoveEffect(CardPlayerUI player);
		
	}
}