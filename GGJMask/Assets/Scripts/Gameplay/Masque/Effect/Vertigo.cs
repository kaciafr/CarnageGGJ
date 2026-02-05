using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.UI;
using UnityEngine;

namespace Masque.Effect
{
	[CreateAssetMenu (menuName = "Effect/Vertigo")]
	public class Vertigo : MaskEffect
	{
		public override void ApplyEffect(CardPlayerUI player )
		{
			player.cardMaxSelected = 2;
			Debug.Log(player.cardMaxSelected);
		}
		public override void RemoveEffect(CardPlayerUI player )
		{
			
		}
	}
}