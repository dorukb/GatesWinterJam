using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentSession = 1;
    public int currentDialogueSession = 0;

    public int socialScenePeriod = 3; // i.e, one after every session. use 2 for one after every 2 sessions and so on.
    public int sessionCount = 15;
    [SerializeField] List<float> playersCurrentMoney; // index by player index, 0 is human's, 1 is ai_1 etc.
    [SerializeField] List<PlayerItems> playerItems;
    private List<float> moneyCopy;


    [SerializeField] int playerScore;
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
        if (scene.buildIndex == 0) // start scene, show cinematic entrance etc.
        {
            currentSession = 1;
            currentDialogueSession = 0;
            for (int i = 0; i < moneyCopy.Count; i++) playersCurrentMoney[i] = moneyCopy[i];

            for (int i = 0; i < playerItems.Count; i++)
            {
                playerItems[i].ownedItems = new List<string>();
            }
            Debug.Log("restarted, reset currentsession, money, ownedItems.");
        }
        else if (scene.buildIndex == 1) // auction scene
        {
            //Debug.Log("Session scene");
            if (currentSession > sessionCount)
            {
                Debug.Log("All sessions ended. show game result.");
                FindObjectOfType<TransitionManager>().LoadEndScene();

            }
            else
            {
                FindObjectOfType<AuctionManager>().StartSession(currentSession);
            }
        }
        else if (scene.buildIndex == 2) //dialogue scene
        {
            Debug.Log("Dialogue scene");

            currentDialogueSession++;
            FindObjectOfType<DialogueManager>().SetupScene();
        }
    }

    public void SessionEnded()
    {
        if (currentSession % socialScenePeriod == 0)
        {
            currentSession++;
            // time for dialogue scene.
            FindObjectOfType<TransitionManager>().LoadDialogueScene();
            //Invoke("SwitchToDialogueScene", 2f);
        }
        else
        {
            currentSession++;
            FindObjectOfType<TransitionManager>().LoadAuctionScene();
            //Invoke("ReloadAuctionScene", 2f);
        }
    }
    private void SwitchToDialogueScene()
    {
        FindObjectOfType<TransitionManager>().LoadDialogueScene();

    }
    private void ReloadAuctionScene()
    {

        FindObjectOfType<TransitionManager>().LoadAuctionScene();
    }

    public void DecreaseMoney(int playerIndex, float amount)
    {
        playersCurrentMoney[playerIndex] -= amount;
    }
    public float GetCurrentMoney(int playerIndex)
    {
        return playersCurrentMoney[playerIndex];
    }
    public int GetPlayerScore()
    {
        return playerScore;
    }
    public bool IsLastSession()
    {
        return currentSession == sessionCount;
    }

    public void BoughtItem(int playerIndex, ItemData itemData)
    {
        if (playerIndex == 0)
        {
            //player bought an item, increase score!
            int scored = 0;
            switch (itemData.type)
            {
                case ItemType.Valuable:
                    scored = 150;
                    break;
                case ItemType.Regular:
                    scored = 75;
                    break;
                case ItemType.Worthless:
                    scored = 30;
                    break;
            }
            playerScore += scored;
            FindObjectOfType<ScoreDisplayUI>().UpdateScore(playerScore);

        }
        playerItems[playerIndex].ownedItems.Add(itemData.name);
    }
    public bool HasItem(int playerIndex, string itemId)
    {
        string result = playerItems[playerIndex].ownedItems.Find(t => t == itemId);
        if (result == null || result == "")
        {
            return false;
        }
        return true;
    }

    public CandidateSpeakers GetSessionSpeakers()
    {
        return sessionSpeakers[currentDialogueSession-1];
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
