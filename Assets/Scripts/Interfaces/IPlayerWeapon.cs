using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerWeapon
{
    public float FireRate {get; set;}
    public float Damage {get; set;}
    public float ReloadSpeed {get; set;}
    public int MaxCapacity {get; set;}
    public int Capacity {get; set;}
    public void Shoot();
    public void Reload();
}
