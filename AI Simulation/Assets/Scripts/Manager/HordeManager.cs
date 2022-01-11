using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : MonoBehaviour
{
    private static HordeManager _instance;
    public static HordeManager Instance { get { return _instance; } }

    private PlayerController player;

    private List<GameObject> hordeList = new List<GameObject>();
    private List<Vector3> formationPositionList = new List<Vector3>();

    private bool hordeSeesPlayer = false;

    private int[] ringPositionCount = { 5, 10, 20, 30, 34 };
    private float[] ringDistance = { 5f, 10f, 15f, 20f, 25f };

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

    private void Start()
    {
        player = PlayerController.Instance;
    }

    public void AddToHorde(GameObject zombie)
    {
        if (!hordeList.Contains(zombie))
        {
            hordeList.Add(zombie);
        }
    }

    public void RemoveFromHorde(GameObject zombie)
    {
        if (hordeList.Contains(zombie))
        {
            hordeList.Remove(zombie);
        }
    }

    public List<GameObject> GetHordeList()
    {
        return hordeList;
    }

    public void SendAlarmToHorde()
    {
        foreach (var zombie in hordeList)
        {
            zombie.GetComponent<ZombieBehaviour>().OverrideStage(EnumZombieBehaviour.CHASE);
            print($"Send command to zombie: {zombie.name}");
        }
    }

    public void SetPlayerSeesPlayerStatus(bool state)
    {
        hordeSeesPlayer = state;
    }

    public bool CheckIfHordeSeesPlayer()
    {
        return hordeSeesPlayer;
    }

    public void CheckIfAHordeMemberIsInRange()
    {
        foreach (var zombie in hordeList)
        {
            // check if a zombie is in range
            if (CheckIfIsInRange(zombie.transform, player.transform, 10f))
            {
                SetPlayerSeesPlayerStatus(true);
                return;
            }
        }
        SetPlayerSeesPlayerStatus(false);
    }

    private bool CheckIfIsInRange(Transform targetFrom, Transform targetTo, float maxRange)
    {
        //print($"Range: {(target.position - transform.position).sqrMagnitude} | {(maxRange * maxRange)}");
        if ((targetTo.position - targetFrom.position).sqrMagnitude < (maxRange * maxRange))
        {
            return true;
        }
        return false;
    }

    private List<Vector3> GetRingPosition(Vector3 center, float distance, int positionCount)
    {
        List<Vector3> ringPositionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * new Vector3(1, 0);
            Vector3 position = center + direction * distance;
            ringPositionList.Add(position);
        }

        return ringPositionList;
    }

    public bool CheckIfIsZombieLeader(GameObject zombie)
    {
        //print($"Zombie {zombie.name} is lieader: {hordeList[0] == zombie}");
        return hordeList[0] == zombie;
    }

    public void HordePatrolFormation()
    {
        foreach (var zombie in hordeList)
        {
            if (CheckIfIsZombieLeader(zombie))
            {
                ClearFormationPositionList();
                for (int i = 0; i < ringDistance.Length; i++)
                {
                    formationPositionList.AddRange(GetRingPosition(zombie.transform.position, ringDistance[i], ringPositionCount[i]));
                }
            }
        }
    }

    public List<Vector3> GetFormationList()
    {
        return formationPositionList;
    }

    public void ClearFormationPositionList()
    {
        formationPositionList.Clear();
    }

    public int GetIndexOfZombieInHordeList(GameObject zombie)
    {
        return hordeList.IndexOf(zombie);
    }
}
