using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : InputMonoBehaviour
{
    public Vector2 MouseSensitivity;
    public bool Inverted = false;

    public Vector2 CurrentAimPosition { get; private set; }

    [SerializeField, Range(0,1)]
    private float CrosshairHorizontalPercentage = 0.25f;
    private float HorizontalOffset;
    private float MaxHorizontalDeltaConstraint;
    private float MinHorizontalDeltaConstraint;

    [SerializeField, Range(0, 1)]
    private float CrosshairVerticalPercentage = 0.25f;
    private float VerticalOffset;
    private float MaxVerticalDeltaConstraint;
    private float MinVerticalDeltaConstraint;

    private Vector2 CrosshairStartingPosition;
    private Vector2 CurrentLookDeltas;

    // Start is called before the first frame update
   private void Start()
    {
        if (GameManager.Instance.CursorActive)
        {
            AppEvents.Invoke_OnMouseCursorEnable(false);
        }

        CrosshairStartingPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

        HorizontalOffset = (Screen.width * CrosshairHorizontalPercentage) / 2f;
        MinHorizontalDeltaConstraint = -(Screen.width / 2f) + HorizontalOffset;
        MaxHorizontalDeltaConstraint = (Screen.width / 2f) - HorizontalOffset;

        VerticalOffset = (Screen.height * CrosshairVerticalPercentage) / 2f;
        MinVerticalDeltaConstraint = -(Screen.height / 2f) + VerticalOffset;
        MaxVerticalDeltaConstraint = (Screen.height / 2f) - VerticalOffset;

    }


    private void OnLook(InputAction.CallbackContext delta)
    {
        Vector2 mouseDelta = delta.ReadValue<Vector2>();

        CurrentLookDeltas.x += mouseDelta.x * MouseSensitivity.x;
        if (CurrentLookDeltas.x >= MaxHorizontalDeltaConstraint || CurrentLookDeltas.x <= MinHorizontalDeltaConstraint)
        {
            CurrentLookDeltas.x -= mouseDelta.x * MouseSensitivity.x;
        }


        CurrentLookDeltas.y += mouseDelta.y * MouseSensitivity.y;
        if (CurrentLookDeltas.y >= MaxVerticalDeltaConstraint || CurrentLookDeltas.y <= MinVerticalDeltaConstraint)
        {
            CurrentLookDeltas.y -= mouseDelta.y * MouseSensitivity.y;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        float crosshairXPosition = CrosshairStartingPosition.x + CurrentLookDeltas.x;
        float crosshairYPosition = Inverted
            ? CrosshairStartingPosition.y - CurrentLookDeltas.y
            : CrosshairStartingPosition.y + CurrentLookDeltas.y;

        CurrentAimPosition = new Vector2(crosshairXPosition, crosshairYPosition);

        transform.position = CurrentAimPosition;
    }



    private new void OnEnable()
    {
        base.OnEnable();
        GameInput.PlayerActionMap.Look.performed += OnLook;
    }

 
    private new void OnDisable()
    {
        base.OnDisable();
        GameInput.PlayerActionMap.Look.performed -= OnLook;
    }
}
