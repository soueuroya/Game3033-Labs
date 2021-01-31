using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Crosshair CrossHair => CrossHairComponent;
    [SerializeField]
   private Crosshair CrossHairComponent;


    public bool IsFiring;
    public bool IsReloading;
    public bool IsJumping;
    public bool IsRunning;
    
}
