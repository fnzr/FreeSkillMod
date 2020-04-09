using TaleWorlds.SaveSystem;

namespace FreeSkillMod
{
    static class FreeSkill
    {
        public static FreeSkillData Get(string heroId)
        {
            // What the hell
            if (heroId == null)
            {                
                return new FreeSkillData();
            }
            if (FreeSkillCampaignBehavior.FreeSkillDataMap.ContainsKey(heroId))
            {
                return FreeSkillCampaignBehavior.FreeSkillDataMap[heroId];
            }
            var instance = new FreeSkillData(heroId);
            FreeSkillCampaignBehavior.FreeSkillDataMap.Add(heroId, instance);
            return instance;
        }
    }

    class FreeSkillData
    {
        public FreeSkillData()
        {
        }

        public FreeSkillData(string heroId)
        {
            HeroId = heroId;
        }
        
        [SaveableField(1)]
        public string HeroId;

        [SaveableField(2)]
        public int TotalSkillPoints;

        [SaveableField(3)]
        public int SpentSkillPoints;
        
        public int UnspentSkillPoints => TotalSkillPoints - SpentSkillPoints; 
        
    }
}
