using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SelectableSlider : MonoBehaviour, ISelectableElement
{

    [SerializeField] private Image _border;
    [SerializeField] private Image _fill;

    private Slider _slider;


    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }


    public void OnSelected()
    {
        _border.color = _slider.colors.selectedColor;
        _fill.color = new Vector4(
            _slider.colors.highlightedColor.r,
            _slider.colors.highlightedColor.g,
            _slider.colors.highlightedColor.b,
            _fill.color.a);
    }


    public void OnDeselected()
    {
        _border.color = _slider.colors.normalColor;
        _fill.color = new Vector4(
            _slider.colors.normalColor.r, 
            _slider.colors.normalColor.g, 
            _slider.colors.normalColor.b,
           _fill.color.a);
    }    

}
