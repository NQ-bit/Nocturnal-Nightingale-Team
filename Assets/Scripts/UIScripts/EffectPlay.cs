using System.Collections.Generic;
using Yarn.Unity;
using UnityEngine;

public class EffectPlay : MonoBehaviour
{
    [System.Serializable]
    public class SFXTrack
    {
        public string sfxName;
        public AudioClip audioName;
    }

    [Header("FireSFX")]
    [SerializeField] private SFXTrack[] sfxTracks;


    private static AudioSource sfxSource;
    private static Dictionary<string, AudioClip> sfxDictionary;

    void Start()
    {
        // Initialize static members
        sfxDictionary = new Dictionary<string, AudioClip>();

        // Create audio source
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;

        // Setup music dictionary
        foreach (SFXTrack track in sfxTracks)
        {
            if (!string.IsNullOrEmpty(track.sfxName) && track.audioName != null)
            {
                sfxDictionary[track.sfxName] = track.audioName;
            }
        }
    }

    [YarnCommand("play_sfx")]
    public static void PlayEffect(string trackName)
    {

        if (sfxSource == null)
        {
            Debug.LogError("[Yarn] Please ensure Effect it is present in the scene.");
            return;
        }

        if (sfxDictionary.TryGetValue(trackName, out AudioClip clip))
        {
            sfxSource.clip = clip;
            sfxSource.Play();
            Debug.Log($"[Yarn] Playing track: {trackName}");
        }
    }

    [YarnCommand("stop_SFX")]
    public static void StopSFX()
    {
        sfxSource.Stop();
        Debug.Log("[Yarn] Stopped music playback");
    }
}
