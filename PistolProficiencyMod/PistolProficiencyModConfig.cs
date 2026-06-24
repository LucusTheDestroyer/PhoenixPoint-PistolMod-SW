using PhoenixPoint.Modding;

namespace PistolProficiencyMod
{
	/// ModConfig is mod settings that players can change from within the game.
	/// Config is only editable from players in main menu.
	/// Only one config can exist per mod assembly.
	/// Config is serialized on disk as json.
	public class PistolProficiencyModConfig : ModConfig
	{		
		[ConfigField(text: "Sniper Accuracy Bonus", description: "Adjusts the bonus percentage Handgun accuracy given to the Sniper (Default: 20%)")]
		public int SniperBonus = 20; // 20% is the default multiplier

		[ConfigField(text: "Berserker Damage Bonus", description: "Adjusts the bonus percentage Handgun damage given to the Berserker (Default: 20%)")]
		public int BerserkerBonus = 20; // 20% is the default multiplier
	}
}
