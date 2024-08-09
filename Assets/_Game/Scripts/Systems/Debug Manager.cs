using UnityEngine;

namespace fgames.Debug
{
    public class DebugManager : Singleton<DebugManager>
    {
        public bool InfiniteBattery { 
            get { return _infiniteBattery; } 
            set {
                _infiniteBattery = value;
                InfiniteBatteryUpdate?.Invoke(_infiniteBattery);
            }
        }
        [SerializeField] private bool _infiniteBattery;
        public event FlagUpdate InfiniteBatteryUpdate;
        public bool UnlimitedBatteryPacks => _unlimitedBatteryPacks;
        [SerializeField]
        private bool _unlimitedBatteryPacks;
        public bool LevelDoesNotReset = false;
        public bool StatueSelfHarm = false;
        public bool PlayerWarpOnDeath = false;
        public bool DisableFlashlightCooldown = false;
        public bool DisableStatues { 
            get { return _disableStatues; } 
            set {
                _disableStatues = value;
                DisableStatuesUpdate?.Invoke(_disableStatues);
            }
        }
        [SerializeField] private bool _disableStatues;
        public event FlagUpdate DisableStatuesUpdate;
    }
}

public delegate void FlagUpdate(bool flag);