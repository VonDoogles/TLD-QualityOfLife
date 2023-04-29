using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    internal class WidgetUtils
    {
        internal static UISprite? MakeSprite( string Name, Transform parent, Vector3 offset, string SpriteName )
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

        private static UIAtlas? FindAtlas( string AtlasName )
        {
            return InterfaceManager.GetInstance().m_ScalableAtlases.FirstOrDefault( Atlas => Atlas.name == AtlasName );
        }

        internal static void SetActive( Component? Comp, bool Active )
        {
            if ( Comp != null && Comp.gameObject != null && Comp.gameObject.activeSelf != Active )
            {
                Comp.gameObject.SetActive( Active );
            }
        }

        internal static void SetActive( GameObject GameObj, bool Active )
        {
            if ( GameObj != null && GameObj.activeSelf != Active )
            {
                GameObj.SetActive( Active );
            }
        }
    }
}