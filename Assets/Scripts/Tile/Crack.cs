using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crack : Tile
{
    private MapManager _map;

    void Awake()
    {
        Init();
    }

    protected override void Create()
    {
        base.Create();
        _type = Type.Crack;
        _map = GameObject.FindWithTag("Map").GetComponent<MapManager>();                                                                                
    }

    void OnTriggerExit2D(Collider2D other)
    {
        gameObject.SetActive(false);
        _map.CreateWater(transform.position);
    }
}
