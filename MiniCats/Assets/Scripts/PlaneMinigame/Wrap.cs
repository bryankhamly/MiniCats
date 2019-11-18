using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrap : MonoBehaviour
{
    private Vector2 _screenPos;
    private Vector2 _screenExtents;
    private float _newX;
    private float _newY;
    private readonly Vector3 _halfUnit = Vector3.one / 2;
    private SpriteRenderer _sprite;
    private Camera _mainCam;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _mainCam = Camera.main;
    }

    void Update()
    {
        _screenPos = _mainCam.WorldToViewportPoint(_sprite.bounds.center);
        _screenExtents = (_mainCam.WorldToViewportPoint(_sprite.bounds.extents) - _halfUnit);

        if (_screenPos.x > 1.0f + _screenExtents.x)
        {
            _newX = -_screenExtents.x;
            _newY = _screenPos.y;
            SetNewPosition();
        }
        if (_screenPos.x < 0 - _screenExtents.x)
        {
            _newX = 1.0f + _screenExtents.x;
            _newY = _screenPos.y;
            SetNewPosition();
        }
        if (_screenPos.y > 1.0f + _screenExtents.y)
        {
            _newY = -_screenExtents.y;
            _newX = _screenPos.x;
            SetNewPosition();
        }
        if (_screenPos.y < 0 - _screenExtents.y)
        {
            _newY = 1.0f + _screenExtents.y;
            _newX = _screenPos.x;
            SetNewPosition();
        }
    }

    void SetNewPosition()
    {
        transform.position = _mainCam.ViewportToWorldPoint(new Vector3(_newX, _newY, 10));
    }
}
