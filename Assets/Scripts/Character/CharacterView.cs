using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private const string TakeDamageTriggerKey = "TakeDamage";
    private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private readonly int IsJumpingKey = Animator.StringToHash("IsJumping");
    private readonly int IsIdlingKey = Animator.StringToHash("IsIdling");
    private readonly int IsDeadKey = Animator.StringToHash("IsDead");

    [SerializeField] private PlaySounds _playSounds;

    private int _woundedLayerIndex;

    private Animator _animator;

    public void Initialise()
    {
        _animator = GetComponentInParent<Animator>();
        _woundedLayerIndex = _animator.GetLayerIndex("WoundedMask");
    }

    public void StartIdling()
    {
        _animator.SetBool(IsIdlingKey, true);
    }

    public void StopIdling()
    {
        _animator.SetBool(IsIdlingKey, false);
    }

    public void StartRunning()
    {
        _animator.SetBool(IsRunningKey, true);

        if (_animator.GetLayerWeight(_woundedLayerIndex) == 0)
            _playSounds.OnFootSoundClip();

        if (_animator.GetLayerWeight(_woundedLayerIndex) == 1)
            _playSounds.OnFootDirtSoundClip();
    }

    public void StopRunning()
    {
        _animator.SetBool(IsRunningKey, false);
        _playSounds.OffFootSoundClip();  
    }

    public void StartJumping()
    {
        _animator.SetBool(IsJumpingKey, true);
        _playSounds.OnJumpSoundClip();
    }

    public void StopJumping()
    {
        _animator.SetBool(IsJumpingKey, false);
        _playSounds.OffJumpSoundClip();
    }

    public void StartWoundedWalk()
    {
        _animator.SetLayerWeight(_woundedLayerIndex, 1);        
    }

    public void StopWoundedWalk()
    {
        _animator.SetLayerWeight(_woundedLayerIndex, 0);
        _playSounds.OffFootSoundClip();
    }

    public void AnimationTakeDamage()
    {
        _animator.SetTrigger(TakeDamageTriggerKey);
    }

    public void StartAnimationDead()
    {
        _animator.SetBool(IsDeadKey, true);
    }

    public void StopAnimationDead()
    {
        _animator.SetBool(IsDeadKey, false);
    }
}
