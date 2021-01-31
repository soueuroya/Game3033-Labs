using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float JumpForce;

    private PlayerController PlayerController;
    private Animator PlayerAnimator;
    Transform PlayerTransform;

    private Vector2 InputVector = Vector2.zero;
    private Vector3 MoveDirection = Vector3.zero;
    private Rigidbody PlayerRigidbody;

    public readonly int MovementXHash = Animator.StringToHash("MovementX");
    public readonly int MovementYHash = Animator.StringToHash("MovementY");
    public readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
    public readonly int IsRunningHash = Animator.StringToHash("IsRunning");

    private void Awake()
    {
        PlayerTransform = transform;
        PlayerController = GetComponent<PlayerController>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    public void OnMovement(InputValue value)
    {
        InputVector = value.Get<Vector2>();

        PlayerAnimator.SetFloat(MovementXHash, InputVector.x);
        PlayerAnimator.SetFloat(MovementYHash, InputVector.y);
    }

    public void OnRun(InputValue value)
    {
        //InputVector = value.Get<Vector2>();

        //PlayerController.IsRunning = value.isPressed;
        PlayerAnimator.SetFloat(MovementXHash, InputVector.x);
        PlayerAnimator.SetFloat(MovementYHash, InputVector.y);
        
    }

    public void OnJump(InputValue value)
    {
        PlayerController.IsJumping = value.isPressed;
        PlayerAnimator.SetBool(IsJumpingHash, value.isPressed);
        PlayerRigidbody.AddForce((PlayerTransform.up + MoveDirection) * 3, ForceMode.Impulse);

    }


    private void Update()
    {
        MoveDirection = PlayerTransform.forward * InputVector.y + PlayerTransform.right * InputVector.x;

        if (PlayerController.IsJumping) return;
        if (!(InputVector.magnitude > 0)) return;

        float currentSpeed = PlayerController.IsRunning ? RunSpeed : WalkSpeed;
        Vector3 movementDirection = MoveDirection * (currentSpeed * Time.deltaTime);
        PlayerTransform.position += movementDirection;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            PlayerController.IsJumping = false;
            PlayerAnimator.SetBool(IsJumpingHash, false);
        }
    }


}
