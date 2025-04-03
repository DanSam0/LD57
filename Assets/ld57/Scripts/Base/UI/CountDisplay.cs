using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CountDisplay : MonoBehaviour
{

    [SerializeField] private string _prefix;

    private TextMeshProUGUI _display;


    private void Awake()
    {
        _display = GetComponent<TextMeshProUGUI>();
    }


    public void SetTime(float time) => _display.text = $"{time:F2}s";
    

    public void SetCount(int count) => _display.text = $"{count}{_prefix}";


    public void SetCount(float count) => _display.text = $"{count}{_prefix}";

}
