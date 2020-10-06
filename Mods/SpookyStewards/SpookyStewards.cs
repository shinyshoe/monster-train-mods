using BepInEx;
using HarmonyLib;
using ShinyShoe;
using UnityEngine;
using System.IO;

namespace SpookyStewards
{
    [BepInPlugin("com.shinyshoe.spookystewards", "Spooky Stewards", "1.0.0.0")]
    public class SpookyStewards : BaseUnityPlugin
    {
        public static string[] SpriteFilePaths;

        void Awake()
        {
            var directory = Path.Combine(Path.GetDirectoryName(Info.Location), "images");
            SpriteFilePaths = Directory.GetFiles(directory);

            var harmony = new Harmony("com.shinyshoe.spookystewards");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(CharacterData))]
    [HarmonyPatch("GetName")]
    public static class ChangeFriendCharacterName
    {
        static void Postfix(ref string __result)
        {
            if (__result == "Train Steward") __result = "Spooky Steward";
        }
    }

    [HarmonyPatch(typeof(CardState))]
    [HarmonyPatch("GetTitle")]
    public static class ChangeFriendCardName
    {
        static void Postfix(ref string __result)
        {
            if (__result == "Train Steward") __result = "Spooky Steward";
        }
    }

    [HarmonyPatch(typeof(CharacterUI))]
    [HarmonyPatch("Setup")]
    public static class Mod_CharacterUI_Setup
    {
        private static CharacterOverlayImage OverlayImage;

        static void Postfix(ref string ___debugName, ref CharacterUIMeshBase ____characterMesh, ref CharacterState characterState)
        {
            if (___debugName.StartsWith("Character_TrainSteward"))
            {
                OverlayImage = CreateFaceObject(____characterMesh.GetSortingLayer().LayerID());
                (____characterMesh as CharacterUIMeshSpine).OrNull()?.AttachToBone(OverlayImage.transform, VfxAtLoc.Location.BoneStatusEffectSlot1);
            }
        }

        private static CharacterOverlayImage CreateFaceObject(int sortingLayerID)
        {
            GameObject parent = new GameObject("Face");
            GameObject child = new GameObject("Face_Image");
            child.transform.SetParent(parent.transform);

            CharacterOverlayImage overlayImage = parent.AddComponent<CharacterOverlayImage>();
            SpriteRenderer spriteRenderer = child.AddComponent<SpriteRenderer>();

            spriteRenderer.transform.localPosition += new Vector3(-.65f, -.65f, -.1f);
            spriteRenderer.transform.localScale = new Vector3(.456f, .456f);

            string path = SpookyStewards.SpriteFilePaths[RandomManager.Range(0, SpookyStewards.SpriteFilePaths.Length, RngId.NonDeterministic)];
            spriteRenderer.sprite = LoadNewSprite(path);
            spriteRenderer.sortingLayerID = sortingLayerID;
            spriteRenderer.sortingOrder = 1;

            overlayImage.SetSpriteRenderer(spriteRenderer);
            return overlayImage;
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

    [HarmonyPatch(typeof(CharacterUI))]
    [HarmonyPatch("UpdateVfxSortingOrder")]
    public static class Mod_CharacterUI_UpdateVfxSortingOrder
    {
        static void Postfix(ref CharacterUIMeshBase ____characterMesh, ref int ___TopAllMeshSortingOrder, ref SpriteRenderer ___spriteRenderer)
        {
            CharacterOverlayImage overlayImage = ___spriteRenderer.GetComponentInChildren<CharacterOverlayImage>();
            if (overlayImage != null)
            {
                overlayImage.SetSortingOrder(____characterMesh.GetSortingLayer().LayerID(), ___TopAllMeshSortingOrder);
            }
        }
    }
}
