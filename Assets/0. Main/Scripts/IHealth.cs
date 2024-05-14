using UnityEngine;

public interface IHealth
{
    public GameObject GameObject { get; }
    public HealthTeamSide HealthTeamSide { get; }
    public float MaxHealth { get; }
    public float CurrentHealth { get; }
    public void TakeDamge(float damage, HealthEventHandler evt);
    public void Regeneration(float regeneration, HealthEventHandler evt);
}

public class HealthEventHandler
{
    public GameObject caller;
    public HealthTeamSide teamSide;

    public string callerData;

    public HealthEventHandler(GameObject caller, string callerData)
    {
        this.caller = caller;
        this.callerData = callerData;
    }

    public HealthEventHandler(GameObject caller, HealthTeamSide teamSide)
    {
        this.caller = caller;
        this.teamSide = teamSide;
    }

    public HealthEventHandler() { }
}

[System.Serializable]
public enum HealthTeamSide
{
    None,
    A,
    B,
    C
}