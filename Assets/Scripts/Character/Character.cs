using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IDamageable, IDetonatable
{
    [Header("Links")]
    [SerializeField] private HealthView _healthView;

    [Header("Configs")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private AnimationCurve _animationJumpCurve;
    [SerializeField] private float _jumpDuration;

    private Health _health;
    private int _percentWoundedForSlowing = 30;

    private float _timeExplosionMax = 0.25f;   
    private Coroutine _timeExplosionDurationCoroutine;

    private CharacterView _view;

    private NavMeshAgent _navMeshAgent;
    private InputHandler _inputHandler;
    private ScreenClickHandler _screenClickHandler;
    private CharacterStateSwitcher _characterStateSwitcher;

    private Vector3 _blastWaveDirection = Vector3.zero;
    private float _detonateStrenght;    

    [field: SerializeField] public bool _isAlive { get; private set; }
    [field: SerializeField] public bool IsDetonate { get; private set; }

    public void Initialize(InputHandler inputHandler, ScreenClickHandler screenClickHandler, Health health)
    {
        _inputHandler = inputHandler;
        _screenClickHandler = screenClickHandler;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _maxSpeed;
        _view = GetComponentInChildren<CharacterView>();
        _view.Initialise();

        _health = health;
        _healthView.SetTextHealth(_health.Value);

        _isAlive = true;
        _view.StopWoundedWalk();
        _characterStateSwitcher = new CharacterStateSwitcher(_inputHandler, _screenClickHandler, this);
    }

    public Transform Transform => transform;
    public CharacterView View => _view;
    public CharacterStateSwitcher CharacterStateSwitcher => _characterStateSwitcher;
    public NavMeshAgent MeshAgent => _navMeshAgent;
    public AnimationCurve AnimationJupmCurve => _animationJumpCurve;
    public float JumpDuration => _jumpDuration;   

    private void Update()
    {
        if (_isAlive == false)
            return;

        _characterStateSwitcher.Update();

        if (IsDetonate)
        {
            MoveByBlastWave();
        }
    }

    public void TakeDamage(int damage)
    {
        _health.Reduce(damage);
        _healthView.SetTextHealth(_health.Value);

        if (_health.Value <= 0)
        {
            ToDie();

            return;
        }

        if (_health.Value < (_health._maxHealth * _percentWoundedForSlowing) / 100)
        {
            float SlowSpeed = _maxSpeed / 3f;

            _navMeshAgent.speed = SlowSpeed;
            _view.StartWoundedWalk();
        }

        _characterStateSwitcher.SetCharacterState(_characterStateSwitcher.TakeDamageState);
        Debug.Log($" У игрока осталось здоровья {_health}");
    }

    public void OnDetonate(Vector3 detonateDirection, float detonateStrenght, int damage)
    {
        TakeDamage(damage);

        IsDetonate = true;
        
        _blastWaveDirection = detonateDirection;
        _detonateStrenght = detonateStrenght;
    }

    private void MoveByBlastWave()
    { 
        Vector3 direction = _blastWaveDirection - transform.position;
        transform.Translate(direction * _detonateStrenght / 2 * Time.deltaTime, Space.World);
        
        _timeExplosionDurationCoroutine = StartCoroutine(StartTimerExplosion(_timeExplosionMax));
    }

    private IEnumerator StartTimerExplosion(float duration)
    {
        yield return new WaitForSeconds(duration);

        IsDetonate = false;
    }

    private void ToDie()
    {
        GetComponentInChildren<Collider>().enabled = false;

        _view.StopWoundedWalk();

        _characterStateSwitcher.SetCharacterState(_characterStateSwitcher.DyingState);

        _isAlive = false;

        Debug.Log($" Игрок мертв! Уровень здоровья = {_health}");
    }
}
