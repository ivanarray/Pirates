namespace Pirates;

public class  LaserGun
{
    private readonly double maxSpeed;
    private double currentAzimuth;
    private IList<Pirate> pirates;

    public LaserGun(double azimuth, double maxSpeed, IList<Pirate> pirates)
    {
        currentAzimuth = azimuth;
        this.maxSpeed = maxSpeed;
        this.pirates = pirates;
    }
    
    public string Calculate()
    {
        return string.Empty;
    }
}