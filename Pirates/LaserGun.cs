using System.Globalization;
using System.Text;
using Pirates.DataStructures;

namespace Pirates;

public class LaserGun
{
    public double CurrentTimeInMinutes;
    private readonly double maxSpeed;
    private CyclicLinkList<Ship> ships;

    public LaserGun(double azimuth, double maxSpeed, List<Ship> pirates)
    {
        this.maxSpeed = maxSpeed;
        var startPosition = new Ship(azimuth, 0, 0, 0);
        pirates.Add(startPosition);
        pirates.Sort((x, y) => x.Azimuth.CompareTo(y.Azimuth));
        ships = new CyclicLinkList<Ship>(pirates);
    }


    public List<Ship>? Process()
    {
        var res = new List<Ship>();
        ships = ships.First(x => x.Value.Id == 0);
        while (!ships.IsSingleValue)
        {
            var min = ships
                .Where(x => x.Value.Id != ships.Value.Id)
                .Min(x => x.Value);

            var l = Math.Abs(ships.Left.Value.Azimuth - ships.Value.Azimuth);
            l = l <= 180 ? l : 360 - l;
            var r = Math.Abs(ships.Right.Value.Azimuth - ships.Value.Azimuth);
            r = r <= 180 ? r : 360 - r;
            var near = l < r ? ships.Left : ships.Right;

            var distanceToNear = Math.Abs(ships.Value.Azimuth - near.Value.Azimuth);
            distanceToNear = distanceToNear <= 180 ? distanceToNear : 360 - distanceToNear;

            var timeToNear = distanceToNear / (maxSpeed * 360);

            var distanceFromNearToMin = Math.Abs(min.Azimuth - near.Value.Azimuth);
            distanceFromNearToMin = distanceFromNearToMin <= 180 ? distanceFromNearToMin : 360 - distanceFromNearToMin;

            var timeFromNearToMin = distanceFromNearToMin / (maxSpeed * 360);
            var timeToNearAndMin = timeToNear + timeFromNearToMin;
            var next = timeToNearAndMin < min.TimeToTargetInHours ? near.Value : min;

            var offsetAngle = Math.Abs(next.Azimuth - ships.Value.Azimuth);

            if (offsetAngle <= 180)
            {
                res.Add(ships.Right!.Value);
                CurrentTimeInMinutes += offsetAngle / (maxSpeed * 360);
                if (CurrentTimeInMinutes > next.TimeToTargetInHours)
                {
                    return null;
                }

                ships = ships.Right;
                ships.RemoveLeft();
            }
            else
            {
                res.Add(ships.Left!.Value);
                CurrentTimeInMinutes += (360 - offsetAngle) / (maxSpeed * 360);
                if (CurrentTimeInMinutes > next.TimeToTargetInHours)
                {
                    return null;
                }

                ships = ships.Left;
                ships.RemoveRight();
            }
        }

        return res;
    }


    public string GetResultInString()
    {
        var res = Process();
        if (res is null) return "Impossible";
        var sb = new StringBuilder();
        sb.Append($"{CurrentTimeInMinutes.ToString("F3", CultureInfo.InvariantCulture)}\n");
        foreach (var ship in res)
        {
            sb.Append($"{ship.Id}\n");
        }

        return sb.ToString();
    }
}