using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.UI;
using UnityEngine;

namespace Masque
{

	public abstract class MaskEffect :  ScriptableObject
	{
		public abstract void ApplyEffect( TurnManager manager);
		public abstract void RemoveEffect(  );
		
	}
}