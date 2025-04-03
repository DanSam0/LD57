using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;


public class AudioLogic : MonoBehaviour
{

    public static float VolumeDenominator = 0.01f;
    public static float DefaultVolume = 60f;


    [Header("Mixers")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _sfxMixer;
    [SerializeField] private AudioMixerGroup _musicMixer;
    [SerializeField] private string[] _mixerParameters = { "Master", "Music", "SFX" };


    [Header("Addressables")]
    [SerializeField] private string _sfxLabel = "SFX";
    [SerializeField] private string _musicLabel = "Music";


    private AudioSource _sourceMusic;

    private List<AudioClip> _sfxList = new();
    private List<AudioClip> _musicList = new();

    private Dictionary<string, float> _soundsFrequency = new();


    private void Awake()
    {
        _sourceMusic = gameObject.AddComponent<AudioSource>();
        _sourceMusic.volume = 1f;
        _sourceMusic.spatialBlend = 0f;
        _sourceMusic.loop = true;
        _sourceMusic.outputAudioMixerGroup = _musicMixer;

        Addressables.LoadAssetsAsync<AudioClip>(_sfxLabel, 
            asset => _sfxList.Add(asset))
            .Completed += (asyncOperationHandler) => Logger.OnAssetLoaded(asyncOperationHandler, _sfxLabel);           

        Addressables.LoadAssetsAsync<AudioClip>(_musicLabel,
            asset => _musicList.Add(asset))
            .Completed += (asyncOperationHandler) => Logger.OnAssetLoaded(asyncOperationHandler, _musicLabel);
    }


    private void Start()
    {
        SetVolumeOnStart();
    }


    private void SetVolumeOnStart()
    {
        foreach(string audioMixGroup in _mixerParameters)
        {
            if (Game.Instance.Preferences.HasKey(audioMixGroup))
            {
                SetMixerVolume(audioMixGroup, Game.Instance.Preferences.GetFloat(audioMixGroup));
            }
            else
            {
                SetMixerVolume(audioMixGroup, DefaultVolume);
            }
        }
    }


    public void SetMixerVolume(string groupName, float value)
    {
        Game.Instance.Preferences.SetValue(groupName, value.ToString());
        Game.Instance.Preferences.Save();

        _audioMixer.SetFloat(groupName, Mathf.Log10(0.0001f + (value * VolumeDenominator)) * 20);
    }


    public void PlaySound(string sound, Vector3 pos = new())
    {
        if (string.IsNullOrEmpty(sound)) return;

        AudioClip audioClip = _sfxList.Find(a => a.name == sound);

        if (audioClip != null)
            PlaySoundWithPos(audioClip, pos);
    }


    public void PlaySoundWithFrequency(string sound, Vector3 pos = new(), float frequency = 0.05f)
    {
        float time = Time.unscaledTime;

        if (_soundsFrequency.ContainsKey(sound))
        {
            if(time - frequency > _soundsFrequency[sound])
            {
                _soundsFrequency[sound] = time;
                PlaySound(sound, pos);
            }
        }
        else
        {
            _soundsFrequency.Add(sound, time);
            PlaySound(sound, pos);
        }
    }


    public void PlayMusic(string music)
    {
        if (string.IsNullOrEmpty(music)) return;

        AudioClip audioClip = _musicList.Find(a => a.name == music);

        if (audioClip != null) PlayMusicLoop(audioClip);
    }


    public bool HaveSound(string name)
    {
        return _sfxList.Find(a => a.name == name);
    }


    private void PlaySoundWithPos(AudioClip sound, Vector3 pos)
    {
        GameObject newAudio = new GameObject("TempSFX");
        newAudio.transform.position = pos;

        AudioSource newSource = newAudio.AddComponent<AudioSource>();
        newSource.clip = sound;
        newSource.outputAudioMixerGroup = _sfxMixer;
        newSource.spatialBlend = pos != Vector3.zero ? 1f : 0;

        newSource.Play();
        Destroy(newAudio, sound.length);
    }


    public void PlayMusicLoop(AudioClip music) 
    {
        _sourceMusic.Stop();
        _sourceMusic.clip = music;
        _sourceMusic.Play();
    }


    public void PauseMusic()
    {
        _sourceMusic.Pause();
    }


    public void ResumeMusic()
    {
        _sourceMusic.UnPause();

        if (!_sourceMusic.isPlaying)
            _sourceMusic.Play();
    }


}

