using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Upgrades
{
    // Attached to the player; manages available and applied upgrades.
    public class UpgradeController : MonoBehaviour
    {
        // Upgrades that can be rolled/shown in UI
        [SerializeField] private List<UpgradeBase> availableUpgrades = new List<UpgradeBase>();

        // Upgrades that have been applied at least once
        private readonly List<UpgradeBase> appliedUpgrades = new List<UpgradeBase>();

        // Event invoked when an upgrade is applied; subscribers (UI) can react
        public event Action<UpgradeBase> OnUpgradeApplied;

        public IReadOnlyList<UpgradeBase> AvailableUpgrades => availableUpgrades;
        public IReadOnlyList<UpgradeBase> AppliedUpgrades => appliedUpgrades;

        // Apply a specific upgrade to this player GameObject
        public bool ApplyUpgrade(UpgradeBase upgrade)
        {
            if (upgrade == null)
                return false;

            // Ensure the upgrade applies to this gameObject (player)
            if (!upgrade.CanApply)
                return false;

            // Let the upgrade execute its effect and progress its own level
            var progressed = upgrade.ApplyUpgrade(gameObject);
            if (!progressed)
                return false;

            if (!appliedUpgrades.Contains(upgrade))
                appliedUpgrades.Add(upgrade);

            OnUpgradeApplied?.Invoke(upgrade);
            return true;
        }

        // Optionally allow registering new upgrades at runtime (likely through some sort of ui?)
        public void RegisterAvailableUpgrade(UpgradeBase upgrade)
        {
            if (upgrade != null && !availableUpgrades.Contains(upgrade))
            {
                availableUpgrades.Add(upgrade);
            }
        }


    }
}
