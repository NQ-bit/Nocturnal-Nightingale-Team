using System.Collections.Generic;
using Yarn.Unity;
using UnityEngine;

public class MusicSoundManager : MonoBehaviour
{
    #region Old Script
    //public static MusicSoundManager Instance;


    //[SerializeField]
    //private MusicLibrary musicLibrary;
    //[SerializeField]
    //private AudioSource musicSource;

    //private void Awake()
    //{
    //    if(Instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //}

    //public void PlayMusic(string trackName, float fadeDuration = 1.0f)
    //{
    //    StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));
    //}

    //IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 1.0f)
    //{
    //    float percent = 0;
    //    while (percent < 1)
    //    {
    //        percent += Time.deltaTime * 1 / fadeDuration;
    //        musicSource.volume = Mathf.Lerp(1f, 0, percent);
    //        yield return null;
    //    }

    //    musicSource.clip = nextTrack;
    //    musicSource.Play();

    //    percent = 0;
    //    while (percent < 1)
    //    {
    //        percent += Time.deltaTime * 1 / fadeDuration;
    //        musicSource.volume = Mathf.Lerp(0f, 1f, percent);
    //        yield return null;
    //    }
    //}
    #endregion

    [System.Serializable]
    public class FireTrack
    {
        public string IDName;
        public AudioClip audioClips;
    }

    [Header("Fire Loop")]
    [SerializeField] private FireTrack[] fireTracks;
    private static string lastPlayedTrack = ""; // Remember last played track


    private static AudioSource sfxSource;
    private static Dictionary<string, AudioClip> fireDictionary;

    void Start()
    {
        // Initialize static members
        fireDictionary = new Dictionary<string, AudioClip>();

        // Create audio source
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = true;

        // Setup music dictionary
        foreach (FireTrack track in fireTracks)
        {
            if (!string.IsNullOrEmpty(track.IDName) && track.audioClips != null)
            {
                fireDictionary[track.IDName] = track.audioClips;
            }
        }

        Debug.Log($"[SimpleMusicManager] Initialized with {fireTracks.Length} music tracks");
    }

    [YarnCommand("play_effect")]
    public static void PlayEffect(string trackName)
    {

        if (sfxSource == null)
        {
            Debug.LogError("[Yarn] MusicSoundManager not initialized. Please ensure it is present in the scene.");
            return;
        }

        if (fireDictionary.TryGetValue(trackName, out AudioClip clip))
        {
            sfxSource.clip = clip;
            sfxSource.Play();
            lastPlayedTrack = trackName; // Update last played track
            Debug.Log($"[Yarn] Playing track: {trackName}");
        }
        else
        {
            Debug.LogWarning($"[Yarn] Track not found: {trackName}");
            trackName = lastPlayedTrack;
            fireDictionary.TryGetValue(trackName, out AudioClip fallbackClip);
            sfxSource.clip = fallbackClip;
            sfxSource.Play();
        }
    }

    [YarnCommand("stop_fire")]
    public static void StopFire()
    {
        sfxSource.Stop();
        Debug.Log("[Yarn] Stopped music playback");
    }
}
