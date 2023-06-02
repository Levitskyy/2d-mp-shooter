using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileWeapon : IPlayerWeapon
{  
    public float ProjectileSpeed {get; set;}
    public float ProjectileFlightDuration {get; set;}
}
