using UnityEngine;
using UnityEngine.Audio;

public class SoundMasterScript : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource music;
    public AudioSource deathsound, crushsound;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Play(string soundname)
    {
        switch (soundname)
        {
            case "death": deathsound.Play(); break;
            case "crush": crushsound.Play(); break;
        }
    }
}
