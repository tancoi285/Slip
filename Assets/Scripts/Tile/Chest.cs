using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Tile
{
    private Animator _AC;
    private UnlockLevel _UL;
    private UIManager _UIM;
    private Player _player;
    private SoundManager _sound;

    void Awake()
    {
        Init();
    }

    protected override void Create()
    {
        base.Create();
        _type = Type.Chest;
        _AC = transform.GetComponent<Animator>();
        _UL = GameObject.FindWithTag("UnlockLvl").GetComponent<UnlockLevel>();
        _UIM = GameObject.FindWithTag("UI").GetComponent<UIManager>();
        _sound = GameObject.FindWithTag("Sound").GetComponent<SoundManager>();
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            _player = other.gameObject.GetComponent<Player>();
            _sound.Play(SoundManager.Sound.Clear);
            _sound.StopLoop();
            _AC.Play("Open");
        }
    }

    void Cleared() // gọi trong ani Open
    {
        _AC.Play("Idle");
        Time.timeScale = 0;
        _UIM.GameClear(_player);
        _UL.GetStar(_player);
        _UL.GetHighestLvl();
        _UL.Unlock();
        _UL.StarEarned();
    }
}
