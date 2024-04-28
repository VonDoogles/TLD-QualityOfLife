using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using UnityEngine;

namespace QualityOfLife
{
    [RegisterTypeInIl2Cpp]
    public class StatusBarWind : MonoBehaviour
    {
        public UISprite? Icon;
        public UISprite? Backsplash;
        public UISprite? CircleBG;

        public UILabel? FeelsLikeLabel;

        public UILabel? WindLabel;

        public Transform? Arrow1;
        public Transform? Arrow2;
        public Transform? Arrow3;
        public UISprite? Arrow1Sprite;
        public UISprite? Arrow2Sprite;
        public UISprite? Arrow3Sprite;

        public Color TemperatureNegativeColor = new Color( 0.64f, 0.0f, 0.0f, 1.0f );

        public float AngleBetweenArrows = 25.0f;
        public Color ArrowGustColor = new Color( 0.0f, 0.8f, 1.0f, 1.0f );

        public string IconNameSheltered = "ico_sheltered";
        public string IconNameWind = "ico_clothingStats_wind";

        public Vector3 IconScaleSheltered = new Vector3( 0.95f, 0.95f, 0.95f );
        public Vector3 IconScaleWind = new Vector3( 1.1f, 1.1f, 1.1f );

        public StatusBarWind( IntPtr ptr ) : base( ptr ) {}
        public StatusBarWind() : base( ClassInjector.DerivedConstructorPointer<StatusBarWind>() ) => ClassInjector.DerivedConstructorBody( this );

        public void Awake()
        {
            transform.localPosition = new Vector3( 80, 0, 0 );
            transform.localScale = Vector3.one;

            GenericStatusBarSpawner Spawner = GetComponentInParent<GenericStatusBarSpawner>();
            if ( Spawner != null )
            {
                StatusBar CopyBar = Spawner.m_SpawnedStatusBar;
                if ( CopyBar != null )
                {
                    Backsplash = Instantiate( CopyBar.transform.FindChild( "Root/BacksplashDarken" ), transform )?.GetComponent<UISprite>();
                    if ( Backsplash != null )
                    {
                        Backsplash.transform.localPosition = Vector3.zero;
                        Backsplash.transform.localScale = Vector3.one;
                    }
                    
                    CircleBG = Instantiate( CopyBar.transform.FindChild( "Root/Background2" ), transform )?.GetComponent<UISprite>();
                    if ( CircleBG != null )
                    {
                        CircleBG.transform.localPosition = Vector3.zero;
                        CircleBG.transform.localScale = Vector3.one;
                    }

                    Icon = Instantiate( CopyBar.transform.FindChild( "Root/Background" ), transform )?.GetComponent<UISprite>();

                    if ( Icon != null )
                    {
                        Icon.transform.localPosition = Vector3.zero;
                        Icon.transform.localScale = Vector3.one;
                        Icon.spriteName = IconNameWind;
                        Icon.MarkAsChanged();
                    }

                    Arrow1 = Instantiate( CopyBar.transform.FindChild( "Root/DownArrow1" ), transform );
                    if ( Arrow1 != null && Arrow1.transform.childCount > 0 )
                    {
                        Arrow1.gameObject.name = "Arrow1";
                        Arrow1.transform.localPosition = Vector3.zero;
                        Arrow1.transform.localScale = Vector3.one;
                        Arrow1.transform.GetChild(0).localPosition = new Vector3( 0, 24, 0 );
                        Arrow1Sprite = Arrow1.transform.GetChild( 0 ).GetComponent<UISprite>();
                        WidgetUtils.SetActive( Arrow1, true );
                    }

                    Arrow2 = Instantiate( Arrow1, transform );
                    if ( Arrow2 != null )
                    {
                        Arrow2.gameObject.name = "Arrow2";
                        Arrow2.transform.localPosition = Vector3.zero;
                        Arrow2.transform.localScale = Vector3.one;
                        Arrow2Sprite = Arrow2.transform.GetChild( 0 ).GetComponent<UISprite>();
                    }

                    Arrow3 = Instantiate( Arrow1, transform );
                    if ( Arrow3 != null )
                    {
                        Arrow3.gameObject.name = "Arrow3";
                        Arrow3.transform.localPosition = Vector3.zero;
                        Arrow3.transform.localScale = Vector3.one;
                        Arrow3Sprite = Arrow3.transform.GetChild( 0 ).GetComponent<UISprite>();
                    }
                }
            }

            Panel_Inventory PanelInv = InterfaceManager.GetPanel<Panel_Inventory>();
            if ( PanelInv != null && PanelInv.m_CategoryWeightLabel != null && PanelInv.m_CategoryWeightBG != null )
            {
                FeelsLikeLabel = Instantiate( PanelInv.m_CategoryWeightLabel, transform );
                if ( FeelsLikeLabel != null )
                {
                    FeelsLikeLabel.transform.localPosition = new Vector3( -320, 32, 0 );;
                    FeelsLikeLabel.transform.localScale = Vector3.one;
                    FeelsLikeLabel.depth = 120;
                    FeelsLikeLabel.effectColor = new Color( 0, 0, 0, 0.5f );
                    FeelsLikeLabel.effectDistance = new Vector2( 0.5f, 0.5f );
                    FeelsLikeLabel.effectStyle = UILabel.Effect.Shadow;
                    FeelsLikeLabel.fontSize = 16;
                    FeelsLikeLabel.text = "";
                }

                WindLabel = Instantiate( PanelInv.m_CategoryWeightLabel, transform );
                if ( WindLabel != null )
                {
                    WindLabel.transform.localPosition = new Vector3( 0, 32, 0 );
                    WindLabel.transform.localScale = Vector3.one;
                    WindLabel.depth = 120;
                    WindLabel.effectColor = new Color( 0, 0, 0, 0.5f );
                    WindLabel.effectDistance = new Vector2( 0.5f, 0.5f );
                    WindLabel.effectStyle = UILabel.Effect.Shadow;
                    WindLabel.fontSize = 16;
                    WindLabel.text = "";
                }
            }
        }

