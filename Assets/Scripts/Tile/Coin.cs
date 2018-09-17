using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Tile
{
    private SoundManager _sound;

    void Awake()
    {
        Init();
    }

    protected override void Create()
    {
        base.Create();
        _type = Type.Coin;
        _sound = GameObject.FindWithTag("Sound").GetComponent<SoundManager>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            gameObject.SetActive(false);
            _sound.Play(SoundManager.Sound.CollectCoin);
        }
    }
}
