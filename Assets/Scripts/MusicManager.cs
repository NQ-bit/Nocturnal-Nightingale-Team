using UnityEngine;
using Yarn.Unity;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public class MusicTrack
    {
        public string trackName;
        public AudioClip audioClip;
    }
    
    [Header("Music Tracks")]
    [SerializeField] private MusicTrack[] musicTracks;
    private static string lastPlayedTrack = ""; // Remember last played track

    
    private static AudioSource musicSource;
    private static Dictionary<string, AudioClip> musicDictionary;
    
    void Start()
    {
        // Initialize static members
        musicDictionary = new Dictionary<string, AudioClip>();
        
        // Create audio source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        
        // Setup music dictionary
        foreach (MusicTrack track in musicTracks)
        {
            if (!string.IsNullOrEmpty(track.trackName) && track.audioClip != null)
            {
                musicDictionary[track.trackName] = track.audioClip;
            }
        }
        
        Debug.Log($"[SimpleMusicManager] Initialized with {musicTracks.Length} music tracks");
    }
    
    [YarnCommand("play_music")]
    public static void PlayMusic(string trackName)
    {

        if (musicSource == null)
        {
            Debug.LogError("[Yarn] MusicManager not initialized. Please ensure it is present in the scene.");
            return;
        }

        if (musicDictionary.TryGetValue(trackName, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.Play();
            lastPlayedTrack = trackName; // Update last played track
            Debug.Log($"[Yarn] Playing music track: {trackName}");
        }
        else
        {
            Debug.LogWarning($"[Yarn] Music track not found: {trackName}");
            trackName = lastPlayedTrack;
            musicDictionary.TryGetValue(trackName, out AudioClip fallbackClip);
            musicSource.clip = fallbackClip;
            musicSource.Play();
        }
    }
    
    [YarnCommand("stop_music")]
    public static void StopMusic()
    {
        musicSource.Stop();
        Debug.Log("[Yarn] Stopped music playback");
    }
}