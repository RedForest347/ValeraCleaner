using UnityEngine;
using RangerV;

[Component("Engine/GameObject component", "d_Prefab Icon")]
public class GameObjectCmp : ComponentBase
{
    public GameObject GameObject;
    public Vector3 previous_positon = new Vector3(0, 0, 0);
    public Vector3 current_position = new Vector3(0, 0, 0);
    public Vector3 velocity = new Vector3(0, 0, 0);

    public GameObjectCmp()
    {
        GameObject = null;
    }

    public GameObjectCmp(GameObject GO)
    {
        GameObject = GO;
    }
}
