using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private UnlockLevel _UL;
    private GameObject _tile;
    private Dictionary<Tile.Type, GameObject> _dicTile = new Dictionary<Tile.Type, GameObject>();
    private Dictionary<Tile.Type, List<Tile>> _dicLsTile = new Dictionary<Tile.Type, List<Tile>>();
    private Vector3 _startPos;
    private string[] _mapData;
    private int _mapSizeX;
    private int _mapSizeY;
    private bool _isCreated;
    

    void Start()
    {
        _tile = (GameObject)Resources.Load("Prefabs/Floor");
        _UL = GameObject.FindWithTag("UnlockLvl").GetComponent<UnlockLevel>();
        GetStartPos();
        AddTile();
        //CreateMap();
    }

    void AddTile()
    {
        foreach (Tile.Type type in Enum.GetValues(typeof(Tile.Type)))
        {
            _dicTile.Add(type, (GameObject)Resources.Load("Prefabs/" + type.ToString()));
            _dicLsTile.Add(type, new List<Tile>());
        }
    }

    void GetStartPos()
    {
        _startPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        _startPos = new Vector3(_startPos.x + tileSize * 3 / 2, _startPos.y - tileSize * 3 / 2);
    }

    string[] ReadLevelText()
    {
        //TextAsset bindData = Resources.Load<TextAsset>("Level/lvl2");
        TextAsset bindData = Resources.Load<TextAsset>("Level/lvl" + _UL.presentLvl);
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        data = data.Replace(','.ToString(), string.Empty);
        return data.Split('-');
    }

    void LoadMap()
    {
        _mapData = ReadLevelText();
        _mapSizeX = _mapData[0].ToCharArray().Length;
        _mapSizeY = _mapData.Length;
    }

    //GameObject CreateTile(string tileType, int x, int y)
    //{
    //    GameObject newTile;
    //    int tileID = int.Parse(tileType);
    //    if (tileID != (int)Tile.Type.Floor)
    //    {
    //        GameObject newFloor = Tile.Pool(_dicTile[Tile.Type.Floor], gameObject, ref _lsTile);
    //        newFloor.transform.position = new Vector3(_startPos.x + tileSize * x * tileScale,
    //            _startPos.y - tileSize * y * tileScale, 0);
    //        newTile = Tile.Pool(_dicTile[(Tile.Type)tileID], gameObject, ref _lsTile);
    //    }
    //    else newTile = Tile.Pool(_dicTile[Tile.Type.Floor], gameObject, ref _lsTile);
    //    return newTile;
    //}

    Tile CreateTile(string tileType, int x, int y)
    {
        Tile newTile;
        List<Tile> lsTile;
        int tileID = int.Parse(tileType);
        if (tileID != (int)Tile.Type.Floor)
        {
            lsTile = _dicLsTile[Tile.Type.Floor];
            Tile newFloor = Tile.CreateTile<Tile>(_dicTile[Tile.Type.Floor].GetComponent<Tile>(), gameObject, ref lsTile);
            lsTile = _dicLsTile[Tile.Type.Floor];
            newFloor.transform.position = new Vector3(_startPos.x + tileSize * x * tileScale,
                _startPos.y - tileSize * y * tileScale, 0);
            lsTile = _dicLsTile[(Tile.Type)tileID];
            newTile = Tile.CreateTile<Tile>(_dicTile[(Tile.Type)tileID].GetComponent<Tile>(), gameObject, ref lsTile);
        }
        else
        {
            lsTile = _dicLsTile[Tile.Type.Floor];
            newTile = Tile.CreateTile<Tile>(_dicTile[Tile.Type.Floor].GetComponent<Tile>(), gameObject, ref lsTile);
            _dicLsTile[Tile.Type.Floor] = lsTile;
        }
        return newTile;
    }

    public void CreateMap()
    {
        LoadMap();
        Vector3 maxTile = Vector3.zero;
        //int count = 0;
       // Debug.Log("LENG" + newTiles.Length);
        for (int y = 0; y < _mapSizeY; y++)
        {
            char[] newTiles = _mapData[y].ToCharArray();
            //Debug.Log("LENG " + y);
            if (_mapData[y].Length > 20)
                _mapData[y] = _mapData[y].Remove(0, 1);
            for (int x = 0; x < _mapSizeX; x++)
            {
                //count++;
               // Debug.Log(count + "-" + x + ":" + _mapData[y][x]);
                Tile newTile = CreateTile(_mapData[y][x].ToString(), x, y);
              //  Debug.Log(count + ": " + newTile);
                newTile.transform.position = new Vector3(_startPos.x + tileSize * x * tileScale,
                    _startPos.y - tileSize * y * tileScale, 0);
                maxTile = newTile.transform.position;
            }
        }
        Camera.main.GetComponent<CameraFollow>().Limit(maxTile);
        _isCreated = true;
    }

    public void CreateWater(Vector3 pos)
    {
        List<Tile> lsWater = _dicLsTile[Tile.Type.Water];
        Tile newWater = Tile.CreateTile<Tile>(_dicTile[Tile.Type.Water].GetComponent<Water>(), gameObject, ref lsWater);
        newWater.transform.position = pos;
        _dicLsTile[Tile.Type.Water] = lsWater;
    }

    public void DeactiveAll()
    {
        int count = 0;
        foreach (Tile.Type type in Enum.GetValues(typeof(Tile.Type)))
            for (int i = 0; i < _dicLsTile[type].Count; i++)
            {
                _dicLsTile[type][i].gameObject.SetActive(false);
                count += 1;
            }
        print(count);
        _isCreated = false;
        Player player = _dicLsTile[Tile.Type.Player][0].GetComponent<Player>();
        player.Reset();
    }

    public float tileSize
    {
        get { return _tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    public float tileScale
    {
        get { return _tile.transform.localScale.x; }
    }

    public bool isCreated
    {
        get { return _isCreated; }
        set { _isCreated = value; }
    }
}
