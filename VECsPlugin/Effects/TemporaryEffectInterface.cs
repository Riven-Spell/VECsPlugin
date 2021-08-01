using UnboundLib.Cards;
using UnityEngine;

namespace VECsPlugin.Effects
{
    public interface GameTemporaryEffect
    {
        void EndGame();
    }
    
    public interface RoundTemporaryEffect : GameTemporaryEffect
    {
        void SetupRound();
        void CleanupRound();
    }
}