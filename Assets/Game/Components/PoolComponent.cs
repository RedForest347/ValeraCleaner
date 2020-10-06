using RangerV;

[System.Serializable]
public class PoolComponent : ComponentBase
{
    [Pool]
    public int start_pool_size;
    [Pool]
    public int id = 0;


    public PoolComponent()
    {

    }
}

