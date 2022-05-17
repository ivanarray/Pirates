using System.Globalization;

namespace Pirates;

public class Pirate
{
    public Pirate(double azimuth, double distance, double speed, int id)
    {
        Id = id;
        Speed = speed;
        Azimuth = azimuth;
        startDistance = distance;
    }

    public static Pirate Parse(string s, int id)
    {
        var split = s.Split();

        var az = double.Parse(split[0], CultureInfo.InvariantCulture);
        var d = double.Parse(split[1], CultureInfo.InvariantCulture);
        var speed = double.Parse(split[2], CultureInfo.InvariantCulture);

        return new Pirate(az, d, speed, id);
    }

    private readonly double startDistance;
    public double Speed { get; }
    public double Azimuth { get; }
    public int Id { get; }

    public double GetCurrentDistance(double timeInHours = 0) => startDistance - Speed * timeInHours;
}