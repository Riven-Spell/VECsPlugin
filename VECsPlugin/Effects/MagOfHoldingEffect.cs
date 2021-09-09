using UnityEngine;

namespace VECsPlugin.Effects
{
    public class MagOfHoldingEffect : MonoBehaviour, GameTemporaryEffect
    {
        private Player _p;
        private Gun _g;
        private GunAmmo _ga;
        private bool _prepared;

        private bool willUndermine;

        public void PrepareOnce(Player p, GunAmmo ga, Gun g)
        {
            if (_prepared)
                return;
            
            _p = p;
            _g = g;
            _ga = ga;
            
            _p.data.stats.OutOfAmmpAction += OutOfAmmoAction;
            _p.data.stats.OnReloadDoneAction += OnReloadDoneAction;

            _prepared = true;
        }

        private void OnReloadDoneAction(int obj)
        {
            if (willUndermine)
            {
                _ga.maxAmmo--;                
            }
        }

        private void OutOfAmmoAction(int obj)
        {
            if (_ga.maxAmmo > 3)
            {
                willUndermine = true;
            }
        }

        public void EndGame() { }
    }
}