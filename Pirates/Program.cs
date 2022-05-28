using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pirates
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var input = Console.ReadLine().Split();
            var az = decimal.Parse(input[0]);
            var speed = decimal.Parse(input[1]);
            var n = int.Parse(input[2]);
            var pirates = new List<Ship>(n);
            for (var i = 1; i <= n; i++)
            {
                pirates.Add(Ship.Parse(Console.ReadLine(), i));
            }

            var gun = new LaserGun(az, speed, pirates);
            try
            {
                Console.WriteLine(gun.GetResultInString());
            }catch{Console.WriteLine("Impossible");}
        }
    }

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

        public int CompareTo(object obj)
        {
            if (!(obj is Ship s)) throw new ArgumentException($"{obj} isn't {nameof(Ship)}");
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

        public override bool Equals(object obj)
        {
            return obj is Ship other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }

    public class CyclicLinkList<T> : IEnumerable<CyclicLinkList<T>>
    {
        public CyclicLinkList(IEnumerable<T> list)
        {
            foreach (var t in list)
            {
                Add(t);
            }
        }

        private CyclicLinkList()
        {
        }

        public T Value { get; set; }
        public CyclicLinkList<T> Left { get; private set; }
        public CyclicLinkList<T> Right { get; private set; }

        public bool IsEmpty => Left is null;

        public bool IsSingleValue => this == Left;

        public void RemoveRight()
        {
            if (IsEmpty) throw new InvalidOperationException("List is Empty");
            if (IsSingleValue)
            {
                Value = default;
                Left = Right = null;
            }
            else
            {
                Right!.Right!.Left = this;
                Right = Right.Right;
            }
        }

        public void RemoveLeft()
        {
            if (IsEmpty) throw new InvalidOperationException("List is Empty");
            if (IsSingleValue)
            {
                Value = default;
                Left = Right = null;
            }
            else
            {
                Left!.Left!.Right = this;
                Left = Left.Left;
            }
        }

        public void Add(T val)
        {
            if (Left is null || Right is null)
            {
                Value = val;
                Left = this;
                Right = this;
            }
            else
            {
                var res = new CyclicLinkList<T>
                {
                    Value = val,
                    Left = Left,
                    Right = this
                };
                Left.Right = res;
                Left = res;
            }
        }

        public IEnumerator<CyclicLinkList<T>> GetEnumerator()
        {
            if (IsEmpty)
                yield break;
            yield return this;

            var cur = Right;
            while (!cur!.Equals(this))
            {
                yield return cur;
                cur = cur.Right;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return $"Value: {Value}\n" +
                   $"Left: {Left.Value}\n" +
                   $"Right: {Right.Value}";
        }
    }

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
                if (timeToNear == 0)
                {
                    res.Add(near.Value);
                    near = near.Left;
                    near!.RemoveRight();
                }

                var timeFromNearToMin = near.Value.GetTimeToOther(min, maxSpeed);
                var timeToNearAndMin = timeToNear + timeFromNearToMin;
                var next = timeToNearAndMin + CurrentTimeInMinutes < min.TimeToTargetInMinutes ? near.Value : min;

                var offsetAngle = next.Azimuth - ships.Value.Azimuth;
                offsetAngle = offsetAngle >= 0 ? offsetAngle : offsetAngle + 360;


                if (offsetAngle <= 180)
                {
                    res.Add(ships.Right!.Value);
                    CurrentTimeInMinutes += offsetAngle / (maxSpeed * 360);
                    if (CurrentTimeInMinutes > next.TimeToTargetInMinutes)
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
                    if (CurrentTimeInMinutes > next.TimeToTargetInMinutes)
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
}