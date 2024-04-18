using MelonLoader;
using System.Reflection;

//This is a C# comment. Comments have no impact on compilation.

[assembly: AssemblyTitle(BuildInfo.ModName)]
[assembly: AssemblyCopyright($"Created by VonDoogles")]

[assembly: AssemblyVersion(BuildInfo.ModVersion)]
[assembly: AssemblyFileVersion(BuildInfo.ModVersion)]
[assembly: MelonInfo(typeof(QualityOfLife.QualityOfLifeMod), BuildInfo.ModName, BuildInfo.ModVersion, BuildInfo.ModAuthor)]

//This tells MelonLoader that the mod is only for The Long Dark.
[assembly: MelonGame("Hinterland", "TheLongDark")]

internal static class BuildInfo
{
	internal const string ModName = "Quality of Life";
	internal const string ModAuthor = "VonDoogles";
	/// <summary>
	/// Version numbers in C# are a set of 1 to 4 positive integers separated by periods.
	/// Mods typically use 3 numbers. For example: 1.2.1
	/// </summary>
	internal const string ModVersion = "1.10.0";
}