        public void Update()
        {
            Wind WindComp = GameManager.GetWindComponent();
            if ( WindComp != null )
            {
                Color ArrowColor = Color.white;
                float WindAngle = -WindComp.GetWindAngleRelativeToPlayer() + 180.0f;
                float WindSpeed = WindComp.GetSpeedMPH();
                float Offset = 0.0f;

                int NumArrows = 1;

                Weather WeatherComp = GameManager.GetWeatherComponent();
                if ( FeelsLikeLabel != null && WeatherComp != null )
                {
                    float FeelsLike = WeatherComp.m_PrevBodyTemp;
                    FeelsLikeLabel.color = FeelsLike >= 0.0f ? Color.white : TemperatureNegativeColor;
                    FeelsLikeLabel.text = Utils.GetTemperatureString( FeelsLike, false, true, false );
                    WidgetUtils.SetActive( FeelsLikeLabel, Settings.Instance.ShowTemperatureLabels );
                }

                if ( WindLabel != null )
                {
                    float WindChill = WindComp.GetWindChill();
                    WindLabel.color = WindChill >= 0.0f ? Color.white : TemperatureNegativeColor;
                    WindLabel.text = Utils.GetTemperatureString( WindChill, false, true, false );
                    WidgetUtils.SetActive( WindLabel, Settings.Instance.ShowTemperatureLabels && Settings.Instance.WindStatusBar != WindStatusType.None );
                }

                if ( Settings.Instance.WindStatusBar == WindStatusType.DirectionAndSpeed )
                {
                    ArrowColor = WindSpeed >= 40.0f ? ArrowGustColor : Color.white;
                    NumArrows = Mathf.Clamp( Mathf.FloorToInt( WindSpeed * 0.1f ), 1, 3 );

                    if ( NumArrows == 2 )
                    {
                        Offset = AngleBetweenArrows * 0.5f;
                    }
                }

                bool bSheltered = WindComp.PlayerShelteredFromWind();
                string WindIconName = bSheltered ? IconNameSheltered : IconNameWind;
                if ( Icon != null && Icon.spriteName != WindIconName )
                {
                    Icon.spriteName = WindIconName;
                    Icon.transform.localScale = bSheltered ? IconScaleSheltered : IconScaleWind;
                    Icon.MarkAsChanged();
                }

                if ( WeatherComp != null && WeatherComp.IsIndoorEnvironment() )
                {
                    NumArrows = 0;
                }

                if ( Settings.Instance.WindStatusBar == WindStatusType.None || Settings.Instance.WindStatusBar == WindStatusType.ShelterOnly )
                {
                    NumArrows = 0;
                }

                WidgetUtils.SetActive( Backsplash, Settings.Instance.WindStatusBar != WindStatusType.None );
                WidgetUtils.SetActive( CircleBG, Settings.Instance.WindStatusBar != WindStatusType.None );
                WidgetUtils.SetActive( Icon, Settings.Instance.WindStatusBar != WindStatusType.None );

                if ( Arrow1 != null && Arrow1Sprite != null )
                {
                    Arrow1Sprite.color = ArrowColor;
                    Arrow1.localRotation = Quaternion.Euler( 0, 0, WindAngle + Offset );
                    WidgetUtils.SetActive( Arrow1, NumArrows >= 1 );
                }

                if ( Arrow2 != null && Arrow2Sprite != null )
                {
                    Arrow2Sprite.color = ArrowColor;
                    Arrow2.localRotation = Quaternion.Euler( 0, 0, WindAngle - AngleBetweenArrows + Offset );
                    WidgetUtils.SetActive( Arrow2, NumArrows >= 2 );
                }

                if ( Arrow3 != null && Arrow3Sprite != null )
                {
                    Arrow3Sprite.color = ArrowColor;
                    Arrow3.localRotation = Quaternion.Euler( 0, 0, WindAngle + AngleBetweenArrows + Offset );
                    WidgetUtils.SetActive( Arrow3, NumArrows >= 3 );
                }
            }
        }
    }
}
