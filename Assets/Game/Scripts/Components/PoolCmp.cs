using RangerV;

[System.Serializable]
public class PoolCmp : ComponentBase
{
    [Pool]
    public int start_pool_size;
    [Pool]
    public int id = 0;
}

