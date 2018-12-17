using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource1;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource efxSource2;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource efxSource3;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.


    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }


    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip, int source)
    {
        switch (source)
        {
            case 1:
                efxSource1.clip = clip;
                efxSource1.Play();
                break;
            case 2:
                efxSource2.clip = clip;
                efxSource2.Play();
                break;
            default:
                efxSource3.clip = clip;
                efxSource3.Play();
                break;
        }
    }

    public void StartMusic()
    {
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void Stop(int source)
    {
        switch (source)
        {
            case 1:
                efxSource1.Stop();
                break;
            case 2:
                efxSource2.Stop();
                break;
            default:
                efxSource3.Stop();
                break;
        }
    }
}