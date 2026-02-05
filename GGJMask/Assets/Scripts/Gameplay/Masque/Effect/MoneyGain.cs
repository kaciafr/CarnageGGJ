using Masque;
using UnityEngine;

namespace Gameplay.Masque.Effect
{
    [CreateAssetMenu(menuName = "Effect/MoneyGain")]
    public class MoneyGain : MaskEffect
    {
        public override void ApplyEffect(TurnManager manager)
        {
            manager.moneyReward *= 2;
        }

        public override void RemoveEffect()
        {
            
        } 
    }
}
