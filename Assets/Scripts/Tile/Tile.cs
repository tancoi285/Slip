using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Type _type;
    private bool _isInit;

    protected virtual void Create() { }

    protected virtual void Init()
    {
        if (!_isInit)
        {
            Create();
            _isInit = true;
        }
    }

    static T Pool<T>(T sample, GameObject parent, ref List<T> lsObject) where T : MonoBehaviour
    {
        for (int i = 0; i < lsObject.Count; i++)
            if (!lsObject[i].gameObject.activeInHierarchy)
            {
                lsObject[i].gameObject.SetActive(true);
                return lsObject[i];
            }
        T newObject = Instantiate(sample);
        newObject.transform.SetParent(parent.transform);
        newObject.gameObject.SetActive(true);
        lsObject.Add(newObject);
        return newObject;
    }

    public static T CreateTile<T>(T sample, GameObject parent, ref List<T> lsTile) where T : MonoBehaviour
    {
        T newTile = Pool<T>(sample, parent, ref lsTile);
        return newTile;
    }

    public static GameObject Pool(GameObject sample, GameObject parent, ref List<GameObject> lsObject)
    {
        for (int i = 0; i < lsObject.Count; i++)
            if (!lsObject[i].gameObject.activeInHierarchy)
            {
                lsObject[i] = sample;
                lsObject[i].gameObject.SetActive(true);
                return lsObject[i];
            }
        GameObject newObject = Instantiate(sample);
        newObject.transform.SetParent(parent.transform);
        newObject.gameObject.SetActive(true);
        lsObject.Add(newObject);
        return newObject;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) { }

    protected virtual void OnCollisionEnter2D(Collision2D other) { }

    public enum Type
    {
        Floor = 1,
        Crack = 2,
        Ice = 3,
        Snow = 4,
        Water = 5,
        Chest = 6,
        Coin = 7,
        Player = 8
    }
}
