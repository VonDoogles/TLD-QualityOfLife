using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace QualityOfLife
{
    [RegisterTypeInIl2Cpp]
    public class WeightLabel : MonoBehaviour
    {
        public UILabel? Label;
        public Vector3 LabelOffset = new Vector3( -30.0f, 16.0f, 0.0f );

        private UISprite? SpriteCapacity;
        private UISprite? SpriteEncumber;

        public void Awake()
        {
            transform.localPosition = LabelOffset;
            transform.localScale = Vector3.one;

            Label = GetComponent<UILabel>();
			Label.effectColor = new Color( 0, 0, 0, 0.75f );
			Label.effectDistance = new Vector2( 0.5f, 0.5f );
			Label.effectStyle = UILabel.Effect.Shadow;

            Panel_HUD PanelHUD = InterfaceManager.GetPanel<Panel_HUD>();
            if ( PanelHUD != null )
            {
                SpriteCapacity = PanelHUD.m_Sprite_CapacityBuff;
                SpriteEncumber = PanelHUD.m_Sprite_Encumbered;
            }
        }

        public void Update()
        {
            float Alpha = 0.0f;

            if ( Settings.Instance.WeightLabel == WeightLabelType.AutoHide )
            {
                Panel_Actions PanelActions = InterfaceManager.GetPanel<Panel_Actions>();
                if ( PanelActions != null )
                {
                    Alpha = PanelActions.GetPanelAlpha();
                }

                if ( SpriteCapacity != null && SpriteCapacity.enabled )
                {
                    Alpha = Math.Max( Alpha, SpriteCapacity.alpha );
                }

                if ( SpriteEncumber != null && SpriteEncumber.enabled )
                {
                    Alpha = Math.Max( Alpha, SpriteEncumber.alpha );
                }
            }
            else if ( Settings.Instance.WeightLabel == WeightLabelType.AlwaysOn )
            {
                Alpha = 1.0f;
            }

            if ( Label != null )
            {
                Label.alpha = Alpha;
                Label.enabled = Alpha > 0.0f;

                if ( Label.enabled )
                {
                    Encumber EncumberComp = GameManager.GetEncumberComponent();
                    if ( EncumberComp != null )
                    {
                        string Text = EncumberComp.GetPlayerCarryCapacityString();
                        if ( Label.text != Text )
                        {
                            Label.text = Text;
                            Label.ProcessText();
                            transform.localPosition = LabelOffset - new Vector3( Label.lineWidth, 0.0f, 0.0f );
                        }
                    }
                }
            }
        }
    }
}
