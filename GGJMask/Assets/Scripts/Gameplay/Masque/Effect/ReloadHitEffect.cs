using System;
using System.Linq;
using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Masque;
using UnityEngine;

[CreateAssetMenu(menuName = ("Effect/ReloadHitEffect"))]
public class ReloadHitEffect : MaskEffect
{
	public override void ApplyEffect(UtilitiesCards card)
	{
	}

	public override void RemoveEffect(UtilitiesCards cards)
	{
		
	}
	/*
		Debug.Log("ReloadHitEffect");

		int count = hand.MaxCards;
		var removeCard = hand.Cards.ToArray();
		
		
		if (hand.Cards.Count == 0)
		{
			Debug.Log("Hand is empty, adding test cards");
			for (int i = 0; i < hand.MaxCards; i++)
				hand.AddCard(deck.DrawCard());
		}
		
		for (int i = 0; i < count; i++)
		{
			var drawn = deck.DrawCard();
			if (drawn != null)
				hand.AddCard(drawn);
			Debug.Log("Removing: " + drawn);
		}*/
	
}
