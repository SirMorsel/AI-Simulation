using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObstaclesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownTextField = null;

    // Start is called before the first frame update
    void Start()
    {
        ChangeCountdownUITextVisibility(false);
    }

    public void ChangeCountdownUITextVisibility(bool state)
    {
        countdownTextField.gameObject.SetActive(state);
    }

    public void SetCountdownValueOnTextField(float currentTimerValue)
    {
        countdownTextField.text = $"{currentTimerValue.ToString("N0")}";
    }
}
