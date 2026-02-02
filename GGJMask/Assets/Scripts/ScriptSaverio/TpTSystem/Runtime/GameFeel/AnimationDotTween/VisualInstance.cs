using UnityEngine;

namespace RunTime.TpTSystem
{
    public class VisualInstance : MonoBehaviour
    {
        public static VisualInstance Instance;

        public void Awake()
        {
            Instance = this;
        }
    }
}
