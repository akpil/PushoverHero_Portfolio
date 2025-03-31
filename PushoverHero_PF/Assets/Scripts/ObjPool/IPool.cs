namespace ObjPool
{
    public interface IPool<TID>
    {
        public void Initialize(params IPoolElement<TID>[] elem);
        public IPoolElement<TID> Get(TID id);
        public void Return(IPoolElement<TID> contents);
        public void Clear();
    }
}
