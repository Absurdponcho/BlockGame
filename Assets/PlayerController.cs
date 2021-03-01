using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera PlayerCamera;
    public Rigidbody Rigidbody;
    public MasterInput InputActions;
    public float LookSensitivity = 0.1f;
    public float MovementMultiplier = 1;
    public float HorizontalDrag = 1;
    public float JumpStrength = 10;

    float CameraPitch = 0;

    Vector2 StrafeValue;

    private void Awake()
    {
        InputActions = new MasterInput();
        InputActions.Player.Look.performed += (value) => {
            Vector2 lookValue = value.ReadValue<Vector2>();
            CameraPitch += -lookValue.y * LookSensitivity;

            if (CameraPitch < -90) CameraPitch = -90;
            if (CameraPitch > 90) CameraPitch = 90;

            PlayerCamera.transform.localEulerAngles = new Vector3(CameraPitch, 0, 0);
            transform.eulerAngles += new Vector3(0, lookValue.x * LookSensitivity, 0);
        };
        InputActions.Player.Strafe.performed += (value) =>
        {
            StrafeValue = value.ReadValue<Vector2>();
        };
        InputActions.Player.Strafe.canceled += (value) =>
        {
            StrafeValue = value.ReadValue<Vector2>();
        };
        InputActions.Player.Jump.performed += (value) =>
        {
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, JumpStrength, Rigidbody.velocity.z);
        };
    }

    private void Update()
    {
        Rigidbody.AddRelativeForce(new Vector3(StrafeValue.x, 0, StrafeValue.y) * MovementMultiplier * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Rigidbody.velocity -= new Vector3(Rigidbody.velocity.x * HorizontalDrag, 0, Rigidbody.velocity.z * HorizontalDrag);
    }

    private void OnEnable()
    {
        InputActions.Enable();
        InputActions.Player.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }
}
