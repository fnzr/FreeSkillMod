using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using UIExtenderLib;
using UIExtenderLib.Prefab;
using UIExtenderLib.ViewModel;

namespace FreeSkillMod
{
    
    [PrefabExtension("CharacterDeveloper", "/Prefab/Window/Widget/Children/Widget[1]/Children/Widget/Children")]
    public class CharacterDeveloperExtensionUnspentSkillPoints : PrefabExtensionInsertPatch
    {
        public override string Name => "UnspentSkillPoints";

        public override int Position => PositionLast;
    }
    
    [PrefabExtension("CharacterDeveloper", "descendant::ListPanel[@Id='SkillPropertiesList']/Children/ListPanel/Children")]
    public class CharacterDeveloperExtensionAddSkillButton : PrefabExtensionInsertPatch
    {
        public override string Name => "AddSkillPoint";

        public override int Position => 0;
    }
    
    
    [ViewModelMixin]
    public class CharacterMixin : BaseViewModelMixin<CharacterVM>
    {
        //TODO: properly use this as i18n string
        private static readonly string HintText = "Characters can use Skill Points to increase the level of any skill.\nFree Skill Points are awarded every time the character levels up.";

        private readonly FreeSkillData _freeSkillData;
        [DataSourceProperty] public int UnspentSkillPoints => _freeSkillData.UnspentSkillPoints;

        [DataSourceProperty] public bool CanAddSkill => _freeSkillData.UnspentSkillPoints > 0;

        [DataSourceProperty] public HintViewModel SkillPointsHint => new HintViewModel(HintText, null);

        public CharacterMixin(CharacterVM vm) : base(vm)
        {
            _freeSkillData = FreeSkill.Get(_vm.Hero.StringId);
        }

        [DataSourceMethod]
        public void ExecuteAddSkill()
        {
            if (!CanAddSkill)
            {
                return;
            }
            var skillVm = _vm.CurrentSkill;
            var exp = skillVm.XpRequiredForNextLevel - skillVm.CurrentSkillXP;
            var expPercent = (float)(Campaign.Current.Models.CharacterDevelopmentModel.GetXpRequiredForSkillLevel(skillVm.Level + 2) - Campaign.Current.Models.CharacterDevelopmentModel.GetXpRequiredForSkillLevel(skillVm.Level + 1));
            expPercent *= (float)(skillVm.ProgressPercentage / 100);

            _vm.GetCharacterDeveloper().AddSkillXp(skillVm.Skill, exp + expPercent, false, false);
            _freeSkillData.SpentSkillPoints += 1;
            _vm.OnPropertyChanged(nameof(UnspentSkillPoints));
            if (!CanAddSkill)
            {
                _vm.OnPropertyChanged(nameof(CanAddSkill));
            }
            skillVm.InitializeValues();
        }
    }
    
}
