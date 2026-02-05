using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Masque;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/RiverChangeEffect")]
public class RiverChangeEffect : MaskEffect
{
    private int healthCost = 5;
    public override void ApplyEffect(TurnManager manager)
    {
        manager.River.RemoveCard(manager.River.Cards[0]);
        manager.River.RemoveCard(manager.River.Cards[0]);
        manager.River.AddCard(manager.Deck.DrawCard());
        manager.River.AddCard(manager.Deck.DrawCard());
    }

    public override void RemoveEffect()
    {
    }

}