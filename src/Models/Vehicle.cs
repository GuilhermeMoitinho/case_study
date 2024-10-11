using CaseLocaliza.Models.Abstractions;

namespace CaseLocaliza.Models;

public class Vehicle : Entity
{
    public string Mark { get; private set; }
    public SituationType Situation { get; private set; }
    public DateTime DateSituationChanged { get; private set; }

    public Vehicle(string mark, SituationType situationType)
    {
        Mark = mark;
        Situation = situationType;
    }

    public Vehicle() { }

    public enum SituationType
    {
        Available = 1,
        Maintenance,
        Rented,
        Disabled
    }

    public void UpdateSituation(SituationType situation)
    {
        Situation = situation;
        DateSituationChanged = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Vehicle Create(string mark, SituationType situationType)
            => new(mark, situationType);
}
