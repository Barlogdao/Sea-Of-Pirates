﻿using Project.Configs.GameResources;
using Project.Utils;
using UnityEngine;

namespace Project.Systems.Stats
{
    [System.Serializable]
    public class UpgradeCost
    {
        [SerializeField] private GameResource _resource;

        [field: SerializeField] protected int MinCost { get; private set; }
        [field: SerializeField] protected int MaxCost { get; private set; }

        public GameResourceAmount GetCost(StatConfig stat, int level)
        {
            int cost = ComputeCost(stat, level);

            return new GameResourceAmount(_resource, cost);
        }

        protected virtual int ComputeCost(StatConfig statConfig, int level)
        {
            return (int)ExtendedMath.Remap(level, statConfig.MinLevel, statConfig.MaxLevel, MinCost, MaxCost);
        }
    }
}