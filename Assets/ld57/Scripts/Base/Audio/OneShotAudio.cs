using System;
using UnityEngine;

public class OneShotAudio : MonoBehaviour
{

    [SerializeField] protected string _sound;
    [SerializeField] protected float _frequency;
    [SerializeField] protected bool _playOnAwake;


    private void Awake()
    {
        if (_playOnAwake)
            PlaySound();
    }


    public void PlaySound()
    {
        if (string.IsNullOrEmpty(_sound)) return;
        
        Game.Instance.audioLogic.PlaySoundWithFrequency(_sound, transform.position, _frequency);                      
    }

}
