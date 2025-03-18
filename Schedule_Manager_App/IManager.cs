interface IManager<T>
{
    void Create(T item);
    T Read(int id);
    void Update(T item);
    void Delete(int id);
}
