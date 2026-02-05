using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using UnityEngine;

namespace Masque
{

	public abstract class MaskEffect :  ScriptableObject
	{
		public abstract void ApplyEffect( TurnManager manager);
		public abstract void RemoveEffect(  );
		
	}
}