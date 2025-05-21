

namespace FitnessHere.DAL.Repositories
{
    public interface IRepository<T> : IDisposable
    {
        IList<T> GetAll();

        T? GetById(int id);

        void Update(T member);

        void Create(T member);

        void Delete(int id);


    }
}
