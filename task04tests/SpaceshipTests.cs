using Xunit;
using System;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        var cruiser = new Cruiser();
        
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower); 
        Assert.Equal(20, cruiser.Ammo);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        var fighter = new Fighter();
        
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(40, fighter.FirePower);
        Assert.Equal(40, fighter.Ammo);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void MoveForward_ShouldIncreaseDistanceTraveled()
    {
        var cruiser = new Cruiser();
        cruiser.MoveForward();
        cruiser.MoveForward();
        Assert.Equal(100, cruiser.DistanceTraveled);

        var fighter = new Fighter();
        fighter.MoveForward();
        Assert.Equal(100, fighter.DistanceTraveled);
    }

    [Fact]
    public void Rotate_ShouldModifyAngleCorrectly()
    {
        var fighter = new Fighter();
        
        fighter.Rotate(90);
        fighter.Rotate(45);
        Assert.Equal(135, fighter.CurrentAngle);

        fighter.Rotate(240);
        Assert.Equal(15, fighter.CurrentAngle);
    }

    [Fact]
    public void Fire_ShouldDecreaseAmmo()
    {
        var cruiser = new Cruiser();
        var fighter = new Fighter();

        cruiser.Fire();
        fighter.Fire();
        fighter.Fire();

        Assert.Equal(19, cruiser.Ammo);
        Assert.Equal(38, fighter.Ammo);
    }

    [Fact]
    public void Fire_AmmoShouldNotGoBelowZero()
    {
        var cruiser = new Cruiser();

        for (int i = 0; i < 25; i++)
        {
            cruiser.Fire();
        }

        Assert.Equal(0, cruiser.Ammo);
    }
}