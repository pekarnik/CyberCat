using UnityEngine;
using System.Collections;
using Assets.Scripts.MonoBehaviours.ChargeableItems;
using UnityEngine.AI;

public class MimicMovementController : MonoBehaviour
{
    NavMeshAgent _agent;
    Assets.Scripts.MonoBehaviours.ChargeableItems.Subject[] _targets;
    Assets.Scripts.MonoBehaviours.ChargeableItems.Subject _curTarget;
    NavMeshPath _path;
    bool _alreadyChoosed = false;
    ParticleSystem _pSystem;
    float _timeToDie = 2.0f;
    float _curTimeToDie = 0.0f;
    bool _particlePlayed = false;
    Animator _animator;
    [SerializeField]
    GameObject _activeMesh;
    [SerializeField]
    GameObject _hideMesh;
    // Use this for initialization
    void Start()
    {
        _path = new NavMeshPath();
        _agent = GetComponent<NavMeshAgent>();
        _targets = FindObjectsOfType<Assets.Scripts.MonoBehaviours.ChargeableItems.Subject>();
        _pSystem = GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
        
    }
    void ChooseTargetToCorrupt()
    {
        int curIndex = (int)Random.value * _targets.Length;
        _curTarget = _targets[curIndex];
        NavMesh.CalculatePath(transform.position, _curTarget.transform.position, NavMesh.AllAreas, _path);
    }
    // Update is called once per frame
    void Update()
    {
        if(!_alreadyChoosed)
        {
            _alreadyChoosed = true;
            ChooseTargetToCorrupt();
            _agent.SetPath(_path);
            _agent.SetDestination(_curTarget.transform.position);
        }
        else
        {
            if(Vector3.Distance(_agent.transform.position, _agent.destination) < 2.0f)
            {
                if (!_particlePlayed)
                {
                    _pSystem.Play();
                    _particlePlayed = true;
                    _animator.SetTrigger("OnDeath");
                }
                if(_curTimeToDie > _timeToDie)
                {
                    Debug.Log("ZARAZA");
                    _curTarget.ChangeState(SubjectStateComponent.States.Captured);
                    Destroy(gameObject);
                }
                _curTimeToDie += Time.deltaTime;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _agent.isStopped = true;
            _pSystem.Play();
            _activeMesh.SetActive(false);
            _hideMesh.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _agent.isStopped = false;
            _pSystem.Play();
            _activeMesh.SetActive(true);
            _hideMesh.SetActive(false);
        }
    }

    private Collider[] _hittedColliders = new Collider[15];
}
