using UnityEngine;
using Unity.Netcode;

namespace fgames.Playtesting
{
    public class DebugManager : SingletonNetwork<DebugManager>
    {
        public bool InfiniteBattery { 
            get { return _infiniteBattery.Value; } 
            set {
                _infiniteBattery.Value = value;
                InfiniteBatteryUpdate?.Invoke(_infiniteBattery.Value);
            }
        }
        [SerializeField] private NetworkVariable<bool> _infiniteBattery;
        public event FlagUpdate InfiniteBatteryUpdate;

        public bool UnlimitedBatteryPacks {
            get { return _unlimitedBatteryPacks.Value; }
            set {
                _unlimitedBatteryPacks.Value = value;;
                UnlimitedBatteryPacksUpdate?.Invoke(_unlimitedBatteryPacks.Value);
            }
        } 
        [SerializeField] private NetworkVariable<bool> _unlimitedBatteryPacks;
        public event FlagUpdate UnlimitedBatteryPacksUpdate; 

        public bool LevelDoesNotReset {
            get { return _levelDoesNotReset.Value; }
            set {
                _levelDoesNotReset.Value = value;
                LevelDoesNotResetUpdate?.Invoke(_levelDoesNotReset.Value);
            }
        }
        [SerializeField] private NetworkVariable<bool> _levelDoesNotReset;
        public event FlagUpdate LevelDoesNotResetUpdate; 

        public bool StatueSelfHarm {
            get { return _statueSelfHarm.Value; }
            set {
                _statueSelfHarm.Value = value;
                StatueSelfHarmUpdate?.Invoke(_statueSelfHarm.Value);
            }
        }
        [SerializeField] private NetworkVariable<bool> _statueSelfHarm;
        public event FlagUpdate StatueSelfHarmUpdate; 

        public bool PlayerWarpOnDeath {
            get { return _playerWarpOnDeath.Value; }
            set {
                _playerWarpOnDeath.Value = value;
                PlayerWarpOnDeathUpdate?.Invoke(_playerWarpOnDeath.Value);
            }
        }
        [SerializeField] private NetworkVariable<bool> _playerWarpOnDeath;
        public event FlagUpdate PlayerWarpOnDeathUpdate; 

        public bool DisableFlashlightCooldown {
            get { return _disableFlashlightCooldown.Value; }
            set {
                _disableFlashlightCooldown.Value = value;
                DisableFlashlightCooldownUpdate?.Invoke(_disableFlashlightCooldown.Value);
            }
        }
        [SerializeField] private NetworkVariable<bool> _disableFlashlightCooldown;
        public event FlagUpdate DisableFlashlightCooldownUpdate; 

        public bool DisableStatues { 
            get { return _disableStatues.Value; } 
            set {
                _disableStatues.Value = value;
                DisableStatuesUpdate?.Invoke(_disableStatues.Value);
            }
        }
        [SerializeField] private NetworkVariable<bool> _disableStatues;
        public event FlagUpdate DisableStatuesUpdate;
    }
}

public delegate void FlagUpdate(bool flag);