using System;
using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.UI;
using UnityEngine;

namespace Masque
{
	public class MaskManager : MonoBehaviour
	{
		public MaskData currentMask;
		
		public TurnManager turnManager;
		
		public Hand hand { get; private set; }
		public Deck deck { get; private set; }
		private void Awake()
		{

		}

		public void EquipMask(MaskData mask)
		{
			if (currentMask != null)
				currentMask.Effect.RemoveEffect();
			

			currentMask = mask;
			mask.Effect.ApplyEffect(turnManager);
		}

		public void UnequipMask()
		{
			if (currentMask == null)
				return;

			currentMask.Effect.RemoveEffect();
			currentMask = null;
		}
	}
}
