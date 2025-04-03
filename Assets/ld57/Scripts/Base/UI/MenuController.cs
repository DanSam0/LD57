using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;


public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject[] _firstAppearing;
    [SerializeField] private GameObject[] _disableAtClose;
    [SerializeField] private Button _firstSelectedBtn;
    [SerializeField] private bool _bindInput;


    private GameObject _currentSelected;
    private InputAction ia_menu;
    public bool isOpen = false;


    private void Start()
    {
        SetSelectable(_firstSelectedBtn.gameObject);

        if (!_bindInput) UpdateMenuState();
    }


    private void OnEnable()
    {
        if (!_bindInput) return;       

        ia_menu = InputSystem.actions.FindAction("Menu");
        ia_menu.started += OnMenu;
        _firstSelectedBtn.onClick.AddListener(UpdateMenuState);
    }


    private void OnDisable()
    {
        if (!_bindInput) return;

        ia_menu.started -= OnMenu;
        _firstSelectedBtn.onClick.RemoveListener(UpdateMenuState);
    }


    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != _currentSelected)
        {
            if(EventSystem.current.currentSelectedGameObject == null)
                StartCoroutine(SetSelectionOnFocus(_currentSelected));
            else
                SetSelectable(EventSystem.current.currentSelectedGameObject);
        }   
    }


    private void SetSelectable(GameObject selectedGO)
    {
        if(selectedGO != null)
        {
            _currentSelected?.GetComponent<ISelectableElement>()?.OnDeselected();
            _currentSelected = selectedGO;
            _currentSelected.GetComponent<ISelectableElement>()?.OnSelected();
        }        
    }


    private void OnMenu(CallbackContext callbackContext)
    {
        UpdateMenuState();
    }


    private void UpdateObjectsState(GameObject[] elements, bool setActive)
    {
        if(elements.Length == 0) return;

        foreach (GameObject element in elements)
        {
            element.SetActive(setActive);
        }
    }


    private void UpdateMenuState()
    {
        if (!isOpen)
        {
            isOpen = true;
            Game.Instance.Pause();

            UpdateObjectsState(_firstAppearing, true);
            UpdateObjectsState(_disableAtClose, false);

            StartCoroutine(SetSelectionOnFocus(_firstSelectedBtn.gameObject));
        }
        else
        {
            isOpen = false;
            Game.Instance.Resume();

            UpdateObjectsState(_firstAppearing, false);
        }
    }


    private IEnumerator SetSelectionOnFocus(GameObject objectToSelect)
    {
        yield return new WaitForEndOfFrame();

        SetSelectable(objectToSelect);
        EventSystem.current.SetSelectedGameObject(objectToSelect);
    }

}