using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Game.Weapons;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.CoreSystem
{
    public class UpgradeMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private Core core;
        [SerializeField] private Stats stats;
        [SerializeField] private StatsChange statsChange;
        [SerializeField] private WeaponSwap weaponSwap;
        [SerializeField] private WeaponInventory weaponInventory;
        
        [SerializeField] private GameObject UpgradeCanvasGO;
        [SerializeField] private List<UpgradeSlot> upgradeSlots;
        [SerializeField] private StatUpgradeDataSet statUpgradeDataSet;
        [SerializeField] private WeaponDataSet weaponDataSet;

        private bool isPaused;

        public PlayerInputHandler InputHandler { get; private set; }
        private PlayerInput playerInput;
        // private bool upgradeMenuInteract = false;
        
        
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
            statsChange.OnMinorUpgradeInteract += HandleStatUpgradeMenuInteract;
            weaponSwap.OnMajorUpgradeInteract += HandleWeaponMenuInteract;
        }

        private void OnDisable()
        {
            statsChange.OnMinorUpgradeInteract -= HandleStatUpgradeMenuInteract;
            weaponSwap.OnMajorUpgradeInteract -= HandleWeaponMenuInteract;
        }

        private void HandleStatUpgradeMenuInteract(StatUpgradeDataSet dataSet)
        {
            statUpgradeDataSet = dataSet;

            List<int> usedIndices = new List<int>();
            // System.Random random = new System.Random();

            for (int i = 0; i < upgradeSlots.Count; i++)
            {
                if (i < statUpgradeDataSet.statUpgradeData.Count)
                {
                    int index;
                    do
                    {
                        index = UnityEngine.Random.Range(0, statUpgradeDataSet.statUpgradeData.Count);
                    } while (usedIndices.Contains(index));

                    usedIndices.Add(index);
                    upgradeSlots[i].SetData(statUpgradeDataSet.statUpgradeData[index]);
                }
                else
                {
                    upgradeSlots[i].SetData(null);
                }
            }

            Pause();
        }

        private void HandleWeaponMenuInteract(WeaponDataSet dataSet)
        {
            weaponDataSet = dataSet;

            // Create a local copy of the weaponData list
            List<WeaponData> localWeaponData = new List<WeaponData>(weaponDataSet.weaponData);

            // Remove weapons that are already in the inventory or do not meet prerequisites
            localWeaponData.RemoveAll(weaponData => 
                weaponInventory.currentWeaponData.Contains(weaponData) ||
                weaponInventory.oldWeaponData.Contains(weaponData) ||
                (weaponData.prereqWeapon != null &&
                !weaponInventory.currentWeaponData.Contains(weaponData.prereqWeapon) &&
                !weaponInventory.oldWeaponData.Contains(weaponData.prereqWeapon))
            );

            List<int> usedIndices = new List<int>();
            // System.Random random = new System.Random();

            for (int i = 0; i < upgradeSlots.Count; i++)
            {
                if (i < localWeaponData.Count)
                {
                    int index;
                    do
                    {
                        index = UnityEngine.Random.Range(0, localWeaponData.Count);
                    } while (usedIndices.Contains(index));

                    usedIndices.Add(index);
                    upgradeSlots[i].SetData(localWeaponData[index]);
                }
                else
                {
                    upgradeSlots[i].SetData(null);
                }
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

            EventSystem.current.SetSelectedGameObject(null); // CLEAR SELECTED BUTTON FOR COLOR TRANSITIONS
        }
        #endregion

        public void OnUpgradeClicked(UpgradeSlot slot)
        {
            // int index = upgradeSlots.IndexOf(slot);
            if (statUpgradeDataSet != null) {
                // StatUpgradeData currentData = statUpgradeDataSet.statUpgradeData[index];
                StatUpgradeData currentData = slot.currentStatData;
                stats.UpdateStats(currentData.Health, currentData.Attack);

            } else if (weaponDataSet != null) {
                // WeaponData currentData = weaponDataSet.weaponData[index];
                WeaponData currentData = slot.currentWeaponData;
                int weaponIndex = (int)currentData.weaponIndex;
                weaponInventory.TrySetWeapon(currentData, weaponIndex);
            }

            statUpgradeDataSet = null;
            weaponDataSet = null;
        }
    }
}
