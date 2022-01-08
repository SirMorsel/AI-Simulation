using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    private static NoiseManager _instance;
    public static NoiseManager Instance { get { return _instance; } }

    private List<GameObject> objectsWithNoiseList = new List<GameObject>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void AddObjectToNoiseList(GameObject newObject)
    {
        if (!objectsWithNoiseList.Contains(newObject))
        {
            objectsWithNoiseList.Add(newObject);
        }
    }

    public void RemoveObjectToNoiseList(GameObject newObject)
    {
        if (objectsWithNoiseList.Contains(newObject))
        {
            objectsWithNoiseList.Remove(newObject);
        }
    }

    public List<GameObject> GetObjectsWithNoiseList()
    {
        return objectsWithNoiseList;
    }

}
