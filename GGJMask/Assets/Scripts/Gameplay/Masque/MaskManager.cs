using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Masque
{
	public class MaskManager : MonoBehaviour
	{
		public MaskData currentMask;
		public MaskData newMask;
		
		public Hand hand { get; private set; }
		public Deck deck { get; private set; }
		private void Awake()
		{
			hand = new Hand(5);
			deck = new Deck();
		}
		public void EquipMask(MaskData mask)
		{
			if(currentMask != null)
				currentMask.Effect.RemoveEffect();
			

			currentMask = mask;
			//mask.Effect.ApplyPlayerEffect(hand, deck, null);
			mask.Effect.ApplyEffect();
		}

		public void UnequipMask(MaskData mask)
		{
			if (currentMask == null)
				currentMask.Effect.RemoveEffect();
		}
	}
}