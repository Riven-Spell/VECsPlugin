using System;
using UnityEngine;

namespace VECsPlugin.Effects
{
    public class FailTryBlockHookEffect : MonoBehaviour, GameTemporaryEffect
    {
        /*
         * Real implementation hours: Patches/FailTryBlockHookPatch.cs
         */

        public event Action<Block> OnFailBlockAction;

        public void FailedBlock(Block b)
        {
            OnFailBlockAction?.Invoke(b);
        }
        
        public void EndGame() { }
    }
}