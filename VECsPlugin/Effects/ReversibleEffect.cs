using UnboundLib.Cards;
using UnityEngine;

namespace VECsPlugin.Effects
{
    public interface ReversibleEffect
    {
        void EndGame();
        void SetupRound();
        void CleanupRound();
    }
}