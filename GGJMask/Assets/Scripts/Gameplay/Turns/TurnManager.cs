using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Gameplay.Masque.Effect;
using Masque;
using Masque.Effect;
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

    [SerializeField] public int moneyReward = 500;
    [SerializeField] private float delayBeforeRetry = 2f;

    [Header("⏳ Cooldowns des Masques")]
    [SerializeField] private int cooldownDuration = 3;

    private Coroutine gameCoroutine;

    public MaskManager MaskManager; 

    public ReloadHitEffect reloadHitEffect;
    public RiverChangeEffect reloadChangeEffect;
    public MoneyGain MoneyGain; 
    public SwapHandsEffect swapHandsEffect;
    public SwapHandWithRiverEffect swapHandWithRiverEffect;

    private Dictionary<string, int> maskCooldowns = new Dictionary<string, int>();

    private void Awake()
    {
        River = new River();
        Deck = new Deck();
        
        maskCooldowns["ReloadHit"] = 0;
        maskCooldowns["ReloadRiver"] = 0;
        maskCooldowns["MoneyGain"] = 0;
        maskCooldowns["SwapHands"] = 0;
        maskCooldowns["SwapHandRiver"] = 0;
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
            
            UpdateCooldowns();

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

    private void UpdateCooldowns()
    {
        List<string> keys = new List<string>(maskCooldowns.Keys);
        foreach (string key in keys)
        {
            if (maskCooldowns[key] > 0)
            {
                maskCooldowns[key]--;
                Debug.Log($"⏳ {key} cooldown: {maskCooldowns[key]} tours restants");
            }
        }
    }

    private bool CanUseMask(string maskName)
    {
        if (maskCooldowns[maskName] > 0)
        {
            Debug.Log($" {maskName} en cooldown ! Attends {maskCooldowns[maskName]} tours");
            return false;
        }
        return true;
    }

    private void ActivateCooldown(string maskName)
    {
        maskCooldowns[maskName] = cooldownDuration;
        Debug.Log($" {maskName} utilisé ! Cooldown de {cooldownDuration} tours activé");
    }

    public void UseMaskReloadHitEffect()
    { 
        if (!CanUseMask("ReloadHit")) return;

        reloadHitEffect.ApplyEffect(this);
        RefreshDeck();
        ActivateCooldown("ReloadHit");
    }

    public void UseMaskReloadRiver()
    {
        if (!CanUseMask("ReloadRiver")) return;

        reloadChangeEffect.ApplyEffect(this);
        ActivateCooldown("ReloadRiver");
    }

    public void UseGainMoney()
    { 
        if (!CanUseMask("MoneyGain")) return;

        MoneyGain.ApplyEffect(this);
        ActivateCooldown("MoneyGain");
    }

    public void SwapHandsEffect()
    {
        if (!CanUseMask("SwapHands")) return;

        swapHandsEffect.ApplyEffect(this);
        ActivateCooldown("SwapHands");
    }

    public void SwapHandWithRiverEffect()
    {
        if (!CanUseMask("SwapHandRiver")) return;

        swapHandWithRiverEffect.ApplyEffect(this);
        ActivateCooldown("SwapHandRiver");
    }

    public int GetCooldown(string maskName)
    {
        return maskCooldowns.ContainsKey(maskName) ? maskCooldowns[maskName] : 0;
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
        RefreshDeck();
        
        foreach (var key in new List<string>(maskCooldowns.Keys))
        {
            maskCooldowns[key] = 0;
        }

        for (int i = 0; i < cardPlayers.Length; i++)
        {
            cardPlayers[i].MainHand.Clear();
            cardPlayers[i].HandAttack.Clear();
            cardPlayers[i].HandDefense.Clear();
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
