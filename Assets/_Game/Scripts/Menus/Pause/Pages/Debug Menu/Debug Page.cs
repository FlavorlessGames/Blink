using UnityEngine.UIElements;
using UnityEngine;
using fgames.Playtesting;

namespace fgames.Menus
{
    public class DebugPage
    {
        public DebugPage(PauseScreenPage page)
        {
            Activate(page);
        }
        
        public void Activate(PauseScreenPage page)
        {
            page.Clear();
            page.Header.Add(new Label("Debug Page"));
            page.Body.Add(Utils.NewToggle("Disable Statues", DebugManager.Instance.DisableStatues, (newValue) => {
                Debug.Assert(newValue != DebugManager.Instance.DisableStatues);
                DebugManager.Instance.DisableStatues = newValue;
            }));
            page.Body.Add(Utils.NewToggle("Infinite Battery", DebugManager.Instance.InfiniteBattery, (newValue) => {
                Debug.Assert(newValue != DebugManager.Instance.InfiniteBattery);
                DebugManager.Instance.InfiniteBattery = newValue;
            }));
            page.Body.Add(Utils.NewToggle("Unlimited Battery Packs", DebugManager.Instance.UnlimitedBatteryPacks, (newValue) => {
                Debug.Assert(newValue != DebugManager.Instance.UnlimitedBatteryPacks);
                DebugManager.Instance.UnlimitedBatteryPacks= newValue;
            }));
            page.Body.Add(Utils.NewToggle("Level Does Not Reset", DebugManager.Instance.LevelDoesNotReset, (newValue) => {
                Debug.Assert(newValue != DebugManager.Instance.LevelDoesNotReset);
                DebugManager.Instance.LevelDoesNotReset = newValue;
            }));
            page.Body.Add(Utils.NewToggle("Statue Self Harm", DebugManager.Instance.StatueSelfHarm, (newValue) => {
                Debug.Assert(newValue != DebugManager.Instance.StatueSelfHarm);
                DebugManager.Instance.StatueSelfHarm = newValue;
            }));
            page.Body.Add(Utils.NewToggle("Player Warp On Death", DebugManager.Instance.PlayerWarpOnDeath, (newValue) => {
                Debug.Assert(newValue != DebugManager.Instance.PlayerWarpOnDeath);
                DebugManager.Instance.PlayerWarpOnDeath = newValue;
            }));
            page.Body.Add(Utils.NewToggle("DisableFlashlightCooldown", DebugManager.Instance.DisableFlashlightCooldown, (newValue) => {
                Debug.Assert(newValue != DebugManager.Instance.DisableFlashlightCooldown);
                DebugManager.Instance.DisableFlashlightCooldown = newValue;
            }));
        }
    }
}