using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_PickUnits ), "Update" )]
    internal class Patch_Panel_PickUnits_Update
    {
        static void Postfix( Panel_PickUnits __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnExecute();
                }

                if ( ModInput.GetKeyDown( __instance, Settings.Instance.EquipKey ) )
                {
                    __instance.OnExecuteAll();
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_PickUnits ), "Refresh" )]
    internal class Patch_Panel_PickUnits_Refresh
    {
        static void Postfix( Panel_PickUnits __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.FoodEatPickUnits )
            {
                if ( __instance.m_GearItem != null )
                {
                    if ( __instance.m_GearItem.m_StackableItem != null && __instance.m_GearItem.m_StackableItem.m_Units > 1 && __instance.m_ExecuteAction == PickUnitsExecuteAction.Harvest )
                    {
                        Panel_PickUnits PickUnits = InterfaceManager.GetPanel<Panel_PickUnits>();
                        if ( PickUnits != null )
                        {
                            PickUnits.m_Label_Description.text = PickUnits.m_Label_Description.text.Replace( "harvest", "eat" );
                            PickUnits.m_Label_Description.ProcessText();

                            UILabel LabelExecute = PickUnits.m_Execute_Button.GetComponentInChildren<UILabel>();
                            if ( LabelExecute != null )
                            {
                                LabelExecute.text = "Eat";
                                LabelExecute.ProcessText();
                            }

                            UILabel LabelExecuteAll = PickUnits.m_ExecuteAll_Button.GetComponentInChildren<UILabel>();
                            if ( LabelExecuteAll != null )
                            {
                                LabelExecuteAll.text = "Eat All";
                                LabelExecuteAll.ProcessText();
                            }
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_PickUnits ), "OnExecute" )]
    internal class Patch_Panel_PickUnits_OnExecute
    {
        static bool Prefix( Panel_PickUnits __instance )
        {
            if ( Settings.Instance.EnableMod && __instance.m_GearItem != null )
            {
                if ( Settings.Instance.FoodEatPickUnits && __instance.m_GearItem.m_StackableItem != null && __instance.m_GearItem.m_StackableItem.m_Units > 1 && __instance.m_ExecuteAction == PickUnitsExecuteAction.Harvest )
                {
                    int EatUnits = __instance.m_numUnits;
                    float TempCalories = __instance.m_GearItem.m_FoodItem.m_CaloriesTotal * EatUnits;
                    __instance.m_GearItem.m_FoodItem.m_CaloriesTotal = __instance.m_GearItem.m_FoodItem.m_CaloriesRemaining = TempCalories;

                    PlayerManager PlayerMan = GameManager.GetPlayerManagerComponent();
                    PlayerMan.m_FoodItemEaten = __instance.m_GearItem;
                    PlayerMan.m_FoodItemEatenStartingCalories = TempCalories;
                    PlayerMan.m_FoodItemEatenStartingWeight = __instance.m_GearItem.GetSingleItemWeightKG() * EatUnits;

                    Hunger HungerMan = GameManager.GetHungerComponent();
                    HungerMan.AddReserveCaloriesOverTime( __instance.m_GearItem.m_FoodItem, TempCalories, __instance.m_GearItem.m_FoodItem.m_TimeToEatSeconds );

                    Panel_GenericProgressBar GenericProgress = InterfaceManager.GetPanel<Panel_GenericProgressBar>();
                    if ( GenericProgress != null )
                    {
                        OnExitDelegate OnExit = new Action<bool, bool, float>( ( success, playerCancel, progress ) =>
                        {
                            PlayerManager PlayerMan = GameManager.GetPlayerManagerComponent();
                            if ( PlayerMan != null )
                            {
                                PlayerMan.OnEatingComplete( success, playerCancel, progress );
                            }
                        } );
                        GenericProgress.Launch( "Eating...", __instance.m_GearItem.m_FoodItem.m_TimeToEatSeconds, 0.0f, 0.0f, false, OnExit );

                        switch ( __instance.m_EnablePanelOnExit )
                        {
                            case EnablePanelOnExit.Inventory:
                                GenericProgress.m_Slider.transform.parent = GenericProgress.m_GearItemLocation;
                                GenericProgress.m_Slider.transform.localPosition = Vector3.zero;
                                break;

                            default:
                                GenericProgress.m_Slider.transform.parent = GenericProgress.m_CenterLocation;
                                GenericProgress.m_Slider.transform.localPosition = Vector3.zero;
                                break;
                        }
                    }

                    __instance.ExitInterface();
                    return false;
                }
                else if ( Settings.Instance.FireCharcoalPickUnits && __instance.m_GearItem.name == "GEAR_Charcoal" && __instance.m_ExecuteAction == PickUnitsExecuteAction.Harvest )
                {
                    Panel_ActionPicker Picker = InterfaceManager.GetPanel<Panel_ActionPicker>();
                    if ( Picker != null )
                    {
                        GameObject InteractObj = Picker.GetObjectInteractedWith();
                        if ( InteractObj != null )
                        {
                            FireplaceInteraction Interaction = InteractObj.GetComponent<FireplaceInteraction>();
                            if ( Interaction != null && Interaction.Fire != null )
                            {
                                int WantAmount = __instance.m_numUnits;

                                if ( WantAmount > 0 && Interaction.Fire.m_NumStartingCharcoalPieces > 0 )
                                {
                                    int Taken = Math.Min( WantAmount, Interaction.Fire.m_NumStartingCharcoalPieces );
                                    Interaction.Fire.m_NumStartingCharcoalPieces -= Taken;
                                    WantAmount -= Taken;
                                }

                                if ( WantAmount > 0 && Interaction.Fire.m_NumGeneratedCharcoalPieces > 0 )
                                {
                                    int Taken = Mathf.Min( WantAmount, Interaction.Fire.m_NumGeneratedCharcoalPieces );
                                    Interaction.Fire.m_NumGeneratedCharcoalPieces -= Taken;
                                    WantAmount -= Taken;
                                }

                                int Amount = __instance.m_numUnits - WantAmount;
                                GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory( __instance.m_GearItem, Amount, 1.0f );
                                GearMessage.AddMessageFadeIn( "GEAR_Charcoal", "Harvested", string.Format( "{0} ({1})", __instance.m_GearItem.DisplayName, Amount ), 4.0f );
                                __instance.m_GearItem.PlayPickUpClip();
                                __instance.ExitInterface();
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }

    [HarmonyPatch( typeof( Panel_PickUnits ), "OnExecuteAll" )]
    internal class Patch_Panel_PickUnits_OnExecuteAll
    {
        static bool Prefix( Panel_PickUnits __instance )
        {
            if ( Settings.Instance.EnableMod && __instance.m_GearItem != null )
            {
                if ( Settings.Instance.FoodEatPickUnits && __instance.m_GearItem.m_StackableItem != null && __instance.m_GearItem.m_StackableItem.m_Units > 1 && __instance.m_ExecuteAction == PickUnitsExecuteAction.Harvest )
                {
                    int EatUnits = __instance.m_maxUnits;
                    float TempCalories = __instance.m_GearItem.m_FoodItem.m_CaloriesTotal * EatUnits;
                    __instance.m_GearItem.m_FoodItem.m_CaloriesTotal = __instance.m_GearItem.m_FoodItem.m_CaloriesRemaining = TempCalories;

                    PlayerManager PlayerMan = GameManager.GetPlayerManagerComponent();
                    PlayerMan.m_FoodItemEaten = __instance.m_GearItem;
                    PlayerMan.m_FoodItemEatenStartingCalories = TempCalories;
                    PlayerMan.m_FoodItemEatenStartingWeight = __instance.m_GearItem.GetSingleItemWeightKG() * EatUnits;

                    Hunger HungerMan = GameManager.GetHungerComponent();
                    HungerMan.AddReserveCaloriesOverTime( __instance.m_GearItem.m_FoodItem, TempCalories, __instance.m_GearItem.m_FoodItem.m_TimeToEatSeconds );

                    Panel_GenericProgressBar GenericProgress = InterfaceManager.GetPanel<Panel_GenericProgressBar>();
                    if ( GenericProgress != null )
                    {
                        OnExitDelegate OnExit = new Action<bool, bool, float>( ( success, playerCancel, progress ) =>
                        {
                            PlayerManager PlayerMan = GameManager.GetPlayerManagerComponent();
                            if ( PlayerMan != null )
                            {
                                PlayerMan.OnEatingComplete( success, playerCancel, progress );
                            }
                        } );
                        GenericProgress.m_Slider.transform.parent = GenericProgress.m_GearItemLocation.transform;
                        GenericProgress.m_Slider.transform.localPosition = Vector3.zero;
                        GenericProgress.Launch( "Eating...", __instance.m_GearItem.m_FoodItem.m_TimeToEatSeconds, 0.0f, 0.0f, false, OnExit );
                    }

                    __instance.ExitInterface();
                    return false;
                }
                else if ( Settings.Instance.FireCharcoalPickUnits && __instance.m_GearItem.name == "GEAR_Charcoal" && __instance.m_ExecuteAction == PickUnitsExecuteAction.Harvest )
                {
                    Panel_ActionPicker Picker = InterfaceManager.GetPanel<Panel_ActionPicker>();
                    if ( Picker != null )
                    {
                        GameObject InteractObj = Picker.GetObjectInteractedWith();
                        if ( InteractObj != null )
                        {
                            FireplaceInteraction Interaction = InteractObj.GetComponent<FireplaceInteraction>();
                            if ( Interaction != null && Interaction.Fire != null )
                            {
                                Interaction.Fire.RemoveAllCharcoal();
                                int Amount = __instance.m_maxUnits;
                                GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory( __instance.m_GearItem, Amount, 1.0f );
                                GearMessage.AddMessageFadeIn( "GEAR_Charcoal", "Harvested", string.Format( "{0} ({1})", __instance.m_GearItem.DisplayName, Amount ), 4.0f );
                                __instance.m_GearItem.PlayPickUpClip();
                                __instance.ExitInterface();
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }

}
