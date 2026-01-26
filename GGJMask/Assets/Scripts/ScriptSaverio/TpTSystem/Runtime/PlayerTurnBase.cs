        
using System;
using UnityEditor;
using UnityEngine;

namespace RunTime.TpTSystem
{
    public class PlayerTurnBase :  MonoBehaviour
    {
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
