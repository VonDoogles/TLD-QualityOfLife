using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife.Patches
{
	[HarmonyPatch( typeof( Panel_CustomXPSetup ), "Initialize" )]
	internal class Patch_Panel_CustomXPSetup_Initialize
    {
        static void Postfix( Panel_CustomXPSetup __instance )
		{
			if ( Settings.Instance.EnableMod )
			{
				Transform Parent = __instance.m_ShareExperiencePopupObj.transform;
				Transform ButtonCopy = Parent.FindChild( "ButtonCopy" );
				GenericButtonMouseSpawner ButtonSpawner = Parent.GetComponentInChildren<GenericButtonMouseSpawner>();

				if ( ButtonCopy == null && ButtonSpawner != null )
				{
					var OnCopy = () =>
					{
						if ( __instance != null && __instance.m_ShareExperiencePopupLabel != null )
						{
							string ShareCode = __instance.m_ShareExperiencePopupLabel.text;
							GUIUtility.systemCopyBuffer = ShareCode;
						}
					};

					GameObject NewButton = GameObject.Instantiate( ButtonSpawner.m_Prefab, Parent );
					NewButton.name = "ButtonCopy";
					NewButton.transform.localPosition = new Vector3( 0, -104, 0 );
					NewButton.transform.localScale = Vector3.one;

					UILabel Label = NewButton.GetComponentInChildren<UILabel>();
					if ( Label != null )
					{
						Label.fontSize = 18;
					}

					UILocalize Localize = NewButton.GetComponentInChildren<UILocalize>();
					if ( Localize != null )
					{
						Localize.key = "Copy";
						Localize.OnLocalize();
					}

					UIButton Button = NewButton.GetComponent<UIButton>();
					if ( Button != null )
					{
						Button.onClick.Add( new EventDelegate( OnCopy ) );
					}

					BoxCollider Box = NewButton.GetComponent<BoxCollider>();
					if ( Box != null )
					{
						Box.size = new Vector3( 96, 30, 0 );
					}

					Transform Background = NewButton.transform.FindChild( "Background" );
					if ( Background != null )
					{
						Background.GetComponentInChildren<UISprite>().SetDimensions( 96, 38 );
					}

					Transform BackGlow = NewButton.transform.FindChild( "BackGlow" );
					if ( BackGlow != null )
					{
						BackGlow.GetComponentInChildren<UISprite>().SetDimensions( 96 + 16, 83 );
					}
				}
			}
		}
    }
}
