using Xunit;
using System;

public class SpaceshipTests
{

    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(90, cruiser.FirePower); 
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(40, fighter.FirePower);
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
}