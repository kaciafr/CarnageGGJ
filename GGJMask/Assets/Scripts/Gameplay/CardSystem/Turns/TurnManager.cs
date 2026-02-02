using System;
using System.Collections;
using Gameplay;
using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.Turns;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public event Action<CardPlayerTurn> OnCardPlayerTurnBegins;
    public event Action<CardPlayerTurn> OnCardPlayerTurnEnds;
    public event Action OnSetupPhase;
    public event Action OnPlayPhase;
    public event Action OnResolutionPhase;
    
    [SerializeField]
    private CardPlayer[] cardPlayers;

    [SerializeField] 
    private GameMetrics metrics;
    
    public CardPlayer CurrentTurnPlayer { get; private set; }
    public int CurrentTurn { get; private set; }
    public River River { get; private set; }
    
    public Deck Deck { get; private set; }
    
    private void Awake()
    {
        River = new River();
        Deck = new Deck();
    }

    private IEnumerator PlayCardGame()
    {
        //TODO : Apprendre les extensions a Kaci
        
        Deck.FillCollectionWithAllCards(metrics);
        Deck.Shuffle();
        
        for (int i = 0; i < cardPlayers.Length; i++)
            cardPlayers[i].PrepareForGame(this);
        int losingPlayer = -1;
        while (HasAnyPlayerLost(out losingPlayer))
        {
            CurrentTurn++;

            //River fill
            OnSetupPhase?.Invoke();
            
            River.Clear();
            for (int i = 0; i < metrics.RiverSize; i++)
            {
                ICard card = Deck.DrawCard();
                River.AddCard(card);
            }
            
            OnPlayPhase?.Invoke();
            //Player turns
            for (var i = 0; i < cardPlayers.Length; i++)
            {
                CurrentTurnPlayer = cardPlayers[i];
                CardPlayerTurn cardPlayerTurn = new CardPlayerTurn(CurrentTurnPlayer);

                OnCardPlayerTurnBegins?.Invoke(cardPlayerTurn);
                yield return cardPlayerTurn.PlayTurn(this);
                OnCardPlayerTurnEnds?.Invoke(cardPlayerTurn);
            }
            
            OnResolutionPhase?.Invoke();
            //Resolution
            for (int i = 0; i < cardPlayers.Length; i++)
            {
                CardPlayer player = cardPlayers[i];
                int damage = River.GetCollectionDamage(player.HandAttack, metrics);

                for (int j = 0; j < cardPlayers.Length; j++)
                {
                    //Ne pas se tuer soit meme
                    if (j != i)
                        cardPlayers[j].TakeDamage(damage);
                }
            }
        }
        
        Debug.Log($"Game Over for {losingPlayer}");
        //Des trucs
    }

    private bool HasAnyPlayerLost(out int losingPlayer)
    {
        for (int i = 0; i < cardPlayers.Length; i++)
        {
            if (cardPlayers[i].CurrentHealth <= 0)
            {
                losingPlayer = i;
                return true;
            }
        }
        losingPlayer = -1;
        return false;
    }
    
    
}
