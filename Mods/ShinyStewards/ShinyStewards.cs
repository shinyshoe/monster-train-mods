using BepInEx;
using HarmonyLib;
using ShinyShoe;
using UnityEngine;
using System.IO;

namespace ShinyStewards
{
    [BepInPlugin("com.shinyshoe.shinystewards", "Shiny Stewards", "1.0.0.0")]
    public class ShinyStewards : BaseUnityPlugin
    {
        public static string[] SpriteFilePaths;

        void Awake()
        {
            var directory = Path.Combine(Path.GetDirectoryName(Info.Location), "images");
            SpriteFilePaths = Directory.GetFiles(directory);

            var harmony = new Harmony("com.shinyshoe.shinystewards");
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

        private static GameObject CreateFaceObject()
        {
            GameObject go = new GameObject("Face");
            SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();

            string path = ShinyStewards.SpriteFilePaths[RandomManager.Range(0, ShinyStewards.SpriteFilePaths.Length, RngId.NonDeterministic)];
            spriteRenderer.sprite = LoadNewSprite(path);
            spriteRenderer.sortingOrder -= 1;

            return go;
        }

        private static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
        {
            Texture2D SpriteTexture = LoadTexture(FilePath);
            Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

            return NewSprite;
        }

        private static Texture2D LoadTexture(string filePath)
        {
            Texture2D texture;
            byte[] data;

            if (File.Exists(filePath))
            {
                data = File.ReadAllBytes(filePath);
                texture = new Texture2D(2, 2);      
                texture.LoadImage(data);

                return texture;                 
            }

            return null;                     
        }
    }
}
