using System;
using System.Collections.Generic;

public interface IManager<T>
{
    void Create(T item);
    T Read(int id);
    void Update(int id, T updatedItem);
    void Delete(int id);
}
