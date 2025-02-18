using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Game.Weapons;
using System;

namespace Game.CoreSystem
{
    public class UpgradeMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private Core core;
        [SerializeField] private Stats stats;
        [SerializeField] private StatsChange statsChange;
        
        [SerializeField] private GameObject UpgradeCanvasGO;
        [SerializeField] private List<UpgradeSlot> upgradeSlots;
        [SerializeField] private StatUpgradeDataSet upgradeDataSet;

        private bool isPaused;

        public PlayerInputHandler InputHandler { get; private set; }
        private PlayerInput playerInput;
        private bool upgradeMenuInteract = false;
        
        
        private void Awake()
        {
        }

        void Start()
        {
            InputHandler = player.GetComponent<PlayerInputHandler>();
            playerInput = player.GetComponent<PlayerInput>();

            UpgradeCanvasGO.SetActive(true);

            Unpause();
        }

        void Update()
        {
            if (InputHandler.UpgradeOpenInput)
            {
                if (!isPaused)
                {
                    Pause();
                }
            }/*
            else if (InputHandler.UIMenuCloseInput)
            {
                if (isPaused)
                {
                    Unpause();
                }
            }*/
        }

        private void OnEnable()
        {
            statsChange.OnMinorUpgradeInteract += HandleUpgradeMenuInteract;
        }

        private void OnDisable()
        {
            statsChange.OnMinorUpgradeInteract -= HandleUpgradeMenuInteract;
        }

        private void HandleUpgradeMenuInteract(StatUpgradeDataSet dataSet)
        {
            // upgradeMenuInteract = true;
            upgradeDataSet = dataSet;

            List<int> usedIndices = new List<int>();
            System.Random random = new System.Random();

            foreach (UpgradeSlot slot in upgradeSlots)
            {
                int index;
                do
                {
                    index = random.Next(upgradeDataSet.statUpgradeData.Count);
                } while (usedIndices.Contains(index));

                usedIndices.Add(index);
                slot.SetData(upgradeDataSet.statUpgradeData[index]);
            }

            Pause();
        }

        #region Pause/Unpause Functions
        public void Pause()
        {
            isPaused = true;
            Time.timeScale = 0f;

            playerInput.SwitchCurrentActionMap("UI");

            OpenMainMenu();
        }

        public void Unpause()
        {
            isPaused = false;
            Time.timeScale = 1f;

            playerInput.SwitchCurrentActionMap("Player");

            CloseAllMenus();
        }
        #endregion

        #region Canvas Activations/Deactivations
        private void OpenMainMenu()
        {
            UpgradeCanvasGO.SetActive(true);

            InputHandler.UseUpgradeOpenInput();
        }

        private void CloseAllMenus()
        {
            UpgradeCanvasGO.SetActive(false);

            InputHandler.UseUpgradeOpenInput();
        }
        #endregion

        public void OnUpgradeClicked(UpgradeSlot slot)
        {
            int index = upgradeSlots.IndexOf(slot);
            StatUpgradeData currentData = upgradeDataSet.statUpgradeData[index];
            stats.UpdateStats(currentData.Health, currentData.Attack);
        }
    }
}
