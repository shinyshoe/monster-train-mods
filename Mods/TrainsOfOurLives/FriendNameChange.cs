using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;

namespace HellHornedFriends
{
    [BepInPlugin("com.shinyshoe.mtsoap", "Trains of Our Lives", "1.0.0.0")]
    public class FriendNameChange : BaseUnityPlugin
    {
        void Awake()
        {
            var harmony = new Harmony("com.shinyshoe.mtsoap");
            harmony.PatchAll();
        }
    }
}
