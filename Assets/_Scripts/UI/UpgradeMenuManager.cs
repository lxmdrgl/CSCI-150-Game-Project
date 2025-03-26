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
        private int playerIndex = -1;
        private UpgradeSlot player1Slot;
        private UpgradeSlot player2Slot;
        
        [SerializeField] private GameObject UpgradeCanvasGO;
        [SerializeField] private List<UpgradeSlot> upgradeSlots;
        [SerializeField] private StatUpgradeDataSet statUpgradeDataSet;
        [SerializeField] private WeaponDataSet weaponDataSet;
        [SerializeField] private int currentPlayer = -1;

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
                InputHandler1.OnUpgradeInputChanged += HelperUIUpgradeClicked;
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
                InputHandler2.OnUpgradeInputChanged += HelperUIUpgradeClicked;
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
            currentPlayer = playerIndex;

            
            // Debug.Log("Debugging .....");

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

            EventSystem.current.SetSelectedGameObject(upgradeSlots[0].gameObject); // SET SELECTED BUTTON FOR COLOR TRANSITIONS
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

            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null); // CLEAR SELECTED BUTTON FOR COLOR TRANSITIONS
            }
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

        public void HelperUIUpgradeClicked(int Index)
        {
            if (EventSystem.current.currentSelectedGameObject.GetComponent<UpgradeSlot>() != null)
            {
                Debug.Log("upgrade clicked: " + EventSystem.current.currentSelectedGameObject.name);
                playerIndex = Index;
            }
        }   

        bool handlePlayer1 = false;
        bool handlePlayer2 = false;

        public void OnUpgradeClicked(UpgradeSlot slot)
        {
            Debug.Log("upgrade playerIndex: " + playerIndex);

            if (playerIndex == -1)    
            {
                return;
            }

            if (playerIndex == 0  && currentPlayer == 0)
            {
                player1Slot = slot;
            }
            else if (playerIndex == 1 && currentPlayer == 1)
            {
                player2Slot = slot;
            }

            if (player1Slot != null && player2Slot == null)
            {
                if (!handlePlayer1)
                {
                    HandleWeaponMenuInteract(weaponDataSet, 1); // set player 2
                    handlePlayer1 = true;
                }
            }
            if (player1Slot == null && player2Slot != null)
            {
                HandleWeaponMenuInteract(weaponDataSet, 0); // set player 1
                if (!handlePlayer2)
                {
                    HandleWeaponMenuInteract(weaponDataSet, 0); // set player 1
                    handlePlayer2 = true;
                }
            }

            if (player1Slot != null && player2Slot != null)
            {
                Debug.Log("Both players have selected. Applying upgrades...");

                if (statUpgradeDataSet != null)
                {
                    // Player 1 upgrade
                    StatUpgradeData currentData = player1Slot.currentStatData;
                    stats1.UpdateStats(currentData.Health, currentData.Attack);
                    

                    // Player 2 upgrade
                    currentData = player2Slot.currentStatData;
                    stats2.UpdateStats(currentData.Health, currentData.Attack);
                    
                }
                else if (weaponDataSet != null)
                {
                    // Player 1 weapon upgrade
                    WeaponData currentData = player1Slot.currentWeaponData;
                    weaponInventory1.TrySetWeapon(currentData, (int)currentData.weaponIndex);
                    

                    // Player 2 weapon upgrade
                    currentData = player2Slot.currentWeaponData;
                    weaponInventory2.TrySetWeapon(currentData, (int)currentData.weaponIndex);
                    
                }

                // Reset selections for next upgrade
                player1Slot = null;
                player2Slot = null;
                statUpgradeDataSet = null;
                weaponDataSet = null;
                playerIndex = -1;

                handlePlayer1 = false;
                handlePlayer2 = false;
                Unpause();
            }
/*
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
            playerIndex = -1;*/
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
