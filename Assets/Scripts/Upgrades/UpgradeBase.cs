using System;
using UnityEngine;

namespace Assets.Scripts.Upgrades
{
    // Base class for all upgrades. Holds shared metadata and level control logic.
    public abstract class UpgradeBase : ScriptableObject
    {
        // UI metadata for displaying this upgrade in a future UI
        public Sprite Icon;
        public string Title;
        public string Description;

        public int MaxLevel = 1;
        public int CurrentLevel { get; protected set; }
        public bool CanApply => CurrentLevel < MaxLevel;

        // Base logic: prevent applying beyond max level and track level progression.
        // should be extended by derived classes to implement actual upgrade effects.
        public virtual bool ApplyUpgrade(GameObject target)
        {
            if (!CanApply)
                return false;

            // Increment level here so overrides can use the new level value
            CurrentLevel++;
            return true;
        }
    }
}
