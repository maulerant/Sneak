using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

public static class Tags
{
    public static string Food = "Eat";
    public static string Block = "Block";
}

public class Sneak : MonoBehaviour
{
    [SerializeField] private int _startBoneQuantity;
    [SerializeField] private GameObject _bonePrefab;
    [SerializeField] private List<GameObject> _tails;
    [SerializeField] private float _delay;
    [Range(0, 100)] [SerializeField] private float _speed;

    private Transform _transform;
    private float _headRotateAngle = 0f;
    private Vector3 _targetBonePosition;
    private float _currentDelay = 0f;
    private bool _isDead = false;

    public event UnityAction SnakeDead;
    public event UnityAction SnakeEatFood;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _delay /= 10;
        AddBones(_startBoneQuantity);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_isDead)
        {
            RotateSnake();
            MoveSnake();
        }
    }

    private void RotateSnake()
    {
        _transform.eulerAngles = new Vector3(0f, GetAngle(), 0f);
    }

    private float GetAngle()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _headRotateAngle = 90f;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _headRotateAngle = 270f;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _headRotateAngle = 0f;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _headRotateAngle = 180f;
        }

        return _headRotateAngle;
    }

    private void MoveSnake()
    {
        _targetBonePosition = _transform.position;
        _transform.Translate(0f, 0f, _speed * Time.deltaTime);
        _currentDelay += Time.deltaTime;
        if (_tails.Count > 0 && _currentDelay > _delay)
        {
            var bone = _tails[0];
            bone.transform.position = _targetBonePosition;
            _tails.RemoveAt(0);
            _tails.Add(bone);
            _currentDelay = 0f;
        }
    }

    private Vector3 MoveBone(Transform bone, Vector3 direction)
    {
        return bone.position + direction * _speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Tags.Food))
        {
            Destroy(other.gameObject);
            SnakeEatFood?.Invoke();
            AddBone();
        }

        if (other.gameObject.CompareTag(Tags.Block))
        {
            Die();
        }
    }

    private void AddBones(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddBone();
        }
    }

    private void AddBone()
    {
        var bone = Instantiate<GameObject>(_bonePrefab, _transform.position, Quaternion.identity);
        _tails.Add(bone);
    }

    private void Die()
    {
        _isDead = true;
        SnakeDead?.Invoke();
    }
}