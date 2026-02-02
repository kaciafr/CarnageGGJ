using System.Collections;

namespace Gameplay.CardSystem.Turns
{
    public struct CardPlayerTurn
    {
        public readonly CardPlayer CardPlayer;

        
        public CardPlayerTurn(CardPlayer player)
        {
            CardPlayer = player;
        }


        public IEnumerator PlayTurn(TurnManager manager)
        {
            //1 : Pioche
            yield return CardPlayer.DrawPlayerCards(manager);
            //2 : Assignation des cartes (attaque ou defense)
            yield return CardPlayer.PlayCards(manager);
            //3 : Fin
            yield return CardPlayer.EndTurn(manager);
        }
    }
}