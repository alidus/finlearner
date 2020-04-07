using UnityEngine;

public abstract class AbstractController : MonoBehaviour
{
    private bool isActiveController;

    public virtual void Init()
    {

    }
    public void Activate()
    {
        isActiveController = true;
    }
    public void Deactivate()
    {
        isActiveController = false;

    }
}