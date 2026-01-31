using System;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.TpTSystem
{
    public class PlayerTurnBase :  MonoBehaviour
    {
        private List<Cards> handCards;
        public event Action<PlayerTurnBase> OnPlayerTurn;
        public bool isMyTurn = false;

        public void StartTurn()
        {
            isMyTurn = true;
            Debug.Log("StartTurn");
            
            
        }

        public void EndTurn()
        {
            if (!isMyTurn)
                return;
            
            isMyTurn = false;
            Debug.Log("EndTurn");
            OnPlayerTurn?.Invoke(this);
        }

    }
}
