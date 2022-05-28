using System.Globalization;
using System.Runtime.CompilerServices;

namespace Pirates;

public readonly struct Ship : IComparable<Ship>, IEquatable<Ship>, IComparable
{
    public Ship(decimal azimuth, decimal distance, decimal speed, int id)
    {
        Id = id;
        Speed = speed;
        Azimuth = azimuth;
        StartDistance = distance - 1;
        TimeToTargetInMinutes = StartDistance / speed * 60;
    }

    public static Ship Parse(string s, int id)
    {
        var split = s.Split();

        var az = decimal.Parse(split[0], CultureInfo.InvariantCulture);
        var d = decimal.Parse(split[1], CultureInfo.InvariantCulture);
        var speed = decimal.Parse(split[2], CultureInfo.InvariantCulture);

        return new Ship(az, d, speed, id);
    }

    public readonly decimal StartDistance;
    public readonly decimal TimeToTargetInMinutes;
    public readonly decimal Speed;
    public readonly decimal Azimuth;
    public readonly int Id;

    public override string ToString()
    {
        return Id == 0
            ? "Молниеносный"
            : $"Пират: \nId = {Id}\nАзимут = {Azimuth}" +
              $"\nСкорость = {Speed}миль/час" +
              $"\nРасстояние = {StartDistance}миль" +
              $"\nВремя до захвата {TimeToTargetInMinutes} мин";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal GetTimeToOther(Ship other, decimal gunsSpeed)
    {
        var distance = Math.Abs(Azimuth - other.Azimuth);
        distance = distance <= 180 ? distance : 360 - distance;

        return distance / (gunsSpeed * 360);
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Ship s) throw new ArgumentException($"{obj} isn't {nameof(Ship)}");
        return CompareTo(s);
    }


    public int CompareTo(Ship other)
    {
        return TimeToTargetInMinutes.CompareTo(other.TimeToTargetInMinutes);
    }

    public bool Equals(Ship other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Ship other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id;
    }
}