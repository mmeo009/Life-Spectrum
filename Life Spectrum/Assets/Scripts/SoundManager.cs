using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.SceneManagement;
using Unity.VisualScripting;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private List<SoundGroup> backGroundMusics = new List<SoundGroup>();
    [SerializeField] private List<SoundGroup> inGameSounds = new List<SoundGroup>();
    [SerializeField] private AudioSource AudioSource;

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

        AudioSource = this.AddComponent<AudioSource>();
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

        backGroundMusics.Add(new SoundGroup("Infancy", Infancy));
        backGroundMusics.Add(new SoundGroup("Adolescenece", Adolescenece));
        backGroundMusics.Add(new SoundGroup("Youth", Youth));
        backGroundMusics.Add(new SoundGroup("MiddleAge", MiddleAge));
        backGroundMusics.Add(new SoundGroup("Elderly", Elderly));

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

        inGameSounds.Add(new SoundGroup("GameEnd", GameOver));
        inGameSounds.Add(new SoundGroup("Paper", Paper));
        inGameSounds.Add(new SoundGroup("PopUp", PopUp));
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
    public List<AudioClip> audioClips;
    public SoundGroup(string name, List<AudioClip> list)
    {
        this.name = name;
        this.audioClips = list;
    }
}