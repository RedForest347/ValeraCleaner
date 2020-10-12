using UnityEngine;
using RangerV;

[Component("Engine/Physics component", "Rigidbody Icon")]
public class PhysicsCmp : ComponentBase
{
    public Collider Collider;
    public Rigidbody Rigidbody;

    public PhysicsCmp()
    {
        Collider = null;
        Rigidbody = null;
    }

    public PhysicsCmp(GameObject GO)
    {
        Collider = GO.GetComponent<Collider>();
        Rigidbody = GO.GetComponent<Rigidbody>();
    }
}
