using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : Tile
{
    void Awake()
    {
        Init();
    }

    protected override void Create()
    {
        base.Create();
        _type = Type.Floor;
    }

}
