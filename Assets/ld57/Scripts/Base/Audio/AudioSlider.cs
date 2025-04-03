using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class AudioSlider : MonoBehaviour
{

    [SerializeField] private string _audioGroupName;
    [SerializeField] private Slider _slider;
    [SerializeField] private UnityEvent<float> _onVolumeChange;


    private void Awake()
    {
        _slider.onValueChanged.AddListener(OnSliderUpdate);
    }


    private void Start()
    {
        if (Game.Instance.Preferences.HasKey(_audioGroupName))
        {
            _slider.SetValueWithoutNotify(Game.Instance.Preferences.GetFloat(_audioGroupName));
            _onVolumeChange.Invoke(Game.Instance.Preferences.GetFloat(_audioGroupName));
        }
        else
        {
            _slider.SetValueWithoutNotify(AudioLogic.DefaultVolume);
            _onVolumeChange.Invoke(AudioLogic.DefaultVolume);
        }
    }


    public void OnSliderUpdate(float value)
    {
        _onVolumeChange.Invoke(value);

        Game.Instance.audioLogic.SetMixerVolume(_audioGroupName, value);
    }


    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(OnSliderUpdate);
    }

}
