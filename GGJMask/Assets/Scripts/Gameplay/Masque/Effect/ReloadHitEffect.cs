using System;
using System.Linq;
using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Masque;
using UnityEngine;

[CreateAssetMenu(menuName = ("Effect/ReloadHitEffect"))]
public class ReloadHitEffect : MaskEffect
{
    public int healthCost = 5; 

    public override void ApplyEffect(TurnManager manager)
    {
        CardPlayer player = manager.CurrentTurnPlayer;
        
        player.TakeDamage(healthCost);
        
        player.MainHand.Clear();

        for (int i = 0; i < manager.Metrics.HandSize; i++)
        {
            ICard card = manager.Deck.DrawCard();
            player.MainHand.AddCard(card);
        }
        
        Debug.Log($"Masque utilisé ! {player.name} perd {healthCost} PV et recharge sa main");
    }

    public override void RemoveEffect()
    {
        Debug.Log("ReloadHitEffect n'a pas d'effet permanent à retirer");
    }
}