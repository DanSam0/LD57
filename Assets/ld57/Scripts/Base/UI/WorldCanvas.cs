using UnityEngine;

public class WorldCanvas : MonoBehaviour
{

    public static WorldCanvas Instance;

    [SerializeField] private Canvas _canvas;


    private void Awake()
    {
        Instance = this; 
    }

}
