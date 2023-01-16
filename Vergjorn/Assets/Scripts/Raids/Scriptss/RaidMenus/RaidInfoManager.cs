
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
public class RaidInfoManager : MonoBehaviour
{
    public enum RaidMenuState { closed, mapOpen, raidInfoTab, selectionTab}
    public RaidMenuState state;

    [Header("Info tab")]
    public static RaidInfoManager Instance;

    public TextMeshProUGUI raidNameText;
    public TextMeshProUGUI raidDescText;
    

    RaidInfo currentRaidInfo;

    public GameObject raidInfoTab;
    public GameObject shipAndWorkersSelectionTab;

    public Image startRaidButtonImage;
    public Color canStartRaidColor;
    public Color cannotStartRaidColor;

    public RaidWorkerShipLists raidSceneInfoGetter;

    [Header("Map")]
    public GameObject map;
    public bool mapOpen = false;

    [Header("Completed raid or not")]
    public TextMeshProUGUI completedOrNotText;
    public RaidSave currentRaidSave;
    bool currentRaidCompleted;
    public Color completedColor;
    public Color notCompletedeColor;
    [Header("Cheats")]
    public bool letStartRaidNoMatterOfVikings;

    public Transform shipButtonsParent;
    public Transform workerButtonParent;

    public GameObject selectButtonPrefab;

    public List<WorkerData> selectedWorkers;
    public List<Ship> selectedShips;

    public TextMeshProUGUI warningText;

    [Header("Capcity and selected management")]
    public TextMeshProUGUI seatsText;
    public TextMeshProUGUI workersOfSeatsText;

    List<GameObject> buttonsToDestroy = new List<GameObject>();
    private void Awake()
    {
        Instance = this;
    }
   
    private void Update()
    {
        switch (state)
        {
            case RaidMenuState.closed:
                if (map.activeSelf == true)
                {
                    map.SetActive(false);
                }
                if(raidInfoTab.activeSelf == true)
                {
                    raidInfoTab.SetActive(false);
                }
                if (shipAndWorkersSelectionTab.activeSelf == true)
                {
                    DestroyButtons();
                    shipAndWorkersSelectionTab.SetActive(false);
                }

                break;
            case RaidMenuState.mapOpen:

                if (map.activeSelf == false)
                {
                    map.SetActive(true);
                }
                if (raidInfoTab.activeSelf == true)
                {
                    raidInfoTab.SetActive(false);
                }
                if (shipAndWorkersSelectionTab.activeSelf == true)
                {
                    DestroyButtons();
                    shipAndWorkersSelectionTab.SetActive(false);
                }
                break;
            case RaidMenuState.raidInfoTab:
                if (map.activeSelf == false)
                {
                    map.SetActive(true);
                }
                if (raidInfoTab.activeSelf == false)
                {
                    raidInfoTab.SetActive(true);
                }
                if (shipAndWorkersSelectionTab.activeSelf == true)
                {
                    DestroyButtons();
                    shipAndWorkersSelectionTab.SetActive(false);
                }

                break;
            case RaidMenuState.selectionTab:
                if (map.activeSelf == false)
                {
                    map.SetActive(true);
                }
                if (raidInfoTab.activeSelf == true)
                {
                    raidInfoTab.SetActive(false);
                }
                if (shipAndWorkersSelectionTab.activeSelf == false)
                {
                    shipAndWorkersSelectionTab.SetActive(true);
                }

                if (CanStartRaid())
                {
                    startRaidButtonImage.color = canStartRaidColor;
                }
                else
                {
                    startRaidButtonImage.color = cannotStartRaidColor;
                }

                if (!CanStartRaid())
                {
                    string text = "";
                    warningText.gameObject.SetActive(true);
                    if(!SelectedEnoughSeats() && SelectedVikings())
                    {
                        text = "Not enough ships selected";
                    }
                    if(!SelectedVikings())
                    {
                        text = "No vikings selected";
                    }
                    warningText.text = text;
                }
                else
                {
                    warningText.gameObject.SetActive(false);
                }

                seatsText.text = "Seats: " + CurrentSeats().ToString();
                workersOfSeatsText.text = selectedWorkers.Count.ToString() + " / " + CurrentSeats().ToString();
                break;
        }
    }

    public void OpenMap()
    {
     
        state = RaidMenuState.mapOpen;
        
        
    }

    public void CloseMap()
    {
        state = RaidMenuState.closed;
    }

