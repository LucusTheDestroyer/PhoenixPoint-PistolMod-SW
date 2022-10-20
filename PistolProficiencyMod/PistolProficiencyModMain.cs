using Base.Defs;
using Base.Core;
using Base.Levels;
using Base.Entities.Abilities;
using Base.UI;
using HarmonyLib;
using PhoenixPoint.Common.Game;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Common.Entities.GameTagsTypes;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Weapons;
using PhoenixPoint.Modding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PistolProficiencyMod
{
	/// This is the main mod class. Only one can exist per assembly.
	/// If no ModMain is detected in assembly, then no other classes/callbacks will be called.
	public class PistolProficiencyModMain : ModMain
	{
		// Use an enum and dictionary to map class names to their respective ClassProficiency Def files.
			enum SoldierClasses { Assault, Heavy, Sniper, Berserker, Priest, Infiltrator, Technician}
            Dictionary<SoldierClasses, ClassProficiencyAbilityDef> ClassMap = new Dictionary<SoldierClasses, ClassProficiencyAbilityDef> {
                {SoldierClasses.Assault, (ClassProficiencyAbilityDef)Repo.GetDef("491c2412-71c4-86b4-aab1-112fdfbd55ca")},	//Assault_ClassProficiency_AbilityDef
                {SoldierClasses.Heavy, (ClassProficiencyAbilityDef)Repo.GetDef("d7c64201-765b-e054-7926-41c7900eb0c8")},	//Heavy_ClassProficiency_AbilityDef
                {SoldierClasses.Sniper, (ClassProficiencyAbilityDef)Repo.GetDef("54328f21-e01a-4364-0aa7-4507affd2ccf")},	//Sniper_ClassProficiency_AbilityDef
                {SoldierClasses.Berserker, (ClassProficiencyAbilityDef)Repo.GetDef("86965a3a-6b95-89d4-4914-0f9fe092013c")},	//Berserker_ClassProficiency_AbilityDef
                {SoldierClasses.Priest, (ClassProficiencyAbilityDef)Repo.GetDef("0db867f2-6315-d454-89a0-612a339333a6")},	//Priest_ClassProficiency_AbilityDef
                {SoldierClasses.Infiltrator, (ClassProficiencyAbilityDef)Repo.GetDef("0b66016e-a59f-38a4-bb6b-859c11349321")},	//Infiltrator_ClassProficiency_AbilityDef
                {SoldierClasses.Technician, (ClassProficiencyAbilityDef)Repo.GetDef("6909d72f-a7b9-8364-2bde-5772686973f5")}	//Technician_ClassProficiency_AbilityDef
		    };

			// Additional Def files that are needed for ModMain:
			enum Extras { PistolItem, SniperTag, BerserkerTag, AllClasses }
			Dictionary< Extras, BaseDef> UsefulDefs = new Dictionary<Extras, BaseDef> {
				{Extras.PistolItem, Repo.GetDef("7a8a0a76-deb6-c004-3b5b-712eae0ad4a5")}, //HandgunItem_TagDef
				{Extras.SniperTag, Repo.GetDef("5ea5ff74-8494-4554-6a31-73bc06dc8fab")}, // Sniper_ClassTagDef
				{Extras.BerserkerTag, Repo.GetDef("ee4013e3-d258-9124-2899-5c5a9ad8e51d")}, // Berserker_ClassTagDef
				{Extras.AllClasses, Repo.GetDef("ad38f75d-7419-ab84-48ed-7a93631fbaf8")} // AllClasses_ClassTagDef
			};

		/// Config is accessible at any time, if any is declared.
		public new PistolProficiencyModConfig Config => (PistolProficiencyModConfig)base.Config;

		// Accessing the Game's definition repository
		internal static readonly DefRepository Repo = GameUtl.GameComponent<DefRepository>();

		/// This property indicates if mod can be Safely Disabled from the game.
		/// Safely sisabled mods can be reenabled again. Unsafely disabled mods will need game restart ot take effect.
		/// Unsafely disabled mods usually cannot revert thier changes in OnModDisabled
		public override bool CanSafelyDisable => true;

		/// Callback for when mod is enabled. Called even on game starup.
		public override void OnModEnabled() {
			/// All mod dependencies are accessible and always loaded.
			int c = Dependencies.Count();
			/// Mods have their own logger. Message through this logger will appear in game console and Unity log file.
			Logger.LogInfo($"Mod Enabled.");
			/// Metadata is whatever is written in meta.json
			string v = MetaData.Version.ToString();
			/// Game creates Harmony object for each mod. Accessible if needed.
			HarmonyLib.Harmony harmony = (HarmonyLib.Harmony)HarmonyInstance;
			/// Mod instance is mod's runtime representation in game.
			string id = Instance.ID;
			/// Game creates Game Object for each mod. 
			GameObject go = ModGO;
			/// PhoenixGame is accessible at any time.
			PhoenixGame game = GetGame();

			//We want to load each class proficiency file and add pistol proficiency to each class
			foreach (SoldierClasses SoldierClass in ClassMap.Keys){
				ClassProficiencyAbilityDef MyActor = ClassMap[SoldierClass];
				switch(SoldierClass){
					case SoldierClasses.Sniper: // Sniper case: gains bonus accuracy;
						//Get new perk from PistolProficiencyModBonusAbilities.cs
						PassiveModifierAbilityDef AccuracyPerk = BonusPistolPerks.Get_BonusPistolAccuracyPerk();
						// Apply config settings:
						AccuracyPerk.ItemTagStatModifications[0].EquipmentStatModification.Value = (1+((float)Config.Compensation/100));
						AccuracyPerk.ViewElementDef.Description = new LocalizedTextBind($"Handgun attacks gain {Config.Compensation}% Accuracy", true);
						MyActor.AbilityDefs = MyActor.AbilityDefs.AddToArray(AccuracyPerk);
						Logger.LogInfo($"Sniper Class Updated");
						break;
					case SoldierClasses.Berserker: // Berserker deals extra damage;
						PassiveModifierAbilityDef DamagePerk = BonusPistolPerks.Get_BonusPistolDamagePerk();
						DamagePerk.ItemTagStatModifications[0].EquipmentStatModification.Value = (1+((float)Config.Compensation/100));
						DamagePerk.ViewElementDef.Description = new LocalizedTextBind($"Handgun attacks gain {Config.Compensation}% Damage", true);
						MyActor.AbilityDefs = MyActor.AbilityDefs.AddToArray(DamagePerk);
						Logger.LogInfo($"Berserker Class Updated");
						break;
					default:
						//For all other classes, we just add Pistol Proficiency:
						MyActor.ClassTags.Add((ItemTypeTagDef)UsefulDefs[Extras.PistolItem]);
						Logger.LogInfo($"{SoldierClass} Class Updated");
						break;
				} 
			}
			// This part is to ensure that Pistols show up in every Class' equipment inventory by adding AllClasses tag to Handgun items.
			foreach (WeaponDef weapon in Repo.GetAllDefs<WeaponDef>().Where(w => w.Tags.Contains((GameTagDef)UsefulDefs[Extras.PistolItem]))) {
				if (weapon.Tags.Contains((GameTagDef)UsefulDefs[Extras.SniperTag])) {
					weapon.Tags.Remove((GameTagDef)UsefulDefs[Extras.SniperTag]);
					weapon.Tags.Remove((GameTagDef)UsefulDefs[Extras.BerserkerTag]);
					weapon.Tags.Add((GameTagDef)UsefulDefs[Extras.AllClasses]);
				}
			}	
			Logger.LogInfo($"PistolProficiencyMod successfully configured");
		}
		
		// Callback for when mod is disabled. This will be called even if mod cannot be safely disabled.
		// Guaranteed to have OnModEnabled before.
		public override void OnModDisabled() {
			/// Undo any game modifications if possible. Else "CanSafelyDisable" must be set to false.
			/// ModGO will be destroyed after OnModDisabled.
			foreach(SoldierClasses SoldierClass in ClassMap.Keys) {
				ClassProficiencyAbilityDef MyActor = ClassMap[SoldierClass];
				switch(SoldierClass) {
					case SoldierClasses.Sniper:
						// Method 1: RHS Returns a list/array containing all AbilityDefs other than the perk
						MyActor.AbilityDefs = MyActor.AbilityDefs.Where(perk => perk != BonusPistolPerks.Get_BonusPistolAccuracyPerk()).ToArray();
						break;
					case SoldierClasses.Berserker: {
						// Method 2: Convert the AbilityDefs to a temporary list, remove the perk, replace the old AbilityDefs with the new list.
						List<AbilityDef> Temp = MyActor.AbilityDefs.ToList();
						Temp.Remove(BonusPistolPerks.Get_BonusPistolDamagePerk());
						MyActor.AbilityDefs = Temp.ToArray();
						break;
						}
					default:
						MyActor.ClassTags.Remove((ItemTypeTagDef)UsefulDefs[Extras.PistolItem]);
						break;
				}
			}
			// Undo changes to visible inventory weapons;
			foreach (WeaponDef weapon in Repo.GetAllDefs<WeaponDef>().Where(w => w.Tags.Contains((GameTagDef)UsefulDefs[Extras.PistolItem]))) {
				if (weapon.Tags.Contains((GameTagDef)UsefulDefs[Extras.AllClasses])) {
					weapon.Tags.Remove((GameTagDef)UsefulDefs[Extras.AllClasses]);
    				weapon.Tags.Add((GameTagDef)UsefulDefs[Extras.SniperTag]); 
    				weapon.Tags.Add((GameTagDef)UsefulDefs[Extras.BerserkerTag]); 
				}
			}
			Logger.LogInfo("Mod Disabled");	
		}

		/// Callback for when any property from mod's config is changed.
		public override void OnConfigChanged() {
			/// Config is accessible at any time.
			Logger.LogInfo($"Config Changed");
		}

		/// Useful callback for when level is ending, before unloading.
		/// Usually game cleanup is executed. <param name="level">Level that ends.</param>
		public override void OnLevelEnd(Level level) {
			if (level.name.Contains("Home")) {
				Logger.LogInfo("Exiting Menu");
				// Get and adjust Accuracy perk
				PassiveModifierAbilityDef AccuracyPerk = BonusPistolPerks.Get_BonusPistolAccuracyPerk();
				AccuracyPerk.ItemTagStatModifications[0].EquipmentStatModification.Value = (1+((float)Config.Compensation/100));
				AccuracyPerk.ViewElementDef.Description = new LocalizedTextBind($"Handgun attacks gain {Config.Compensation}% Accuracy", true);
				// Repeat for Damage perk
				PassiveModifierAbilityDef DamagePerk = BonusPistolPerks.Get_BonusPistolDamagePerk();
				DamagePerk.ItemTagStatModifications[0].EquipmentStatModification.Value = (1+((float)Config.Compensation/100));
				DamagePerk.ViewElementDef.Description = new LocalizedTextBind($"Handgun attacks gain {Config.Compensation}% Damage", true);

				Logger.LogInfo("Abilities Updated");
			} 
		}
	}
}
