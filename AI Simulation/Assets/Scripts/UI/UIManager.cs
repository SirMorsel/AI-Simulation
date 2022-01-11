using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    [SerializeField] private TMP_Text healthTextField = null;
    [SerializeField] private TMP_Text interactableTextField = null;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            ChangeInteractableUITextVisibility(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeInteractableUITextVisibility(bool state)
    {
        interactableTextField.gameObject.SetActive(state);
    }

    public void SetHealthTextFieldText(float currentHealth, float maxHealth)
    {
        print("aaaaaaaaaa");
        healthTextField.text = $"Health: {currentHealth} / {maxHealth}";
    }
}
