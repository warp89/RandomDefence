using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamer : MonoCache
{
    public bool go = false;
    [SerializeField] private float _timerMax;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private Transform _point;
    [SerializeField] private float _speedReboot;
    [SerializeField] private float _damage;
    [SerializeField] private Vector3 _halfOverBox;
    private float _timerM;
    private float _timer;
    private bool _flame = true;
    private float _speed;
    private int _level;
    public float Timer
    {
        get
        {
            return _timer;
        }

        set
        {
            _timer = value;
            if(_timer <= 0)
            {
                _flame = false;
            }
            else if(_timer >= _timerM)
            {
                _flame = true;
            }
        }
    }

    private void Awake()
    {
        _timerM = _timerMax;
        Timer = _timerM;
        _speed = _speedReboot;
    }

    public override void OnTick()
    {
        if(go == true)
        {
            if(SearchTarget())
            {
                if (_flame == true)
                {
                    Timer -= Time.deltaTime;
                }
                else
                {
                    Timer += Time.deltaTime * _speed;
                }
            }
            else
            {
                if(Timer < _timerM)
                {
                    Timer += Time.deltaTime * _speed;
                }
            }
        }
    }

    public override void OnFixedTick()
    {
        if(_flame == true)
        {
            FlameFire();
        }
    }

    /// <summary>
    /// ���������� ������ ������
    /// </summary>
    /// <returns></returns>
    private bool SearchTarget()
    {
        Collider[] targets = Physics.OverlapBox(_point.position, _halfOverBox, Quaternion.identity, _layer);
        if(targets.Length > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// ���������� �������������� ������
    /// </summary>
    /// <returns></returns>
    private void FlameFire()
    {
        Collider[] targets = Physics.OverlapBox(_point.position, _halfOverBox, Quaternion.identity, _layer);
        for(int i = 0; i < targets.Length; i++)
        {
            targets[i].GetComponent<EnemyHealth>().Health -= _damage;
            Debug.Log(targets[i].GetComponent<EnemyHealth>().Health);
        }
    }

    /// <summary>
    /// ���������� ��������
    /// </summary>
    public void Upgrade()
    {
        _level++;
        if(_level == 3)
        {
            _level = 2;
        }
        _damage = Base.CF[_level];
    }

    /// <summary>
    /// ���������� ����������� � ������� ����������
    /// </summary>
    public void Downgrade()
    {
        _level = 0;
        _damage = Base.CF[_level];
    }

    /// <summary>
    /// ���������� ��������� �������
    /// </summary>
    /// <param name="boost"></param>
    public void BoostTimer(float boost)
    {
        _speed = _speedReboot * ((1 - boost) + 1);
    }

    /// <summary>
    /// ���������� �������� �� ������� ����� �������
    /// </summary>
    public void ClassicTimer()
    {
        _speed = _speedReboot;
    }
}