using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppTLD.BigCarry;
using Il2CppTLD.Interactions;
using MelonLoader;

namespace QualityOfLife
{
    [RegisterTypeInIl2Cpp]
    public class TravoisContainerInteraction : SimpleInteraction
    {
        public TravoisContainerInteraction( IntPtr ptr ) : base( ptr ) { }
        public TravoisContainerInteraction() : base( ClassInjector.DerivedConstructorPointer<TravoisContainerInteraction>() ) => ClassInjector.DerivedConstructorBody( this );

        public override void InitializeInteraction()
        {
            CanInteract = true;
            HoverText = "Examine Travois";
            m_EventEntries = new Il2CppSystem.Collections.Generic.List<InteractionEventEntry>();
        }

        public override void UpdateInteraction()
        {
			if ( enabled && ( !Settings.Instance.EnableMod || !Settings.Instance.TravoisPickupWithContents ) )
			{
				ContainerInteraction Interaction = GetComponent<ContainerInteraction>();
				if ( Interaction != null )
				{
					Interaction.enabled = true;
				}
				enabled = false;
			}
        }

        public override bool PerformInteraction()
        {
			TravoisBigCarryItem Travois = gameObject.GetComponentInParent<TravoisBigCarryItem>();
			if ( Travois != null )
			{
				Travois.PickupCallback();
			}
            return true;
        }
    }
}
