using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    public GameObject ProjectileOwner {get; set;}
    public float Damage {get; set;}
    public Vector2 MoveVector {get; set;}
    public float FlightDuration {get; set;}
}
