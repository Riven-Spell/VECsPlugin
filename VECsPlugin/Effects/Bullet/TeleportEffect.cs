using System;
using UnityEngine;

namespace VECsPlugin.Effects.Bullet
{
    public class TeleportEffect : RayHitEffect
    {
        public Transform playerTransform;

        private Camera mainCam;
        private Vector2 offset = Vector2.zero;
        
        public override HasToReturn DoHitEffect(HitInfo hit)
        {
            UnityEngine.Debug.Log(hit.point);
            playerTransform.position = hit.point;
            playerTransform.GetComponentInChildren<PlayerCollision>().IgnoreWallForFrames(2);

            return HasToReturn.canContinue;
        }

        public void Start()
        {
            mainCam = MainCam.instance.transform.GetComponent<Camera>();
            
            // Determine if the screen is reasonably larger than 16:9, and adjust for it
            var standardRatio = 16f / 9f;
            var ratio = standardRatio / mainCam.aspect;
            if (Mathf.Abs(ratio - 1) > Single.Epsilon)
            {
                if (ratio < 1)
                {
                    // It's wider
                    offset.x = Screen.width * (1 - ratio) / 2;
                }
                else
                {
                    // It's taller
                    offset.y = Screen.height * (ratio - 1) / 2;
                }
            }
        }

        public void Update()
        {
            // Screen edge detection
            var position = mainCam.WorldToScreenPoint(transform.position);
            position.x -= offset.x;
            position.y -= offset.y;
            position.x /= Screen.width - (offset.x * 2);
            position.y /= Screen.height - (offset.y * 2);
            if (position.x <= 0 || position.x >= 1 || position.y <= 0 || position.y >= 1)
            {
                playerTransform.position = transform.position;
                playerTransform.GetComponentInChildren<PlayerCollision>().IgnoreWallForFrames(2);

                DestroyImmediate(gameObject);
            }
        }
    }
}