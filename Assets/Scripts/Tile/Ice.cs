using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Tile
{
    void Awake()
    {
        Init();
    }

    protected override void Create()
    {
        base.Create();
        _type = Type.Ice;
    }
}
