using System.Collections;
using Pirates.DataStructures;

namespace Pirates;

public class LaserGun
{
    private readonly double maxSpeed;
    private double currentAzimuth;
    private CyclicLinkList<Ship> ships;
    private SortedList<Ship, Ship> sortedShips;
    private readonly Ship startPosition;
    private double currentTimeInHours = 0;

    public LaserGun(double azimuth, double maxSpeed, List<Ship> pirates)
    {
        currentAzimuth = azimuth;
        this.maxSpeed = maxSpeed;
        startPosition = new Ship(azimuth, 0, 0, 0);
        pirates.Add(startPosition);
        pirates.Sort((x, y) => x.Azimuth.CompareTo(y.Azimuth));
        ships = new CyclicLinkList<Ship>(pirates);
        InitSortedShips();
    }

    private void InitSortedShips()
    {
        sortedShips = new();
        foreach (var ship in ships)
        {
            sortedShips.Add(ship, ship);
        }
    }

    public List<Ship> Process()
    {
        var res = new List<Ship>();


        return res;
    }

    public (double time, List<int> piratIds) GetResult()
    {
        return (0, null)!;
    }

    public string GetResultInString()
    {
        return string.Empty;
    }
}