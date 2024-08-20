namespace Project.Interfaces.Data
{
    public interface IAdvertismentData : IAdvertismentStatus, ISaveable
    {
        new bool IsAdActive { get; set; }
    }
}