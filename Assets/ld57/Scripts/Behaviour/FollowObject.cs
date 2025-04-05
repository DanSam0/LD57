using UnityEngine;

public class FollowObject : MonoBehaviour
{


    [SerializeField] private Transform _followedObject;
    [SerializeField] private float _smoothTime;

    private Vector2 _vel = Vector3.zero;


    private void LateUpdate()
    {
        Move();
    }


    private void Move()
    {
        Vector3 smoothedPos = Vector2.SmoothDamp(transform.position, _followedObject.position, ref _vel, _smoothTime);

        transform.position = new Vector3(smoothedPos.x, smoothedPos.y, transform.position.z);
    }


}