using System;
using UnityEngine;
using Assets.Scripts.Upgrades;

namespace Assets.Scripts.Upgrades
{
    // Increases the projectile speed of the player's LaunchProjectile component.
    [Serializable]
    public class UpgradeProjectileSpeed : UpgradeBase
    {
        [Range(0f, 5f)] public float percentPerLevel = 0.50f; // +20% per level by default

        private LaunchProjectile cachedLauncher;
        private float baseSpeed;
        private bool baseCaptured;

        public UpgradeProjectileSpeed()
        {
            Title = "Hollow-Point Rounds";
            Description = "+20% projectile speed per level (max 3).";
            MaxLevel = 3;
        }

        public override bool ApplyUpgrade(GameObject target)
        {
            if (!base.ApplyUpgrade(target))
                return false;

            if (target == null)
                return false;

            if (cachedLauncher == null)
                cachedLauncher = target.GetComponent<LaunchProjectile>();

            if (cachedLauncher == null)
                return false; // No launcher found on target

            if (!baseCaptured)
            {
                baseSpeed = cachedLauncher.speed;
                baseCaptured = true;
            }

            // New speed = baseSpeed * (1 + percent)^{CurrentLevel}
            var multiplier = Mathf.Pow(1f + percentPerLevel, CurrentLevel);
            cachedLauncher.speed = baseSpeed * multiplier;
            return true;
        }

    }
}
