using UnityEngine;

public class UnityInputService : IUnityInputService
{
    public float GetAxisRaw(string axisName) => Input.GetAxisRaw(axisName);
    public float GetDeltaTime() => Time.deltaTime;
}