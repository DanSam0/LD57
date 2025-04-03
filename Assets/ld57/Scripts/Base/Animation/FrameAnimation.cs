using UnityEngine;

[CreateAssetMenu(menuName = "Custom Animation/Frame Animation")]
public class FrameAnimation : ScriptableObject
{

    public float frameTime;
    public bool isLooped;
    public Sprite[] frames;

}