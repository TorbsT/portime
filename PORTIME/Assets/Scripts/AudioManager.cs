using UnityEngine.Audio;
using System;  // Array.Find
using UnityEngine;


public class AudioManager : MonoBehaviour
{

    int cycle;

    public Sound[] sounds;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return; // no more code executed
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound '" + name + "' not found");
            return;
        }
        s.source.Play();
    }
    public void AdjustVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound '" + name + "' not found");
            return;
        }
        s.source.volume = volume;
    }

    private void Start()
    {
        Play("Portime1");
        Play("Portime2");
        Play("Portime3");
        cycle = 1;
    }

    private void Update()
    {
        if (cycle == 1)
        {
            AdjustVolume("Portime1", 1f);
            AdjustVolume("Portime2", 0f);
            AdjustVolume("Portime3", 0f);
        }
        if (cycle == 2)
        {
            AdjustVolume("Portime1", 0f);
            AdjustVolume("Portime2", 1f);
            AdjustVolume("Portime3", 0f);
        }
        if (cycle == 3)
        {
            AdjustVolume("Portime1", 0f);
            AdjustVolume("Portime2", 0f);
            AdjustVolume("Portime3", 1f);
        }

        if (Input.GetKeyDown("r"))
        {
            cycle++;
            if (cycle > 3)
                cycle = 1;
        }
    }
}
