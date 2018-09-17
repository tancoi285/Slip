using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevel : MonoBehaviour
{
    private static int _level;
    private int _presentLvl;
    private int _highestLvl;
    private string _highestLvlKey;
    private int[] _levelStar;
    private string[] _levelKey;
    private GameObject[] _lock;
    private GameObject[] _lvlStarSet;

    void Start()
    {
        FirstSetting();
    }

    void FirstSetting()
    {
        _level = 15;
        _levelStar = new int[_level];
        _levelKey = new string[_level];
        _lock = new GameObject[_level];
        _lvlStarSet = new GameObject[_level];
        _highestLvlKey = "HighestLvl";
        GetLvlKey();
        GetSave();
    }

    void GetSave()
    {
        _highestLvl = PlayerPrefs.GetInt(_highestLvlKey, 1);
        for (int i = 0; i < _level; i++)
            _levelStar[i] = PlayerPrefs.GetInt(_levelKey[i], 0);
    }

    void GetLvlKey()
    {
        for (int i = 0; i < _levelKey.Length; i++)
            _levelKey[i] = "Lvl_" + (i + 1).ToString();
    }

    public void GetLock(int index, Button btnLvl)
    {
        _lock[index] = btnLvl.transform.Find("lock").gameObject;
        _lvlStarSet[index] = btnLvl.transform.Find("StarSet").gameObject;
    }

    public void GetHighestLvl()
    {
        if (_presentLvl == _highestLvl)
        {
            _highestLvl += 1;
            PlayerPrefs.SetInt(_highestLvlKey, _highestLvl);
            PlayerPrefs.Save();
        }
    }

    public void GetStar(Player player)
    {
        if (player.starEarned > _levelStar[_presentLvl - 1])
        {
            _levelStar[_presentLvl - 1] = player.starEarned;
            PlayerPrefs.SetInt(_levelKey[_presentLvl - 1], _levelStar[_presentLvl - 1]);
            PlayerPrefs.Save();
        }
    }

    public void StarEarned()
    {
        for (int i = 0; i < _lvlStarSet.Length; i++)
            for (int j = 0; j < _levelStar[i]; j++)
                _lvlStarSet[i].transform.GetChild(j).gameObject.SetActive(true);
    }

    public void Unlock()
    {
        for (int i = 0; i < _highestLvl; i++)
            _lock[i].SetActive(false);
    }

    public int presentLvl
    {
        get { return _presentLvl; }
        set { _presentLvl = value; }
    }

    public int highestLvl
    {
        get { return _highestLvl; }
        set { _highestLvl = value; }
    }

    public int level
    {
        get { return _level; }
        set { _level = value; }
    }
}
