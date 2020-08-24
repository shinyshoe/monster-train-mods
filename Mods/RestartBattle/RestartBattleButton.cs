using HarmonyLib;
using ShinyShoe;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RestartBattle
{
    public class RestartBattleButton : MonoBehaviour
    {
        private BattleHud _battleHud;
        private GameUISelectableButton _restartBattleButton;
        private Coroutine _coroutine;

        private void Start()
        {
            _battleHud = GetComponentInParent<BattleHud>();
            _restartBattleButton = GetComponent<GameUISelectableButton>();
            UISignals.GameUITriggered.AddListener(OnGameUITriggered);
        }

        private void OnDestroy()
        {
            UISignals.GameUITriggered.RemoveListener(OnGameUITriggered);
        }

        private void OnGameUITriggered(CoreInputControlMapping mapping, IGameUIComponent component)
        {
            // If the "Restart Battle" button was clicked and we aren't already in the process of restarting, restart the battle
            if (component.IsGameUIComponent(_restartBattleButton) && mapping.IsID(InputManager.Controls.Clicked) && _coroutine == null)
            {
                // Play a sound when the button is clicked
                Traverse.Create(_battleHud).Field("soundManager").GetValue<SoundManager>().PlaySfx(SoundCueNames.EndTurn);

                // Restart the battle
                _coroutine = GlobalMonoBehavior.Inst.StartCoroutine(RestartBattleCoroutine(_battleHud));
            }
        }

        public static IEnumerator RestartBattleCoroutine(BattleHud battleHud)
        {
            // Get managers from battleHud private fields
            CombatManager combatManager = Traverse.Create(battleHud).Field("combatManager").GetValue<CombatManager>();
            GameStateManager gameStateManager = Traverse.Create(combatManager).Field("gameStateManager").GetValue<GameStateManager>();
            SaveManager saveManager = Traverse.Create(combatManager).Field("saveManager").GetValue<SaveManager>();
            ScreenManager screenManager = Traverse.Create(battleHud).Field("screenManager").GetValue<ScreenManager>();
            CardManager cardManager = Traverse.Create(battleHud).Field("cardManager").GetValue<CardManager>();
            CardStatistics cardStatistics = Traverse.Create(cardManager).Field("cardStatistics").GetValue<CardStatistics>();

            // Get the current type of run that you're playing
            RunType runType = Traverse.Create(saveManager).Field("activeRunType").GetValue<RunType>();
            string sharecode = Traverse.Create(saveManager).Field("activeSharecode").GetValue<string>();

            // Return to the main menu
            gameStateManager.LeaveGame();
            screenManager.ReturnToMainMenu();
            cardStatistics.ResetAllStats();

            // Wait until the main menu is loaded
            yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "main_menu" && SceneManager.GetActiveScene().isLoaded);
            yield return null;
            yield return null;
            yield return null;

            // Resume your battle from the main menu
            gameStateManager.ContinueRun(runType, sharecode);
        }

        //---------------------------------------------------------------------
        // Create the restart battle button
        //---------------------------------------------------------------------

        /// <summary>
        /// Copy the "end turn" button and modify it to be the "restart battle" button.
        /// </summary>
        public static void Create(BattleHud battleHud)
        {
            // Don't create button if we're doing a hell rush
            if (Traverse.Create(Traverse.Create(battleHud).Field("combatManager").GetValue<CombatManager>()).Field("saveManager").GetValue<SaveManager>().IsBattleMode())
            {
                return;
            }

            // Get BattleHud private fields
            EndTurnUI endTurnButton = Traverse.Create(battleHud).Field("endTurnButton").GetValue<EndTurnUI>();
            EnergyUI energyUI = Traverse.Create(battleHud).Field("energyUI").GetValue<EnergyUI>();
            CardPileCountUI deckUI = Traverse.Create(battleHud).Field("deckUI").GetValue<CardPileCountUI>();

            // Copy end turn game object
            GameObject buttonRoot = GameObject.Instantiate(endTurnButton.gameObject, energyUI.transform.parent) as GameObject;
            buttonRoot.name = "RestartBattleButton";

            // Delete unwanted game objects and components
            List<GameObject> toDelete = new List<GameObject>();
            DeleteUnwanted(buttonRoot, "", toDelete);

            // Set position to in the corner next to the ember
            RectTransform transRoot = buttonRoot.transform as RectTransform;
            RectTransform transEmber = energyUI.transform as RectTransform;
            transRoot.anchorMin = transEmber.anchorMin;
            transRoot.anchorMax = transEmber.anchorMax;
            transRoot.anchoredPosition = transEmber.anchoredPosition;
            transRoot.sizeDelta = transEmber.sizeDelta;
            buttonRoot.transform.position = new Vector3(
                Mathf.LerpUnclamped(deckUI.transform.position.x, energyUI.transform.position.x, 1.75f),
                Mathf.LerpUnclamped(deckUI.transform.position.y, energyUI.transform.position.y, -0.1f),
                buttonRoot.transform.position.z);

            // Make it a bit smaller than the "End Turn" button 0.6
            buttonRoot.transform.localScale = new Vector3(.6f, .6f, buttonRoot.transform.localScale.z);

            // Clear shortcut key so "End Turn" keyboard shortcut doesn't trigger restart battle
            GameUISelectableButton restartBattleButton = buttonRoot.GetComponent<GameUISelectableButton>();
            Traverse.Create(restartBattleButton).Field("inputType").SetValue((int)InputManager.Controls.NONE);

            // Set button text to "Restart Battle"
            TextMeshProUGUI textMeshPro = buttonRoot.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.text = "Restart Battle";

            // Add RestartBattleButton to handle the button click
            restartBattleButton.gameObject.AddComponent<RestartBattleButton>();
        }

        /// <summary>
        /// We copied the "end turn" button to be used as the "restart battle" button.  Remove all the 
        /// game objects and components that we don't need/want for doing restart battle.
        /// </summary>
        /// <param name="objRoot"></param>
        private static void DeleteUnwanted(GameObject objRoot)
        {
            // Delete the unwanted components and collect the unwanted game objects
            List<GameObject> toDelete = new List<GameObject>();
            DeleteUnwanted(objRoot, "", toDelete);

            // Delete the unwanted game objects
            foreach (GameObject obj in toDelete)
            {
                Destroy(obj);
            }
        }

        /// <summary>
        /// We copied the "end turn" button to be used as the "restart battle" button.  Remove all the 
        /// game objects and components that we don't need/want for doing restart battle.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parentPath"></param>
        /// <param name="toDelete"></param>
        private static void DeleteUnwanted(GameObject obj, string parentPath, List<GameObject> toDelete)
        {
            // Get a name for obj that we can refer to
            string path = string.IsNullOrEmpty(parentPath) ? obj.name : $"{parentPath}/{obj.name}";

            // Keep the root object, but delete all its components except for the GameUISelectableButton and Animator
            if (path == "RestartBattleButton")
            {
                DeleteUnwantedComponents(obj, typeof(GameUISelectableButton), typeof(Animator));
            }
            // Keep the "Content" and "Bg" game objects as they are
            else if (path == "RestartBattleButton/Content" ||
                path == "RestartBattleButton/Content/Bg")
            {
            }
            // Keep the "Label" game object, but delete all its components except for TextMeshProUGUI
            else if (path == "RestartBattleButton/Content/Label")
            {
                DeleteUnwantedComponents(obj, typeof(TextMeshProUGUI));
            }
            // Otherwise, delete the game object
            else
            {
                toDelete.Add(obj);
            }

            // Recursively remove unwanted children
            foreach (Transform transChild in obj.transform)
            {
                DeleteUnwanted(transChild.gameObject, path, toDelete);
            }
        }

        /// <summary>
        /// Remove all the components from obj except for the ones specified by 
        /// compsWanted and components that should never be removed.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="compsWanted"></param>
        private static void DeleteUnwantedComponents(GameObject obj, params Type[] compsWanted)
        {
            Component[] comps = obj.GetComponents<Component>();
            foreach (Component comp in comps)
            {
                if (comp.GetType() == typeof(RectTransform) ||
                    comp.GetType() == typeof(CanvasRenderer) ||
                    Array.IndexOf(compsWanted, comp.GetType()) >= 0)
                {
                    continue;
                }

                Destroy(comp);
            }
        }
    }
}