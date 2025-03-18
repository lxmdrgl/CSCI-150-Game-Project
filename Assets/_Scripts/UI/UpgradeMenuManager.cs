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
        public GameObject player1;
        public GameObject player2;
        private Stats stats1;
        private Stats stats2;
        private StatsChange statsChange1;
        private StatsChange statsChange2;
        private WeaponSwap weaponSwap1;
        private WeaponSwap weaponSwap2;
        private WeaponInventory weaponInventory1;
        private WeaponInventory weaponInventory2;
        
        [SerializeField] private GameObject UpgradeCanvasGO;
        [SerializeField] private List<UpgradeSlot> upgradeSlots;
        [SerializeField] private StatUpgradeDataSet statUpgradeDataSet;
        [SerializeField] private WeaponDataSet weaponDataSet;

        private bool isPaused;

        public PlayerInputHandler InputHandler1 { get; private set; }
        public PlayerInputHandler InputHandler2 { get; private set; }
        private PlayerInput playerInput1;
        private PlayerInput playerInput2;
        // private bool upgradeMenuInteract = false;
        
        
        private void Awake()
        {
        }

        void Start()
        {
            SetDependencies();

            UpgradeCanvasGO.SetActive(false);

            Unpause();
        }

        public void SetDependencies()
        {
            if (player1 != null)
            {
                stats1 = player1.GetComponentInChildren<Stats>();
                statsChange1 = player1.GetComponentInChildren<StatsChange>();
                weaponSwap1 = player1.GetComponentInChildren<WeaponSwap>();
                weaponInventory1 = player1.GetComponentInChildren<WeaponInventory>();
                InputHandler1 = player1.GetComponent<PlayerInputHandler>();
                playerInput1 = player1.GetComponent<PlayerInput>();

                statsChange1.OnMinorUpgradeInteract += HandleStatUpgradeMenuInteract;
                // weaponSwap1.OnMajorUpgradeInteract += HandleWeaponMenuInteract;
                weaponSwap1.OnMajorUpgradeInteract += (dataSet) => HandleWeaponMenuInteract(dataSet, 0);
            }
            if (player2 != null)
            {
                stats2 = player2.GetComponentInChildren<Stats>();
                statsChange2 = player2.GetComponentInChildren<StatsChange>();
                weaponSwap2 = player2.GetComponentInChildren<WeaponSwap>();
                weaponInventory2 = player2.GetComponentInChildren<WeaponInventory>();
                InputHandler2 = player2.GetComponent<PlayerInputHandler>();
                playerInput2 = player2.GetComponent<PlayerInput>();

                statsChange2.OnMinorUpgradeInteract += HandleStatUpgradeMenuInteract;
                // weaponSwap2.OnMajorUpgradeInteract += HandleWeaponMenuInteract;
                weaponSwap2.OnMajorUpgradeInteract += (dataSet) => HandleWeaponMenuInteract(dataSet, 1);
            }
        }

        void Update()
        {
            if ((InputHandler1 != null && InputHandler1.UpgradeOpenInput) || (InputHandler2 != null && InputHandler2.UpgradeOpenInput))
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
            /* if (player1 != null)
            {
                statsChange1.OnMinorUpgradeInteract += HandleStatUpgradeMenuInteract;
                weaponSwap1.OnMajorUpgradeInteract += (dataSet) => HandleWeaponMenuInteract(dataSet, 0);
            }
            if (player2 != null)
            {
                statsChange2.OnMinorUpgradeInteract += HandleStatUpgradeMenuInteract;
                weaponSwap2.OnMajorUpgradeInteract += (dataSet) => HandleWeaponMenuInteract(dataSet, 1);
            } */
        }

        private void OnDisable()
        {
            /* if (player1 != null)
            {
                statsChange1.OnMinorUpgradeInteract -= HandleStatUpgradeMenuInteract;
                weaponSwap1.OnMajorUpgradeInteract -= (dataSet) => HandleWeaponMenuInteract(dataSet, 0);
            }
            if (player2 != null)
            {
                statsChange2.OnMinorUpgradeInteract -= HandleStatUpgradeMenuInteract;
                weaponSwap2.OnMajorUpgradeInteract -= (dataSet) => HandleWeaponMenuInteract(dataSet, 1);
            } */
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

        private void HandleWeaponMenuInteract(WeaponDataSet dataSet, int playerIndex)
        {
            weaponDataSet = dataSet;

            // Create a local copy of the weaponData list
            List<WeaponData> localWeaponData = new List<WeaponData>(weaponDataSet.weaponData);

            // Remove weapons that are already in the inventory or do not meet prerequisites
            if (playerIndex == 0)
            {
                localWeaponData.RemoveAll(weaponData => 
                    weaponInventory1.currentWeaponData.Contains(weaponData) ||
                    weaponInventory1.oldWeaponData.Contains(weaponData) ||
                    (weaponData.prereqWeapon != null &&
                    !weaponInventory1.currentWeaponData.Contains(weaponData.prereqWeapon) &&
                    !weaponInventory1.oldWeaponData.Contains(weaponData.prereqWeapon))
                );
            }
            else if (playerIndex == 1)
            {
                localWeaponData.RemoveAll(weaponData => 
                    weaponInventory2.currentWeaponData.Contains(weaponData) ||
                    weaponInventory2.oldWeaponData.Contains(weaponData) ||
                    (weaponData.prereqWeapon != null &&
                    !weaponInventory2.currentWeaponData.Contains(weaponData.prereqWeapon) &&
                    !weaponInventory2.oldWeaponData.Contains(weaponData.prereqWeapon))
                );
            }

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

            if (playerInput1 != null)
            {
                playerInput1.SwitchCurrentActionMap("UI");
            }
            if (playerInput2 != null)
            {
                playerInput2.SwitchCurrentActionMap("UI");
            }

            OpenMainMenu();
        }

        public void Unpause()
        {
            isPaused = false;
            Time.timeScale = 1f;

            if (playerInput1 != null)
            {
                playerInput1.SwitchCurrentActionMap("Player");
            }
            if (playerInput2 != null)
            {
                playerInput2.SwitchCurrentActionMap("Player");
            }

            CloseAllMenus();
        }
        #endregion

        #region Canvas Activations/Deactivations
        private void OpenMainMenu()
        {
            UpgradeCanvasGO.SetActive(true);

            if (InputHandler1 != null)
            {
                InputHandler1.UseUpgradeOpenInput();
            }
            if (InputHandler2 != null)
            {
                InputHandler2.UseUpgradeOpenInput();
            }
        }

        private void CloseAllMenus()
        {
            UpgradeCanvasGO.SetActive(false);

            if (InputHandler1 != null)
            {
                InputHandler1.UseUpgradeOpenInput();
            }
            if (InputHandler2 != null)
            {
                InputHandler2.UseUpgradeOpenInput();
            }

            EventSystem.current.SetSelectedGameObject(null); // CLEAR SELECTED BUTTON FOR COLOR TRANSITIONS
        }
        #endregion

        /*public void OnUpgradeClicked(UpgradeSlot slot)
        {
            // int index = upgradeSlots.IndexOf(slot);
            if (statUpgradeDataSet != null) {
                // StatUpgradeData currentData = statUpgradeDataSet.statUpgradeData[index];
                StatUpgradeData currentData = slot.currentStatData;
                stats1.UpdateStats(currentData.Health, currentData.Attack);

            } else if (weaponDataSet != null) {
                // WeaponData currentData = weaponDataSet.weaponData[index];
                WeaponData currentData = slot.currentWeaponData;
                int weaponIndex = (int)currentData.weaponIndex;
                weaponInventory1.TrySetWeapon(currentData, weaponIndex);
            }

            statUpgradeDataSet = null;
            weaponDataSet = null;
        }*/

        public void OnUpgradeClicked(UpgradeSlot slot)
        {
            // Get the player input from the EventSystem (the last player to interact with UI)
            PlayerInput playerInput = EventSystem.current.currentSelectedGameObject?.GetComponentInParent<PlayerInput>();

            if (playerInput == null)
            {
                Debug.LogWarning("No player input detected for upgrade selection!");
                return;
            }

            int playerIndex = playerInput.playerIndex; // Get player index

            if (statUpgradeDataSet != null)
            {
                // Get the selected stat upgrade
                StatUpgradeData currentData = slot.currentStatData;

                if (playerIndex == 0)
                {
                    stats1.UpdateStats(currentData.Health, currentData.Attack);
                }
                else if (playerIndex == 1)
                {
                    stats2.UpdateStats(currentData.Health, currentData.Attack);
                }
            }
            else if (weaponDataSet != null)
            {
                // Get the selected weapon upgrade
                WeaponData currentData = slot.currentWeaponData;
                int weaponIndex = (int)currentData.weaponIndex;

                if (playerIndex == 0)
                {
                    weaponInventory1.TrySetWeapon(currentData, weaponIndex);
                }
                else if (playerIndex == 1)
                {
                    weaponInventory2.TrySetWeapon(currentData, weaponIndex);
                }
            }

            statUpgradeDataSet = null;
            weaponDataSet = null;
        }

        /* public void OnUpgradeClicked(UpgradeSlot slot, PlayerInput playerInput)
        {
            // int index = upgradeSlots.IndexOf(slot);
            if (statUpgradeDataSet != null) {
                // StatUpgradeData currentData = statUpgradeDataSet.statUpgradeData[index];
                StatUpgradeData currentData = slot.currentStatData;
                stats1.UpdateStats(currentData.Health, currentData.Attack);

            } else if (weaponDataSet != null) {
                // WeaponData currentData = weaponDataSet.weaponData[index];
                WeaponData currentData = slot.currentWeaponData;
                int weaponIndex = (int)currentData.weaponIndex;
                weaponInventory1.TrySetWeapon(currentData, weaponIndex);
            }

            statUpgradeDataSet = null;
            weaponDataSet = null;
        } */
    }
}
