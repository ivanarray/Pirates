using System.Globalization;
using System.Text;
using Pirates.DataStructures;

namespace Pirates;

public class LaserGun
{
    public decimal CurrentTimeInMinutes;
    private readonly decimal maxSpeed;
    private CyclicLinkList<Ship> ships;

    public LaserGun(decimal azimuth, decimal maxSpeed, List<Ship> pirates)
    {
        this.maxSpeed = maxSpeed;
        var startPosition = new Ship(azimuth, 0, 1, 0);
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

            var l = ships.Value.GetTimeToOther(ships.Left!.Value, maxSpeed);
            var r = ships.Value.GetTimeToOther(ships.Right!.Value, maxSpeed);
           
            var near = l < r ? ships.Left : ships.Right;

            var timeToNear = ships.Value.GetTimeToOther(near.Value, maxSpeed);

            var timeFromNearToMin = near.Value.GetTimeToOther(min, maxSpeed);
            var timeToNearAndMin = timeToNear + timeFromNearToMin;
            var next = timeToNearAndMin < min.TimeToTargetInMinutes ? near.Value : min;

            var offsetAngle = Math.Abs((ships.Value.Azimuth - next.Azimuth) % 360);

            if (offsetAngle <= 180)
            {
                res.Add(ships.Right!.Value);
                CurrentTimeInMinutes += offsetAngle / (maxSpeed * 360);
                if (CurrentTimeInMinutes >= next.TimeToTargetInMinutes)
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
                if (CurrentTimeInMinutes >= next.TimeToTargetInMinutes)
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