using System.Globalization;

namespace Pirates;

public readonly struct Ship : IComparable<Ship>
{
    public Ship(double azimuth, double distance, double speed, int id)
    {
        Id = id;
        Speed = speed;
        Azimuth = azimuth;
        StartDistance = distance;
        TimeToTargetInHours = StartDistance / speed;
    }

    public static Ship Parse(string s, int id)
    {
        var split = s.Split();

        var az = double.Parse(split[0], CultureInfo.InvariantCulture);
        var d = double.Parse(split[1], CultureInfo.InvariantCulture);
        var speed = double.Parse(split[2], CultureInfo.InvariantCulture);

        return new Ship(az, d, speed, id);
    }

    public readonly double StartDistance;
    public readonly double TimeToTargetInHours;
    public readonly double Speed;
    public readonly double Azimuth;
    public readonly int Id;

    public override string ToString()
    {
        return Id == 0
            ? "Молниеносный"
            : $"Пират: \nId = {Id}\nАзимут = {Azimuth}\nСкорость = {Speed}миль/час\nРасстояние = {StartDistance}миль";
    }


    public int CompareTo(Ship other)
    {
        return TimeToTargetInHours.CompareTo(other.TimeToTargetInHours);
    }
}