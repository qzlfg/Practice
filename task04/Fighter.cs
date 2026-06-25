using System;

public class Fighter : ISpaceship
{
    public int Speed { get; } = 100;
    public int FirePower { get; } = 40;

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
        Console.WriteLine($"Истребитель выпустил ракету с мощностью {FirePower}.");
    }
}