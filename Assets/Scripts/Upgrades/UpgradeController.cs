using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Upgrades
{
    // Attached to the player; manages available and applied upgrades.
    public class UpgradeController : MonoBehaviour
    {
        // Asset references to available upgrades (ScriptableObjects)
        [SerializeField] private List<UpgradeBase> availableUpgrades = new List<UpgradeBase>();

        // Runtime instances of upgrades that have been applied at least once
        private readonly Dictionary<UpgradeBase, UpgradeBase> runtimeInstances = new Dictionary<UpgradeBase, UpgradeBase>();
        private readonly List<UpgradeBase> appliedUpgrades = new List<UpgradeBase>();

        // Event invoked when an upgrade is applied; subscribers (UI) can react
        public event Action<UpgradeBase> OnUpgradeApplied;

        public IReadOnlyList<UpgradeBase> AvailableUpgrades => availableUpgrades;
        public IReadOnlyList<UpgradeBase> AppliedUpgrades => appliedUpgrades;

        // Apply a specific upgrade asset to this player GameObject
        public bool ApplyUpgrade(UpgradeBase upgradeAsset)
        {
            if (upgradeAsset == null)
                return false;

            // Get or create a runtime clone so we don't mutate the asset
            if (!runtimeInstances.TryGetValue(upgradeAsset, out var instance) || instance == null)
            {
                instance = Instantiate(upgradeAsset);
                runtimeInstances[upgradeAsset] = instance;
            }

            if (!instance.CanApply)
                return false;

            // Execute the upgrade's effect against this GameObject
            var progressed = instance.ApplyUpgrade(gameObject);
            if (!progressed)
                return false;

            if (!appliedUpgrades.Contains(instance))
                appliedUpgrades.Add(instance);

            OnUpgradeApplied?.Invoke(instance);
            return true;
        }

        // Optionally allow registering new upgrades (assets) at runtime
        public void RegisterAvailableUpgrade(UpgradeBase upgradeAsset)
        {
            if (upgradeAsset != null && !availableUpgrades.Contains(upgradeAsset))
            {
                availableUpgrades.Add(upgradeAsset);
            }
        }

        // Convenience to trigger an upgrade by index from UI buttons
        public bool ApplyUpgradeByIndex(int index)
        {
            if (index < 0 || index >= availableUpgrades.Count)
                return false;

            return ApplyUpgrade(availableUpgrades[index]);
        }

        public void ApplyTestSceneUpgrade()
        {
            if (availableUpgrades[0] != null)
            {
                Console.WriteLine($"[UpgradeController] Applying test upgrade: {availableUpgrades[0].name}");
                ApplyUpgrade(availableUpgrades[0]);
            }
            else
            {
                Console.WriteLine("[UpgradeController] No upgrade found at index 0 for test application.");
            }
        }
    }
}
