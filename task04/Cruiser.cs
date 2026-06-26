using System;

public class Cruiser : ISpaceship
{
    public int Speed { get; } = 50;
    public int FirePower { get; } = 100;
    

    public int Ammo { get; private set; } = 20;
    public int DistanceTraveled { get; private set; }
    public int CurrentAngle { get; private set; }

    public void MoveForward()
    {
        DistanceTraveled += Speed;
    }

    public void Rotate(int angle)
    {
        CurrentAngle = (CurrentAngle + angle) % 360;
    }

    public void Fire()
    {
        if (Ammo > 0) Ammo--;
    }
}