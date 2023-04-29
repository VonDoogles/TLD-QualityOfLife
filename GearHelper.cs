using Il2Cpp;

namespace QualityOfLife
{
    internal class GearHelper
    {
        public static int CompareGearItemByHP( GearItem A, GearItem B )
        {
            if ( A.CurrentHP != B.CurrentHP )
            {
                return A.CurrentHP < B.CurrentHP ? -1 : 1;
            }
            return 0;
        }

        public static int CompareGearItemByHeatAndHP( GearItem A, GearItem B )
        {
            if ( A.m_FoodItem != null && B.m_FoodItem != null )
            {
                if ( A.m_FoodItem.m_HeatPercent != B.m_FoodItem.m_HeatPercent )
                {
                    return A.m_FoodItem.m_HeatPercent > B.m_FoodItem.m_HeatPercent ? -1 : 1;
                }
                if ( A.m_FoodItem.m_CaloriesRemaining != B.m_FoodItem.m_CaloriesRemaining )
                {
                    return A.m_FoodItem.m_CaloriesRemaining < B.m_FoodItem.m_CaloriesRemaining ? -1 : 1;
                }
                return CompareGearItemByHP( A, B );
            }
            return CompareGearItemByHP( A, B );
        }

        public static void Swap( Il2CppSystem.Collections.Generic.List<GearItem> ItemList, int IndexA, int IndexB )
        {
            GearItem Temp = ItemList[ IndexA ];
            ItemList[ IndexA ] = ItemList[ IndexB ];
            ItemList[ IndexB ] = Temp;
        }

        public static int Partition( Il2CppSystem.Collections.Generic.List<GearItem> ItemList, int Low, int High, Func<GearItem, GearItem, int> CompareFunc )
        {
            GearItem Pivot = ItemList[ High ];
            int PartitionIndex = Low - 1;

            for ( int Index = Low; Index < High; ++Index )
            {
                if ( CompareFunc( ItemList[ Index ], Pivot ) < 0 )
                {
                    PartitionIndex++;
                    Swap( ItemList, PartitionIndex, Index );
                }
            }

            Swap( ItemList, PartitionIndex + 1, High );
            return PartitionIndex + 1;
        }

        public static void QuickSort( Il2CppSystem.Collections.Generic.List<GearItem> ItemList, int Low, int High, Func<GearItem, GearItem, int> CompareFunc )
        {
            if ( Low < High )
            {
                int PartitionIndex = Partition( ItemList, Low, High, CompareFunc );
                QuickSort( ItemList, Low, PartitionIndex - 1, CompareFunc );
                QuickSort( ItemList, PartitionIndex + 1, High, CompareFunc );
            }
        }

        public static void Sort( Il2CppSystem.Collections.Generic.List<GearItem> ItemList, Func<GearItem, GearItem, int> CompareFunc )
        {
            QuickSort( ItemList, 0, ItemList.Count - 1, CompareFunc );
        }

        public static void GroupItemsByType( Il2CppSystem.Collections.Generic.List<GearItem> __result )
        {
            Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Collections.Generic.List<GearItem>> ItemByType = new();
            foreach ( GearItem Item in __result )
            {
                if ( !ItemByType.ContainsKey( Item.name ) )
                {
                    ItemByType.Add( Item.name, new() );
                }
                ItemByType[ Item.name ].Add( Item );
            }

            __result.Clear();

            foreach ( var Pair in ItemByType )
            {
                var ItemList = Pair.Value;
                if ( ItemList.Count > 0 )
                {
                    Sort( ItemList, CompareGearItemByHeatAndHP );
                    __result.Add( ItemList[ 0 ] );
                }
            }
        }
    }

}
