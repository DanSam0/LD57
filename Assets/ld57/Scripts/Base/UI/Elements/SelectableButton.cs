using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectableButton : MonoBehaviour, ISelectableElement
{

    [SerializeField] private TextMeshProUGUI _text;

    private Button _button;


    private void Awake()
    {
        _button = GetComponent<Button>();
    }


    public void OnSelected()
    {
        _text.color = _button.colors.selectedColor;
    }


    public void OnDeselected()
    {
        _text.color = _button.colors.normalColor;
    }   

}