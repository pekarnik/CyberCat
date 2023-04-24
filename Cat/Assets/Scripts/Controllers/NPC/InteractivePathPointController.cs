using UnityEngine;

public class InteractivePathPointController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public InteractiveObjectClass objectToInteract;
    [SerializeField] public uint secondsToAwait = 3;
}
