using UnboundLib.Cards;
using UnityEngine;

namespace VECsPlugin.Effects
{
    public abstract class ReversibleEffect : MonoBehaviour
    {
        public abstract void EndGame();
        public abstract void SetupRound();
        public abstract void CleanupRound();
    }
}