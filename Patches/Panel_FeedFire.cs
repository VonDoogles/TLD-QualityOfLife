﻿using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_FeedFire ), "Update" )]
    internal class Patch_Panel_FeedFire_Update
    {
        static void Postfix( Panel_FeedFire __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    __instance.OnFeedFire();
                }
            }
        }
    }
}
