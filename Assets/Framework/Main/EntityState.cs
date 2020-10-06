
namespace RangerV
{
    [System.Serializable]
    public struct EntityState
    {
        public bool enabled;
        public bool requireStarter;
        public bool initialized;
        public bool runtime;

        public void AllFalse()
        {
            enabled = false;
            requireStarter = false;
            initialized = false;
            runtime = false;
        }
    }
}
