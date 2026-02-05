using System;
using Gameplay.CardSystem;
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
			player = GetComponent<CardPlayerUI>();

		}

		public void EquipMask()
		{
			if (currentMask != null)
				currentMask.Effect.RemoveEffect(player);
			

			currentMask = mask;
			//mask.Effect.ApplyPlayerEffect(hand, deck, null);
			mask.Effect.ApplyEffect(turnManager);
		}

		public void UnequipMask()
		{
			if (currentMask == null)
				return;

			currentMask.Effect.RemoveEffect(player);
			currentMask = null;
		}
	}
}
