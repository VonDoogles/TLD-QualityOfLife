using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    public class WidgetUtils
    {
        public static UISprite? MakeSprite( string Name, Transform parent, Vector3 offset, string SpriteName )
        {
            GameObject GameObj = new GameObject( Name );
            if ( GameObj != null )
            {
                GameObj.transform.parent = parent;
                GameObj.transform.localPosition = offset;

                UISprite Sprite = GameObj.AddComponent<UISprite>();
                Sprite.spriteName = SpriteName;
                Sprite.atlas = FindAtlas( "Base Atlas" );
                return Sprite;
            }
            return null;
        }

        public static UIAtlas? FindAtlas( string AtlasName )
        {
            return InterfaceManager.GetInstance().m_ScalableAtlases.FirstOrDefault( Atlas => Atlas.name == AtlasName );
        }

        public static void SetActive( Component? Comp, bool Active )
        {
            if ( Comp != null && Comp.gameObject != null && Comp.gameObject.activeSelf != Active )
            {
                Comp.gameObject.SetActive( Active );
            }
        }

        public static void SetActive( GameObject GameObj, bool Active )
        {
            if ( GameObj != null && GameObj.activeSelf != Active )
            {
                GameObj.SetActive( Active );
            }
        }

        public static bool UIRectContains( UIRect Rect, Vector3 MouseLoc )
        {
            Vector3 Min = Rect.anchorCamera.WorldToScreenPoint( Rect.worldCorners[ 0 ] );
            Vector3 Max = Min;

            foreach ( Vector3 Corner in Rect.worldCorners )
            {
                Vector3 ScreenCorner = Rect.anchorCamera.WorldToScreenPoint( Corner );
                Min.x = Math.Min( Min.x, ScreenCorner.x );
                Min.y = Math.Min( Min.y, ScreenCorner.y );
                Max.x = Math.Max( Max.x, ScreenCorner.x );
                Max.y = Math.Max( Max.y, ScreenCorner.y );
            }

            Rect ScreenRect = UnityEngine.Rect.MinMaxRect( Min.x, Min.y, Max.x, Max.y );
            return ScreenRect.Contains( MouseLoc );
        }
    }
}