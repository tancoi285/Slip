using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Tile
{
    void Awake()
    {
        Init();
    }

    protected override void Create()
    {
        base.Create();
        _type = Type.Water;
    }

}