    #region RaidInfoTab
    public void GetRaidInfo(RaidInfo raidInfo)
    {
        currentRaidInfo = raidInfo;

        state = RaidMenuState.raidInfoTab;

        FileInfo info = new FileInfo(Application.persistentDataPath + "/saves/" + currentRaidInfo.thisRaidSaveFileName + ".save");
        if (info.Exists)
        {
            currentRaidSave = (RaidSave)SerializationManager.Load(Application.persistentDataPath + "/saves/" + currentRaidInfo.thisRaidSaveFileName + ".save");
            currentRaidCompleted = currentRaidSave.raidComleted;
            Debug.Log("Found file");
        }
        else
        {
            Debug.Log("Didnt find file");
            currentRaidCompleted = false;
        }

       

        DisplayRaidInfo();
    }

    void DisplayRaidInfo()
    {
        raidNameText.text = currentRaidInfo.raidName;
        raidDescText.text = currentRaidInfo.raidDesc;


        if (!currentRaidCompleted)
        {
            completedOrNotText.text = "This raid is not completed, destroy all buildings to complete this raid!";
            completedOrNotText.color = notCompletedeColor;
        }
        else
        {
            completedOrNotText.text = "This raid is completed";
            completedOrNotText.color = completedColor;
        }
    }

    public void CloseRaidInfoTab()
    {
        state = RaidMenuState.mapOpen;
    }

    public void OpenSelectionTab()
    {
        state = RaidMenuState.selectionTab;

        //Spawn buttons;
        for (int i = 0; i < PopulationManager.Instance.workersInGame.Count; i++)
        {
            GameObject go = Instantiate(selectButtonPrefab, workerButtonParent);
            go.GetComponent<RaidSelectionButton>().GetWorkerData(PopulationManager.Instance.workersInGame[i].myData);
            buttonsToDestroy.Add(go);
        }

        for (int i = 0; i < ShipManager.Instance.allShips.Count; i++)
        {
            GameObject go = Instantiate(selectButtonPrefab, shipButtonsParent);
            go.GetComponent<RaidSelectionButton>().GetShipData(ShipManager.Instance.allShips[i]);
            buttonsToDestroy.Add(go);
        }
    }

    void DestroyButtons()
    {
       foreach(GameObject g in buttonsToDestroy)
        {
            Destroy(g);
        }

    }
    public void CloseSelectionTab()
    {
        DestroyButtons();

        state = RaidMenuState.raidInfoTab;
        DisplayRaidInfo();
    }
    public void OpenRaidInfoTab()
    {
        state = RaidMenuState.raidInfoTab;
    }

    float CurrentSeats()
    {
        float t = 0;

        for (int i = 0; i < selectedShips.Count; i++)
        {
            t += selectedShips[i].seats;
        }

        return t;
    }
    public void StartRaid()
    {
        
        if(selectedWorkers.Count == 0)
        {
            Debug.Log("No one to bring");
            return;
        }

        if (letStartRaidNoMatterOfVikings)
        {
            raidSceneInfoGetter.workers = selectedWorkers;
            raidSceneInfoGetter.ships = selectedShips;

            SaveManager.Instance.OnSave();
            LoadCurrentRaidScene();
            
            return;
        }

        if (CurrentSeats() < selectedWorkers.Count)
        {
            Debug.Log("Not enough seats");
            return;
        }

        raidSceneInfoGetter.workers = selectedWorkers;
        raidSceneInfoGetter.ships = selectedShips;
        

        SaveManager.Instance.OnSave();
        LoadCurrentRaidScene();
    }
    public bool SelectedEnoughSeats()
    {
        if(CurrentSeats() < selectedWorkers.Count)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool SelectedVikings()
    {
        if(selectedWorkers.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool CanStartRaid()
    {
        if(selectedWorkers.Count == 0)
        {
            return false;
        }
        if(CurrentSeats() < selectedWorkers.Count)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #region Select and deselct workers n ships
    public void SelectWorker(WorkerData w)
    {
        if(!selectedWorkers.Contains(w))
        {
            selectedWorkers.Add(w);
        }
        
    }

    public void SelectShip(Ship s)
    {
        if (!selectedShips.Contains(s))
        {
            selectedShips.Add(s);
        }
        
    }

    public void DeselectWorker(WorkerData w)
    {
        if (selectedWorkers.Contains(w))
        {
            selectedWorkers.Remove(w);
        }
        
    }

    public void DeselectShip(Ship s)
    {
        if (selectedShips.Contains(s))
        {
            selectedShips.Remove(s);
        }
    }
    #endregion
    void LoadCurrentRaidScene()
    {
        SceneLoader.Instance.LoadScene(currentRaidInfo.raidSceneName);
        
    }
    #endregion
}
