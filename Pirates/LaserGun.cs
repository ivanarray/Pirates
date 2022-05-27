using System.Globalization;
using System.Text;
using Pirates.DataStructures;

namespace Pirates;

public class LaserGun
{
    private readonly double maxSpeed;
    private CyclicLinkList<Ship> ships;
    private double currentTimeInMinutes;

    public LaserGun(double azimuth, double maxSpeed, List<Ship> pirates)
    {
        this.maxSpeed = maxSpeed;
        var startPosition = new Ship(azimuth, 0, 0, 0);
        pirates.Add(startPosition);
        pirates.Sort((x, y) => x.Azimuth.CompareTo(y.Azimuth));
        ships = new CyclicLinkList<Ship>(pirates);
    }


    public List<Ship> Process()
    {
        var res = new List<Ship>();
        ships = ships.First(x => x.Value.Id == 0);
        while (!ships.IsSingleValue)
        {
            var min = ships.Where(x => x.Value.Id != 0).Min(x => x.Value);
            var offsetAngle = Math.Abs(min.Azimuth - ships.Value.Azimuth);
            if (offsetAngle <= 180)
            {
                res.Add(ships.Right!.Value);
                currentTimeInMinutes += offsetAngle / (maxSpeed * 360);
                ships = ships.Right;
                ships.RemoveLeft();
            }
            else
            {
                res.Add(ships.Left!.Value);
                currentTimeInMinutes += (360 - offsetAngle) / (maxSpeed * 360);
                ships = ships.Left;
                ships.RemoveRight();
            }
        }

        return res;
    }

    private double GetTimeInHoursTo(Ship left, Ship right) =>
        Math.Abs(left.Azimuth - right.Azimuth) / (maxSpeed * 360);


    public string GetResultInString()
    {
        var res = Process();
        var sb = new StringBuilder();
        sb.Append($"{currentTimeInMinutes.ToString("F3", CultureInfo.InvariantCulture)}\n");
        foreach (var ship in res)
        {
            sb.Append($"{ship.Id}\n");
        }

        return sb.ToString();
    }
}