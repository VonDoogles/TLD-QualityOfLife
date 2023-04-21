using MelonLoader;

namespace QualityOfLife;
internal sealed class QualityOfLifeMod : MelonMod
{
	public override void OnInitializeMelon()
	{
		Settings.OnLoad();
	}
}
