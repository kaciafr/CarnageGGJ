using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using UnityEngine;

namespace Masque.Effect
{
	[CreateAssetMenu (menuName = "Effect/Vertigo")]
	public class Vertigo : MaskEffect
	{
		
		public override void ApplyEffect(UtilitiesCards cards)
		{
			cards.attackSize = 2;
			
			cards.defenseSize = 3;
		}
		public override void RemoveEffect(UtilitiesCards cards)
		{
		}
		
	}
}