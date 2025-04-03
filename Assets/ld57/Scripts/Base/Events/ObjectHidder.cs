using System.Collections;
using UnityEngine;

public class ObjectHidder : MonoBehaviour
{

    [SerializeField] private GameObject[] _objectsToDisableAfterStart;


    private void Start()
    {
        StartCoroutine(DisableAfterStart());
    }


    private IEnumerator DisableAfterStart()
    {
        yield return new WaitForEndOfFrame();

        if (_objectsToDisableAfterStart.Length > 0)
        {
            foreach (var element in _objectsToDisableAfterStart)
            {
                element.SetActive(false);
            }
        }

        Debug.Log("Objects hidden");
    }

}