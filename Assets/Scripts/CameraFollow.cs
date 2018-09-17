using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private MapManager _map;
    public Player _player;
    private bool _isOneTime;
    private float _xMax;
    private float _yMin;

    void Start()
    {
        _map = GameObject.FindWithTag("Map").GetComponent<MapManager>();
    }

    void MoveCamera()
    {
        if (_player)
        {
            transform.position = new Vector3(Mathf.Clamp(_player.transform.position.x, 0, _xMax),
                Mathf.Clamp(_player.transform.position.y, _yMin, 0), transform.position.z);
        }
    }

    public void Limit(Vector3 maxTile)
    {
        //Vector3 wp = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));
        float height = Camera.main.orthographicSize;
        float width = Camera.main.aspect * height;
        //_xMax = maxTile.x - wp.x + 0.48f;
        //_yMin = maxTile.y - wp.y - 0.48f;
        _xMax = maxTile.x - width + 0.48f;
        _yMin = maxTile.y + height - 0.48f;
    }

    void Update()
    {
        if (!_isOneTime && _map.isCreated) 
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            _isOneTime = true;
        }
    }

    void FixedUpdate()
    {
        if (_map.isCreated)
            MoveCamera();
    }
}
