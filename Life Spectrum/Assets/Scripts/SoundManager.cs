using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private List<SoundGroup> backGroundMusics = new List<SoundGroup>();
    [SerializeField] private List<SoundGroup> inGameSounds = new List<SoundGroup>();
    public AudioMixer AudioMixer;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
         if(AudioMixer == null)
        {
            AudioMixer = Resources.Load<AudioMixer>("Sound/AudioMixer");
        }
        LoadSoundData();
    }
    public void LoadSoundData()
    {
        var bgSounds = Resources.LoadAll<AudioClip>("Sound/BackgroundMusic");
        var sfx = Resources.LoadAll<AudioClip>("Sound/SFX");

        List<AudioClip> Infancy = new List<AudioClip>();
        List<AudioClip> Adolescenece = new List<AudioClip>();
        List<AudioClip> Youth = new List<AudioClip>();
        List<AudioClip> MiddleAge = new List<AudioClip>();
        List<AudioClip> Elderly = new List<AudioClip>();

        foreach (var sound in bgSounds)
        {
            if (sound.name.Contains("Infancy"))
            {
                Infancy.Add(sound);
            }
            else if(sound.name.Contains("Adolescenece"))
            {
                Adolescenece.Add(sound);
            }
            else if(sound.name.Contains("Youth"))
            {
                Youth.Add(sound);
            }
            else if(sound.name.Contains("MiddleAge"))
            {
                MiddleAge.Add(sound);
            }
            else if (sound.name.Contains("Elderly"))
            {
                Elderly.Add(sound);
            }
            else
            {
                Debug.LogWarning("이름을 똑바로 적어주세요 ^^");
            }
        }

        backGroundMusics.Add(new SoundGroup("Infancy", Infancy, true));
        backGroundMusics.Add(new SoundGroup("Adolescenece", Adolescenece, true));
        backGroundMusics.Add(new SoundGroup("Youth", Youth, true));
        backGroundMusics.Add(new SoundGroup("MiddleAge", MiddleAge, true));
        backGroundMusics.Add(new SoundGroup("Elderly", Elderly, true));

        List<AudioClip> GameOver = new List<AudioClip>();
        List<AudioClip> Paper = new List<AudioClip>();
        List<AudioClip> PopUp = new List<AudioClip>();
        foreach(var sound in sfx)
        {
            if(sound.name.Contains("GameEnd"))
            {
                GameOver.Add(sound);
            }
            else if(sound.name.Contains("Paper"))
            {
                Paper.Add(sound);
            }
            else if(sound.name.Contains("PopUp"))
            {
                PopUp.Add(sound);
            }
            else
            {
                Debug.LogWarning("이름을 잘 만드세요 ^^");
            }
        }

        inGameSounds.Add(new SoundGroup("GameEnd", GameOver, false));
        inGameSounds.Add(new SoundGroup("Paper", Paper, false));
        inGameSounds.Add(new SoundGroup("PopUp", PopUp, false));
    }
    public void PlaySound(string name, bool isBGM) 
    {
        if(isBGM == true)
        {
            var sg = backGroundMusics.Find(sg =>sg.name == name);
            if(sg.audioClips.Count > 1)
            {

            }
        }
    }
}
[System.Serializable]
public class SoundGroup
{
    public string name;
    public List<Sound> audioClips = new List<Sound>();
    public SoundGroup(string name, List<AudioClip> list, bool isBGM)
    {
        this.name = name;
        foreach(AudioClip clip in list)
        {
            Sound sound = new Sound();
            sound.name = clip.name;
            sound.audioClip = clip;

            if (isBGM == true) 
            {
                sound.mixerGroup = SoundManager.Instance.AudioMixer.FindMatchingGroups("Master")[0];
            }
            else
            {
                sound.mixerGroup = SoundManager.Instance.AudioMixer.FindMatchingGroups("Master")[1];
            }

            audioClips.Add(sound);
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;

    public float volume = 1.0f;
    public float pitch = 1.0f;
    public bool loop;

    public AudioSource source;
    public AudioMixerGroup mixerGroup;
}