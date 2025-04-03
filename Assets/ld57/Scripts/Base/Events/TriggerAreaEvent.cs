using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class TriggerAreaEvent : MonoBehaviour
{

    [SerializeField] private string _triggerTag;
    [SerializeField] private bool _oneShot;    
    [SerializeField] private UnityEvent _triggerOnEnter;

    private bool _isTriggered = false;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Static;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_oneShot && _isTriggered) return;

        if(collision.CompareTag(_triggerTag))
        {
            _isTriggered = true;
            _triggerOnEnter.Invoke();
        }
    }

}