using Base.UI;
using Base.Entities.Abilities;
using Base.Entities.Statuses;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.UI;
using PhoenixPoint.Tactical.Entities.Abilities;
using UnityEngine;

namespace PistolProficiencyMod {
    internal static class BonusPistolPerks {
        internal static PassiveModifierAbilityDef Get_BonusPistolDamagePerk() {
            //Check if perk is already in def library:
            PassiveModifierAbilityDef BonusPistolDamage = (PassiveModifierAbilityDef)PistolProficiencyModMain.Repo.GetDef("3e38fd1c-2a58-46d4-9773-cc377e595045");
            if (BonusPistolDamage == null) { //If it doesn't exist, we create it:
                //This is the legacy "HandgunsTalent_AbilityDef" ability;
                PassiveModifierAbilityDef HandgunPerk = (PassiveModifierAbilityDef)PistolProficiencyModMain.Repo.GetDef("1dee177d-6461-c064-4901-bc95c4c12443");
                //Clone its properties:
                BonusPistolDamage = (PassiveModifierAbilityDef)PistolProficiencyModMain.Repo.CreateDef("3e38fd1c-2a58-46d4-9773-cc377e595045", HandgunPerk);
                BonusPistolDamage.CharacterProgressionData = (AbilityCharacterProgressionDef)PistolProficiencyModMain.Repo.CreateDef("7fc8b8b8-bd9b-4de2-b8c5-df272a391ce4", HandgunPerk.CharacterProgressionData);
                BonusPistolDamage.ViewElementDef = (TacticalAbilityViewElementDef)PistolProficiencyModMain.Repo.CreateDef("50cb63f5-7d55-42a2-8433-50535109c3fd", HandgunPerk.ViewElementDef);
                //Edit relevant modifier values (Note that the value of the multiplier is handled in ModMain):
                BonusPistolDamage.ItemTagStatModifications[0].EquipmentStatModification.TargetStat = StatModificationTarget.BonusAttackDamage;
                BonusPistolDamage.ItemTagStatModifications[0].EquipmentStatModification.Modification = StatModificationType.Multiply;
                BonusPistolDamage.name = "BonusPistolDamage_AbilityDef";

                BonusPistolDamage.CharacterProgressionData.SkillPointCost = 0;

                //Change Sprite & UI data: Sprite is from ExpertLightWeapons
                Sprite NewSprite = ((PassiveModifierAbilityDef)PistolProficiencyModMain.Repo.GetDef("5d26564f-d648-3094-ca84-1c12fa6702fc")).ViewElementDef.LargeIcon;
                BonusPistolDamage.ViewElementDef.LargeIcon = NewSprite;
                BonusPistolDamage.ViewElementDef.SmallIcon = NewSprite;
                BonusPistolDamage.ViewElementDef.DisplayName1 = new LocalizedTextBind("Bonus Handgun Proficiency: Damage", true);
            }
            return BonusPistolDamage;
        }

        //Repeat above process for next ability
        internal static PassiveModifierAbilityDef Get_BonusPistolAccuracyPerk() {
            PassiveModifierAbilityDef BonusPistolAccuracy = (PassiveModifierAbilityDef)PistolProficiencyModMain.Repo.GetDef("e56f29db-7006-45ef-89e5-1d5d9a0c06d1");
            if (BonusPistolAccuracy == null) { 
                PassiveModifierAbilityDef HandgunPerk = (PassiveModifierAbilityDef)PistolProficiencyModMain.Repo.GetDef("1dee177d-6461-c064-4901-bc95c4c12443");
                // Clone
                BonusPistolAccuracy = (PassiveModifierAbilityDef)PistolProficiencyModMain.Repo.CreateDef("e56f29db-7006-45ef-89e5-1d5d9a0c06d1", HandgunPerk);
                BonusPistolAccuracy.CharacterProgressionData = (AbilityCharacterProgressionDef)PistolProficiencyModMain.Repo.CreateDef("dc3b4355-e7c8-4f2b-80e7-a68b266bf364", HandgunPerk.CharacterProgressionData);
                BonusPistolAccuracy.ViewElementDef = (TacticalAbilityViewElementDef)PistolProficiencyModMain.Repo.CreateDef("bd272a84-2ff0-4d9a-b6c4-e3f6bbf42659", HandgunPerk.ViewElementDef);
                // Edit 
                BonusPistolAccuracy.ItemTagStatModifications[0].EquipmentStatModification.TargetStat = StatModificationTarget.Accuracy;
                BonusPistolAccuracy.ItemTagStatModifications[0].EquipmentStatModification.Modification = StatModificationType.Multiply;
                BonusPistolAccuracy.name = "BonusPistolAccuracy_AbilityDef";

                BonusPistolAccuracy.CharacterProgressionData.SkillPointCost = 0;

                //Change UI data: Sprite doesn't need editing since it will be the same as the "HandgunsTalent" that we cloned for the perk
                BonusPistolAccuracy.ViewElementDef.DisplayName1 = new LocalizedTextBind("Bonus Handgun Proficiency: Accuracy", true);
            }
            return BonusPistolAccuracy;
        }
    }
}