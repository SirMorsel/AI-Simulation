using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private List<GameObject> zombieList = new List<GameObject>();

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

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddZombieToZombieList(GameObject zombie)
    {
        if (!zombieList.Contains(zombie))
        {
            zombieList.Add(zombie);
        }
    }

    public void RemoveZombieFromZombieList(GameObject zombie)
    {
        if (zombieList.Contains(zombie))
        {
            zombieList.Remove(zombie);
        }
        if (zombieList.Count <= 0)
        {
            // All zombies defeated
            RestartScene();
        }
    }
}
