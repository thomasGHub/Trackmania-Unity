#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine;

[InitializeOnLoad]
#endif
public class CircleProcessor : InputProcessor<float>
{
#if UNITY_EDITOR
    static CircleProcessor()
    {
        Initialize();
    }
#endif

    [Tooltip("Max Camera Number")]
    public int maxValue = 3;

    private int index = 1;

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<CircleProcessor>();
    }

    public override float Process(float value, InputControl control)
    {
        ++index;
        if (index > maxValue)
            index %= maxValue;

        return index;
    }
}