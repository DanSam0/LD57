using System.Collections;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class CustomAnimator : MonoBehaviour
{

    [SerializeField] private FrameAnimation[] _animations;
    [SerializeField] private string _animationOnAwake;

    private int _curFrame = 0;
    private Coroutine _currentCoroutine;

    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_animations.Length < 0 )
            Debug.LogWarning("No animations to play");

        if (!string.IsNullOrEmpty(_animationOnAwake))
            TryPlayAnimation(_animationOnAwake);
    }


    public bool TryPlayAnimation(string name)
    {
        if (_animations.Length < 0) return false;

        for(int i = 0; i < _animations.Length; i++)
        {
            if (_animations[i].name == name)
            {
                PlayAnimation(i);
                return true;
            }                
        }

        return false;
    }


    private void PlayAnimation(int animationNum)
    {
        StopAnimation();

        _curFrame = 0;

        _currentCoroutine = StartCoroutine(Animate(animationNum));
    }


    public void StopAnimation()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
    }


    protected IEnumerator Animate(int animationNum)
    {
        _spriteRenderer.sprite = _animations[animationNum].frames[_curFrame];

        ++_curFrame;

        if (!_animations[animationNum].isLooped && _curFrame >= _animations[animationNum].frames.Length)
            yield break;

        if (_curFrame >= _animations[animationNum].frames.Length)
            _curFrame = 0;

        yield return new WaitForSeconds(_animations[animationNum].frameTime);

        _currentCoroutine = StartCoroutine(Animate(animationNum));
    }

}