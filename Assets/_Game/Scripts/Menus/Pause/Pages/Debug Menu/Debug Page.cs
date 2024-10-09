using UnityEngine.UIElements;
using fgames.Debug;

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
                DebugManager.Instance.DisableStatues = newValue;
            }));
            page.Body.Add(Utils.NewToggle("Infinite Battery", DebugManager.Instance.InfiniteBattery, (newValue) => {
                DebugManager.Instance.InfiniteBattery = newValue;
            }));
            page.Body.Add(Utils.NewToggle("Unlimited Battery Packs", DebugManager.Instance.UnlimitedBatteryPacks, (newValue) => {
                DebugManager.Instance.UnlimitedBatteryPacks= newValue;
            }));
            page.Body.Add(Utils.NewToggle("Level Does Not Reset", DebugManager.Instance.LevelDoesNotReset, (newValue) => {
                DebugManager.Instance.LevelDoesNotReset = newValue;
            }));
            page.Body.Add(Utils.NewToggle("Statue Self Harm", DebugManager.Instance.StatueSelfHarm, (newValue) => {
                DebugManager.Instance.StatueSelfHarm = newValue;
            }));
            page.Body.Add(Utils.NewToggle("Player Warp On Death", DebugManager.Instance.PlayerWarpOnDeath, (newValue) => {
                DebugManager.Instance.PlayerWarpOnDeath = newValue;
            }));
            page.Body.Add(Utils.NewToggle("DisableFlashlightCooldown", DebugManager.Instance.DisableFlashlightCooldown, (newValue) => {
                DebugManager.Instance.DisableFlashlightCooldown = newValue;
            }));
        }
    }
}