using UnityEngine;

public abstract class StateBase : MonoBehaviour
{
    void Start()
    {
        Enter();
    }

    void Update()
    {
        
    }

    void OnDisable()
    {
    }

    /// <summary>
    /// Called after entering a state with Unity's <b> Start() </b> method. 
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// Called before leaving a state with Unity's <b> OnDisable() </b> method.
    /// </summary>
    public abstract void Exit();
}
