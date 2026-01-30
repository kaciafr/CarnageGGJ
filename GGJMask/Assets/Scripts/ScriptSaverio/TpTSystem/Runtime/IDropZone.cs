using UnityEngine;

namespace RunTime.TpTSystem
{
    public interface IDropZone
    {
        bool isAcceptable(Card card);
        void AcceptDrop(Card card);
    }
}
