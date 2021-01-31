using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : InputMonoBehaviour
{
    [Header("Weapon To Spawn"), SerializeField]
    private GameObject WeaponToSpawn;

    [SerializeField]
    private Transform WeaponSocketLocation;

    private Transform GripIKLocation;
    PlayerController PlayerController;
    Crosshair PlayerCrossHair;
    Animator PlayerAnimator;
    Camera ViewCamera;

    private new void Awake()
    {
        base.Awake();
        PlayerController = GetComponent<PlayerController>();
        PlayerAnimator = PlayerController.GetComponent<Animator>();
        if (PlayerController)
        {
            PlayerCrossHair = PlayerController.CrossHair;
        }

        ViewCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
      GameObject spawnedWeapon = Instantiate(WeaponToSpawn, WeaponSocketLocation.position, WeaponSocketLocation.rotation, WeaponSocketLocation);


        if (spawnedWeapon)
        {
            WeaponComponent weapon = spawnedWeapon.GetComponent<WeaponComponent>();
            if (weapon)
            {
                GripIKLocation = weapon.GripLocation;
            }
        }

    }

    private void OnAnimatorIK(int layerIndex)
    {
        PlayerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        PlayerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, GripIKLocation.position);
    }

    public void OnReload(InputValue pressed)
    {
        PlayerAnimator.SetBool("IsReloading", pressed.isPressed);
    }

    public void OnFire(InputValue pressed)
    {
        PlayerAnimator.SetBool("IsFiring", pressed.isPressed);
    }

    public void OnLookFix(InputAction.CallbackContext obj)
    {
        Vector3 independentMousePosition = ViewCamera.ScreenToViewportPoint(PlayerCrossHair.CurrentAimPosition);

        PlayerAnimator.SetFloat("AimHorizontal", independentMousePosition.x);
        PlayerAnimator.SetFloat("AimVertical", independentMousePosition.y);
    }

    private new void OnEnable()
    {
        base.OnEnable();
        GameInput.PlayerActionMap.Look.performed += OnLookFix;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        GameInput.PlayerActionMap.Look.performed -= OnLookFix;
    }
}
