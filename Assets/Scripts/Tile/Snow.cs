using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Snow : Tile
{
    public float _speed;
    public Transform _collidedObject;

    void Awake()
    {
        Init();
    }

    protected override void Create()
    {
        base.Create();
        _type = Type.Snow;
        _speed = 3;
    }

    IEnumerator WaitToCenter()
    {
        yield return new WaitForSeconds(1);
        _collidedObject.GetComponent<Player>().ToIdle();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        _collidedObject = other.transform;
        _collidedObject.DOMove(transform.position, 1);
        StartCoroutine(WaitToCenter());
        Player player = _collidedObject.GetComponent<Player>();
        if (player)
        {
            player.ToRun();
            player.OnSnow();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (_collidedObject.GetComponent<Player>())
            _collidedObject.GetComponent<Player>().IsOnSnow();

    }
}
