using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;   // Singleton

    [System.Serializable]
    public class Sound
    {
        public string name;                // ID for the sound
        public AudioClip clip;
        public bool loop;
        public float volume = 1.0f;
    }

    public Sound[] sounds;                 // Assign in Inspector
    private Dictionary<string, AudioSource> audioSources;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Create AudioSources dynamically
        audioSources = new Dictionary<string, AudioSource>();

        foreach (Sound s in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.loop = s.loop;
            source.volume = s.volume;
            audioSources.Add(s.name, source);
        }
    }

    public void Play(string name)
    {
        if (audioSources.ContainsKey(name))
            audioSources[name].Play();
        else
            Debug.LogWarning("Sound not found: " + name);
    }

    public void Stop(string name)
    {
        if (audioSources.ContainsKey(name))
            audioSources[name].Stop();
    }

    public void PlayOneShot(string name)
    {
        if (audioSources.ContainsKey(name))
            audioSources[name].PlayOneShot(audioSources[name].clip);
    }
}
