using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public string lastSceneName = "";

    public float transitionTime = 1f;
    private double nextEventTime;
    public AudioSource auctionAudioSource;
    public AudioSource dialogueAudioSource;
    public AudioSource SFXAudioSource;

    public AudioMixerSnapshot auctionSnapshot;
    public AudioMixerSnapshot dialogueSnapshot;
    //public AudioMixerSnapshot livingRoomSnapshot;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip sfx)
    {
        Debug.Log("play sfx "+sfx.name);
        SFXAudioSource.loop = false;
        SFXAudioSource.PlayOneShot(sfx);

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("Auction") || scene.name.Equals("End"))
        {
            if (lastSceneName.Equals("Auction") || lastSceneName.Equals("End"))
            {
                return;
            }
            
            auctionSnapshot.TransitionTo(transitionTime);
            nextEventTime = (AudioSettings.dspTime + transitionTime);

            auctionAudioSource.loop = true;
            auctionAudioSource.PlayScheduled(nextEventTime);
        }
        else if (scene.name.Equals("Dialogue") || scene.name.Equals("Start"))
        {
            if (lastSceneName.Equals("Dialogue") || lastSceneName.Equals("Start"))
            {
                return;
            }
            dialogueSnapshot.TransitionTo(0.5f);
            nextEventTime = (AudioSettings.dspTime + 0.5f);
            dialogueAudioSource.loop = true;
            dialogueAudioSource.PlayScheduled(nextEventTime);
        }

        lastSceneName = scene.name;
    }

}
