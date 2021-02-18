using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentSession = 1;
    public int socialScenePeriod = 3; // i.e, one after every session. use 2 for one after every 2 sessions and so on.
    public int sessionCount = 3;
    [SerializeField] List<float> playersCurrentMoney; // index by player index, 0 is human's, 1 is ai_1 etc.
    [SerializeField] List<PlayerItems> playerItems;
    private List<float> moneyCopy;
     
    [SerializeField] List<CandidateSpeakers> sessionSpeakers;

    private bool playedTutorial = false; // dont reset, show tutorial once.
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        moneyCopy = new List<float>(playersCurrentMoney);
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0) // start scene, show cinematic entrance etc.
        {
            currentSession = 0;
            for(int i=0; i < moneyCopy.Count; i++) playersCurrentMoney[i] = moneyCopy[i];
            
            for(int i=0; i < playerItems.Count; i++)
            {
                playerItems[i].ownedItems = new List<string>();
            }
            Debug.Log("restarted, reset currentsession, money, ownedItems.");
        }
        else if(scene.buildIndex == 1) // auction scene
        {
            Debug.Log("Session scene");
            if(currentSession >= sessionCount)
            {
                Debug.Log("All sessions ended. show game result."); 
                FindObjectOfType<TransitionManager>().LoadEndScene();

            }
            else
            {
                FindObjectOfType<AuctionManager>().StartSession(currentSession);
            }
        }
        else if(scene.buildIndex == 2) //dialogue scene
        {
            Debug.Log("Dialogue scene");

            FindObjectOfType<DialogueManager>().SetupScene();
        }
    }

    public void SessionEnded(int winnerIndex, int soldItemIndex)
    {
        currentSession++;
        Invoke("SwitchToDialogueScene", 5f);
    }

    private void SwitchToDialogueScene()
    {
        FindObjectOfType<TransitionManager>().LoadDialogueScene();

    }

    public void DecreaseMoney(int playerIndex, float amount)
    {
        playersCurrentMoney[playerIndex] -= amount;
    }
    public float GetCurrentMoney(int playerIndex)
    {
        return playersCurrentMoney[playerIndex];
    }

    public bool IsLastSession()
    {
        return currentSession == sessionCount-1;
    }

    public void BoughtItem(int playerIndex, string itemId)
    {
        playerItems[playerIndex].ownedItems.Add(itemId);
    }
    public bool HasItem(int playerIndex, string itemId)
    {
        string result = playerItems[playerIndex].ownedItems.Find(t => t == itemId);
        if(result != null || result != "")
        {
            return true;
        }
        return false;
    }

    public CandidateSpeakers GetSessionSpeakers()
    {
        return sessionSpeakers[currentSession];
    }
    [System.Serializable]
    public class PlayerItems
    {
        public int playerIndex;
        public List<string> ownedItems = new List<string>();
    }

    [System.Serializable]
    public class CandidateSpeakers
    {
        public int sessionNumber;
        public List<string> speakerNames;
    }
}
