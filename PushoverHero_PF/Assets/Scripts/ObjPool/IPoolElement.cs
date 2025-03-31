namespace ObjPool
{
    public interface IPoolElement<TID>
    {
        public TID PoolElemID { get; }
        public string DebuggingName { get; }

        public void SetPool(IPool<TID> pool);
        
        public void Return();
    }
}
