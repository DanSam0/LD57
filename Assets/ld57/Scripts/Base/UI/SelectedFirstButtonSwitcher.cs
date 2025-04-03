using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedFirstButtonSwitcher : MonoBehaviour
{

    [SerializeField] GameObject _buttonToSet;

    private Button _currentButton;


    private void Awake()
    {
        _currentButton = GetComponent<Button>();    
    }


    private void OnEnable()
    {
        _currentButton.onClick.AddListener(SwitchButton);
    }


    private void OnDisable()
    {
        _currentButton.onClick.RemoveListener(SwitchButton);
    }


    private void SwitchButton() => EventSystem.current.SetSelectedGameObject(_buttonToSet);    

}