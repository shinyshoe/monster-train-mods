using BepInEx;
using HarmonyLib;
using ShinyShoe;
using UnityEngine;
using System.IO;

namespace OnlyStewards
{
    [BepInPlugin("com.shinyshoe.onlystewards", "OnlyStewards", "1.0.0.0")]
    public class OnlyStewards : BaseUnityPlugin
    {
        public static string SpriteFilePath;

        void Awake()
        {
            var directory = Path.GetDirectoryName(Info.Location);
            SpriteFilePath = Path.Combine(directory, "face.png");

            var harmony = new Harmony("com.shinyshoe.onlystewards");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(CharacterData))]
    [HarmonyPatch("GetName")]
    public static class ChangeFriendCharacterName
    {
        static void Postfix(ref string __result)
        {
            if (__result == "Train Steward") __result = "Shiny Steward";
        }
    }

    [HarmonyPatch(typeof(CardState))]
    [HarmonyPatch("GetTitle")]
    public static class ChangeFriendCardName
    {
        static void Postfix(ref string __result)
        {
            if (__result == "Train Steward") __result = "Shiny Steward";
        }
    }

    [HarmonyPatch(typeof(CharacterUI))]
    [HarmonyPatch("Setup")]
    public static class Mod_CharacterUI_Setup
    {
        static void Postfix(ref string ___debugName, ref CharacterUIMeshBase ____characterMesh, ref CharacterState characterState)
        {
            if (___debugName.StartsWith("Character_TrainSteward"))
            {
                GameObject faceTexture = CreateFaceObject();
                (____characterMesh as CharacterUIMeshSpine).OrNull()?.AttachToBone(faceTexture.transform, VfxAtLoc.Location.BoneStatusEffectSlot1);
                faceTexture.transform.localPosition += new Vector3(-.25f, -.6f);
                faceTexture.transform.localScale = new Vector3(.5f, .5f);
            }
        }

        static GameObject CreateFaceObject()
        {
            GameObject go = new GameObject("Face");
            SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = LoadNewSprite(OnlyStewards.SpriteFilePath);
            spriteRenderer.sortingOrder -= 1;

            return go;
        }

        static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
        {
            // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
            Texture2D SpriteTexture = LoadTexture(FilePath);
            Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

            return NewSprite;
        }

        public static Texture2D LoadTexture(string FilePath)
        {

            // Load a PNG or JPG file from disk to a Texture2D
            // Returns null if load fails

            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);      
                Tex2D.LoadImage(FileData);

                return Tex2D;                 // If data = readable -> return texture
            }
            return null;                     // Return null if load failed
        }
    }
}
