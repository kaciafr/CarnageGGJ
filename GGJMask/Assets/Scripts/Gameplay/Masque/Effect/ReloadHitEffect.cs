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
    
    private bool hasBeenUsed = false;

    public override void ApplyEffect(TurnManager manager)
    {
        if (hasBeenUsed)
        {
            Debug.Log("⚠️ Masque déjà utilisé cette partie !");
            return;
        }
        
        CardPlayer player = manager.CurrentTurnPlayer;
        
        player.TakeDamage(healthCost);
        
        player.MainHand.Clear();

        for (int i = 0; i < manager.Metrics.HandSize; i++)
        {
            ICard card = manager.Deck.DrawCard();
            player.MainHand.AddCard(card);
        }
        
        hasBeenUsed = true;
        
        Debug.Log($" Masque utilisé ! {player.name} perd {healthCost} PV (Plus disponible)");
    }

    public override void RemoveEffect()
    {
        hasBeenUsed = false;
    }
    
    public bool CanBeUsed() => !hasBeenUsed;
}