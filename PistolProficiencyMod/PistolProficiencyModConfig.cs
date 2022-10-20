using PhoenixPoint.Modding;

namespace PistolProficiencyMod
{
	/// ModConfig is mod settings that players can change from within the game.
	/// Config is only editable from players in main menu.
	/// Only one config can exist per mod assembly.
	/// Config is serialized on disk as json.
	public class PistolProficiencyModConfig : ModConfig
	{		
		[ConfigField(text: "% Compensation Multiplier", description: "Adjust the bonus percentage Pistol accuracy/damage given to the Sniper/Berserker (Default: 20%)")]
		public int Compensation = 20; // 20% is the default multiplier
	}
}
