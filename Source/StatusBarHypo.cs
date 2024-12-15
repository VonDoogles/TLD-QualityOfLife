using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using UnityEngine;

namespace QualityOfLife
{
    [RegisterTypeInIl2Cpp]
    public class StatusBarHypo : MonoBehaviour
    {
        public UISprite? Icon;

		public StatusBar? Status;

        public Color TemperatureNegativeColor = new Color( 0.64f, 0.0f, 0.0f, 1.0f );


		public void Awake()
		{
			Icon = GetComponent<UISprite>();
			Status = GetComponentInParent<StatusBar>();
		}

		public void Update()
		{
			if ( Settings.Instance.HypoStatusBar )
			{
				if ( Icon != null )
				{
					Hypothermia Hypo = GameManager.GetHypothermiaComponent();
					bool bShowHypo = Status != null && Status.m_BacksplashDepleted != null && Status.m_BacksplashDepleted.activeInHierarchy;

                    if ( Hypo != null && bShowHypo )
					{
						Icon.color = TemperatureNegativeColor;
						Icon.fillAmount = Hypo.GetHypothermiaRiskValue();
					}
				}
			}
		}
    }
}
