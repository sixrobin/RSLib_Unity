using UnityEngine;

/// <summary>
/// Utility class to give more options on animators when accessing them through Unity events.
/// </summary>
[RequireComponent(typeof(Animator))]
public class AnimatorEventHelpers : MonoBehaviour
{
    private Animator _animator;
    
    public void SetBoolTrue(string parameterName)
    {
        this._animator.SetBool(parameterName, true);
    }
    
    public void SetBoolFalse(string parameterName)
    {
        this._animator.SetBool(parameterName, false);
    }

    private void Awake()
    {
        this._animator = this.GetComponent<Animator>();
    }
}
