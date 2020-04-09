using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace FreeSkillMod
{
    class FreeSkillCampaignBehavior : CampaignBehaviorBase
    {    

        public override void RegisterEvents()
        {
            CampaignEvents.HeroLevelledUp.AddNonSerializedListener(this, new Action<Hero, bool>(OnHeroLeveledUp));
        }

        public void OnHeroLeveledUp(Hero hero, bool shouldNotify)
        {            
            FreeSkill.Get(hero.StringId).TotalSkillPoints += 1;
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("fnzr_FreeSkill", ref FreeSkillDataMap);
        }

        public static Dictionary<string, FreeSkillData> FreeSkillDataMap = new Dictionary<string, FreeSkillData>();

        public class FreeSkillSaveDefiner : SaveableTypeDefiner
        {
            public FreeSkillSaveDefiner() : base(1465509420)
            {

            }

            protected override void DefineClassTypes()
            {
                AddClassDefinition(typeof(FreeSkillData), 1);
            }

            protected override void DefineContainerDefinitions()
            {
                ConstructContainerDefinition(typeof(Dictionary<string, FreeSkillData>));
            }
        }
    }
}
