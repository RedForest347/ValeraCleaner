using UnityEngine;
using RangerV;

[Component("Engine/Physics component 2D", "Rigidbody Icon")]
public class Physics2DCmp : ComponentBase
{
    public Collider2D Collider;
    public Rigidbody2D Rigidbody;

    public Physics2DCmp()
    {
        Collider = null;
        Rigidbody = null;
    }

    public Physics2DCmp(GameObject GO)
    {
        Collider = GO.GetComponent<Collider2D>();
        Rigidbody = GO.GetComponent<Rigidbody2D>();
    }
}
