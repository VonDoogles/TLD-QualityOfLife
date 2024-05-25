using Il2Cpp;
using Il2CppVLB;
using MelonLoader;
using UnityEngine;

namespace QualityOfLife
{
    [RegisterTypeInIl2Cpp]
    public class TravoisContentItem : MonoBehaviour
    {
		public GearItem? Travois = null;

		public static void SetTravois( GearItem? InnerItem, GearItem? Travois )
		{
			TravoisContentItem? ContentItem = ( Travois != null
													? InnerItem?.GetOrAddComponent<TravoisContentItem>()
													: InnerItem?.GetComponent<TravoisContentItem>() );
            if ( ContentItem != null )
			{
				ContentItem.Travois = Travois;
			}
		}
    }
}