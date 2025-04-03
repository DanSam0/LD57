using UnityEngine;

public class FollowObjectUI : MonoBehaviour
{

    [SerializeField] private Vector3 _offset;
    public Transform followedObject;
    public bool isMoving;


    private void Start()
    {
        transform.parent = WorldCanvas.Instance.transform;
        transform.position = followedObject.position + _offset;
    }


    private void LateUpdate()
    {        
        if (isMoving)
            transform.position = followedObject.position + _offset;
    }

}
