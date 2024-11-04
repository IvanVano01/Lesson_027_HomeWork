using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class JumpState : ICharacterState
{
    private Character _character;
    private CharacterStateSwitcher _characterStateSwitcher;   
    private CharacterView _characterView;
    private NavMeshAgent _navMeshAgent;
    private AnimationCurve _animationJumpCurve;

    private MonoBehaviour _fieldForStartCoroutine;
    private Coroutine _jumpCoroutine;

    private bool _isRunningState;    

    public JumpState(Character character, CharacterStateSwitcher characterStateSwitcher, NavMeshAgent navMeshAgent)
    {
        _character = character;
        _fieldForStartCoroutine = _character;
        _characterStateSwitcher = characterStateSwitcher;
        _navMeshAgent = navMeshAgent;
        _characterView = _character.View;
        _animationJumpCurve = _character.AnimationJupmCurve;
    }

    public void EnterState()
    {
        _isRunningState = true;

        _characterView.StartJumping();
    }

    public void ExitState()
    {
        _isRunningState = false;
        _characterView.StopJumping();
    }

    public void UpdadeState()
    {
        if (_isRunningState == false)
            return;

        if (_jumpCoroutine == null)
        {
            _jumpCoroutine = _fieldForStartCoroutine.StartCoroutine(Jump(_character.JumpDuration));            
        }           

        //Debug.Log($"Крутим состояние JumpState!");
    }

    private IEnumerator Jump(float duration)
    {
        OffMeshLinkData data = _navMeshAgent.currentOffMeshLinkData;
        Vector3 startPosition = _navMeshAgent.transform.position;
        Vector3 endPosition = data.endPos + Vector3.up * _navMeshAgent.baseOffset;

        float progress = 0f;

        while (progress < duration)
        {
            float yOffset = _animationJumpCurve.Evaluate(progress / duration);

            _navMeshAgent.transform.position = Vector3.Lerp(startPosition, endPosition, progress / duration) + yOffset * Vector3.up;
            _character.transform.rotation = Quaternion.LookRotation(endPosition - startPosition);

            progress += Time.deltaTime;
            yield return null;
        }        

        _navMeshAgent.CompleteOffMeshLink();
        _jumpCoroutine = null;
       
        if (_characterStateSwitcher.IsArrivedToTargetClick() || _characterStateSwitcher.GetPath() == false)
            _characterStateSwitcher.SetCharacterState(_characterStateSwitcher.NoMoveState);
        else
            _characterStateSwitcher.SetCharacterState(_characterStateSwitcher.MoveToTargetState);
    }
}
