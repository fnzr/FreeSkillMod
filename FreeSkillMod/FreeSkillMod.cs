using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using UIExtenderLib;

namespace FreeSkillMod
{
    
    [HarmonyPatch(typeof(HeroDeveloper), "GetTotalSkillPoints")]
    public class HeroDeveloperPatch
    {        
        static void Postfix(HeroDeveloper __instance, ref int __result)
        {
            __result -= FreeSkill.Get(__instance.StringId).SpentSkillPoints;
        }
    }
    
    public class FreeSkillMod : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            UIExtender.Register();
        }
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            var campaign = game.GameType as Campaign;
            if (campaign != null)
            {
                var initializer = (CampaignGameStarter)gameStarterObject;
                initializer.AddBehavior(new FreeSkillCampaignBehavior());
            }
        }
    }
}
