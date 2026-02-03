using System.Collections;

namespace Gameplay.CardSystem
{
    public class AICardPlayer : CardPlayer
    {
        public override void BeginTurn(TurnManager manager)
        {
            base.BeginTurn(manager);
            for (int i = 0; i < manager.Metrics.AttackSize; i++)
            {
                var card = MainHand.GetCard(0);
                if(card != null)
                    card.Transfer(MainHand, HandAttack);
            }

            for (int i = 0; i < manager.Metrics.DefenseSize; i++)
            {
                
                var card = MainHand.GetCard(0);
                if(card != null)
                    card.Transfer(MainHand, HandDefense);
            }
            
            SetIsDone();
        }
    }
}