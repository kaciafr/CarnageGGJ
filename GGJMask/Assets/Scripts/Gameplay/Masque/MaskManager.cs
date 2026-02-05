using System;
using Gameplay.CardSystem;
using Gameplay.CardSystem.UI;
using UnityEngine;

namespace Masque
{
	public class MaskManager : MonoBehaviour
	{
		public MaskData currentMask;
		private CardPlayerUI player;

		private void Awake()
		{
			player = GetComponent<CardPlayerUI>();

		}

		public void EquipMask()
		{
			if (currentMask != null)
				currentMask.Effect.RemoveEffect(player);
			
			currentMask.Effect.ApplyEffect(player);
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
