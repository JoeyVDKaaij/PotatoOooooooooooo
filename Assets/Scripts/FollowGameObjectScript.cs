using System;
using UnityEngine;

public class FollowGameObjectScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Tooltip("The GameObject that this GameObject follows.")]
    private GameObject _gameObject;
    [SerializeField, Tooltip("The GameObject that this GameObject follows.")]
    private Vector3 positionOffset = Vector3.zero;

    private void Start()
    {
        if (_gameObject != null) _gameObject = GameManager.instance.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        transform.position = _gameObject.transform.position + positionOffset;
    }
}
