using UnityEngine;

[System.Serializable]
public class CountdownTimer
{
    [SerializeField]
    private float maxValue;
    [SerializeField]
    private float currentValue;
    public float MaxValue
    { get {return maxValue;}}

    public float CurrentValue
    { get {return currentValue;}}

    public CountdownTimer(float maxValue)
    {
        this.maxValue = maxValue;
        Reset();
    }
    public CountdownTimer(float maxValue, float startValue)
    {
        this.maxValue = maxValue;
        this.currentValue = startValue;
    }
    public void Reset()
    {
        currentValue = MaxValue;
    }
    public bool Update(float delta){
        currentValue -=delta;
        return currentValue<0;
    }
    public bool HasExpired
    { get {return CurrentValue<0;}}
	
}