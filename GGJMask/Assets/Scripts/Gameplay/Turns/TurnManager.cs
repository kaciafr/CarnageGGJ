using System;
using System.Collections;
using Gameplay;
using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public event Action OnSetupPhase;
    public event Action OnPlayPhase;
    public event Action OnResolutionPhase;
    public event Action<CardPlayer> OnPlayerWon;
    public event Action OnTurnChanged;
    public event Action<CardPlayer> OnGameOver; 
    public event Action OnGameStarted;
    public event Action OnDesactivateCanvas; 
    
    public event Action OnComboChanged;

    public CardPlayer[] Players => cardPlayers;
    public CardPlayer CurrentTurnPlayer { get; private set; }
    public int CurrentTurn { get; private set; }
    public River River { get; private set; }
    public Deck Deck { get; private set; }

    [SerializeField] private CardPlayer[] cardPlayers;

    [field: SerializeField] 
    public GameMetrics Metrics { get; private set; }

    [Header("Game Over Settings")]
    [SerializeField] private int moneyReward = 500;
    [SerializeField] private float delayBeforeRetry = 2f;

    private Coroutine gameCoroutine;

    private void Awake()
    {
        River = new River();
        Deck = new Deck();
    }

    private void Start()
    {
        gameCoroutine = StartCoroutine(PlayCardGame());
    }

    private IEnumerator PlayCardGame()
    {
        OnGameStarted?.Invoke();
        Deck.FillCollectionWithAllCards(Metrics);
        Deck.Shuffle();

        for (int i = 0; i < cardPlayers.Length; i++)
            cardPlayers[i].PrepareForGame(this);

        int losingPlayer = -1;
        yield return new WaitForSeconds(1);

        while (!HasAnyPlayerLost(out losingPlayer))
        { 
            CurrentTurn++;
            OnTurnChanged?.Invoke();

            OnSetupPhase?.Invoke();
            RefreshDeck();
            River.Clear();
            for (int i = 0; i < Metrics.RiverSize; i++)
            {
                ICard card = Deck.DrawCard();
                River.AddCard(card);
            }

            OnPlayPhase?.Invoke();
            for (var i = 0; i < cardPlayers.Length; i++)
                cardPlayers[i].PrepareTurn(this);

            for (var i = 0; i < cardPlayers.Length; i++)
            {
                CurrentTurnPlayer = cardPlayers[i];
                CurrentTurnPlayer.BeginTurn(this);

                while (!CurrentTurnPlayer.IsDone)
                    yield return null;

                CurrentTurnPlayer.EndTurn(this);
                yield return new WaitForSeconds(.5f);
            }

            yield return new WaitForSeconds(1.5f);
            OnResolutionPhase?.Invoke();

            for (int i = 0; i < cardPlayers.Length; i++)
            {
                CardPlayer player = cardPlayers[i];
                int damage = River.GetCollectionDamage(player.HandAttack, Metrics);

                for (int j = 0; j < cardPlayers.Length; j++)
                {
                    if (j != i)
                    {
                        cardPlayers[j].TakeDamage(damage);
                    }
                    yield return new WaitForSeconds(.5f);
                }

                player.HandAttack.Clear();
            }

            yield return new WaitForSeconds(1.5f);
        }

        yield return StartCoroutine(HandleGameOverWithRetry(losingPlayer));
    }

    private IEnumerator HandleGameOverWithRetry(int losingPlayerIndex)
    {
        CardPlayer loser = cardPlayers[losingPlayerIndex];
        CardPlayer winner = GetWinner(losingPlayerIndex);

        for (int i = 0; i < cardPlayers.Length; i++)
        {
            cardPlayers[i].HandAttack.Clear();
            cardPlayers[i].MainHand.Clear();
        }

        if (winner != null)
        {
            winner.AddMoney(moneyReward);
        }

        OnGameOver?.Invoke(loser);
        OnPlayerWon?.Invoke(winner);

        yield return new WaitForSeconds(delayBeforeRetry);

        OnDesactivateCanvas?.Invoke();

        yield return new WaitForSeconds(0.5f);

        RetryGame();
    }

    private CardPlayer GetWinner(int losingPlayerIndex)
    {
        for (int i = 0; i < cardPlayers.Length; i++)
        {
            if (i != losingPlayerIndex) 
                return cardPlayers[i];
        }
        return null;
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

    public void RetryGame()
    {
        StopAllCoroutines();

        CurrentTurn = 0;
        CurrentTurnPlayer = null;
        River.Clear();
        Deck.Clear();

        for (int i = 0; i < cardPlayers.Length; i++)
        {
            cardPlayers[i].MainHand.Clear();
            cardPlayers[i].HandAttack.Clear();
        }

        gameCoroutine = StartCoroutine(PlayCardGame());
    }

    private void RefreshDeck()
    {
        Deck.Clear();
        Deck.FillCollectionWithAllCards(Metrics);
        Deck.Shuffle();
    }
}
