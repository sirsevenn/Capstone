using UnityEngine;

/// <summary>
/// Represents an abstract poolable object. Game objects to be used by the object pool should inherit this class.
/// By: NeilDG
/// </summary>
public abstract class Util_APoolable : MonoBehaviour
{
    protected Util_GameObjectPool poolRef;

    public void SetPoolRef(Util_GameObjectPool poolRef)
    {
        this.poolRef = poolRef;
    }

    public abstract void Initialize(); //initializes the property of this object.

    public abstract void Release(); //releases this object back to the pool and clean up any data.

    //events for APoolable Object
    public abstract void OnActivate(); //throws this event when this object has been activated from the pool.

}