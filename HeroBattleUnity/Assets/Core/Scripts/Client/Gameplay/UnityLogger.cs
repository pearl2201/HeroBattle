using UnityEngine;

public class UnityLogger : LiteEntitySystem.ILogger
{
    public void Log(string log)
    {
        Debug.Log(log);
    }

    public void LogError(string log)
    {
        Debug.LogError(log);
    }

    public void LogWarning(string log)
    {
        Debug.LogWarning(log);
    }
}
