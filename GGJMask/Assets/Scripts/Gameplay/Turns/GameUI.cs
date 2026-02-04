using System;
using RunTime.TpTSystem;
using UnityEngine;

namespace Gameplay.CardSystem.Turns
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private CardCollectionUI riverUI;
        
        [SerializeField] private CardPlayerUI cardPlayerUI1;
        [SerializeField] private CardPlayerUI cardPlayerUI2;

        public TurnManager TurnManager { get; private set; }
        private void OnEnable()
        {
            TurnManager turnManager = GameObject.FindFirstObjectByType<TurnManager>();
            if(turnManager != null)
                Connect(turnManager);
        }

        private void OnDisable()
        {
            Disconnect();
        }

        public void Connect(TurnManager turnManager)
        {
            if(TurnManager != null)
                Disconnect();
            
            TurnManager = turnManager;
            TurnManager.OnSetupPhase += SetupUI;
        }


        public void Disconnect()
        {
            if(TurnManager == null)
                return;
            
            TurnManager.OnSetupPhase -= SetupUI;
            TurnManager = null;
        }
        
        private void SetupUI()
        {
            riverUI.Connect(TurnManager.River);
            cardPlayerUI1.Connect(TurnManager, TurnManager.Players[0]);
            cardPlayerUI2.Connect(TurnManager, TurnManager.Players[1]);
        }
    }
}