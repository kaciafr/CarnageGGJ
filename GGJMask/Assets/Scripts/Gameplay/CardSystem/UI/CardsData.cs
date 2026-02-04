using UnityEngine;

namespace RunTime.TpTSystem
{
    [CreateAssetMenu(fileName = "CardsData", menuName = "Scriptable Objects/CardsData")]
    public class CardsData : ScriptableObject
    {
        public string cardName;
        public int Id;
        public Sprite Icon;
    }
}
