using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header("links")]
    [SerializeField] private ExplosionViewFX _explosionViewFXPrefab;
    [SerializeField] private PlaySounds _playSounds;

    [Header("Configs")]
    [SerializeField] private float _strengthExplosion;
    [SerializeField] private int _damageExplosion;
    [SerializeField] private float _radiusDetonate;
    [SerializeField] private float _timeDetonateDelay;
    
    private Detonator _detonator;
    private Collider _collider;
    private Coroutine _timerToDetonateCoroutine;

    private bool _isDetonate;

    private void Start()
    {
        this.gameObject.SetActive(true);
        _collider = GetComponent<Collider>();       
    }

    private void Update()
    {
        if (_isDetonate == false)
            return;        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radiusDetonate);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDetonatable detonatable))
        {
            _collider.enabled = false;
            Vector3 explosionPosition = transform.position;

            _detonator = new Detonator(_strengthExplosion, _damageExplosion, explosionPosition, _radiusDetonate, _explosionViewFXPrefab);            

            _timerToDetonateCoroutine = StartCoroutine(StartTimerToDetonate(_timeDetonateDelay));            
        }
    }    

    private IEnumerator StartTimerToDetonate(float timerDetonateDelay)
    {
        _playSounds.OnSoundMineActive();

        yield return new WaitForSeconds(timerDetonateDelay);

        _detonator.CastExplosion();
        _playSounds.OnSoundExplosion();

        _isDetonate = false;
        this.gameObject.SetActive(false);
    }
}
