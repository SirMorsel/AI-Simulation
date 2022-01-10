using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance { get { return _instance; } }

    [SerializeField] private float walkSpeed = 5.0F;
    [SerializeField] private float runSpeed = 10.0F;
    [SerializeField] private float turnSpeed = 10.0F;
    // [SerializeField] private float jumpForce = 8.0F;
    // [SerializeField] private float gravity = 9.81F;

    private Transform playerModel;

    private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            playerModel = this.transform.GetChild(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if (movement != Vector3.zero)
        {
            float movementSpeed = walkSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = runSpeed;
            }
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, Quaternion.LookRotation(movement), Time.deltaTime * turnSpeed);
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
        }
    }
}
