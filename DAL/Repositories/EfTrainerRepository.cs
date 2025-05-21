using FitnessHere.DAL.EF;
using FitnessHere.DAL.Entities;

namespace FitnessHere.DAL.Repositories
{
    public class EfTrainerRepository : IRepository<Trainer>
    {
        public readonly FitnessDatabaseContext _dbContext;

        public EfTrainerRepository(FitnessDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Trainer trainer)
        {
            _dbContext.Add(trainer);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var trainerToRemove = GetById(id);
            if (trainerToRemove != null)
            {
                _dbContext.Trainers.Remove(trainerToRemove);
                _dbContext.SaveChanges();
            }
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public IList<Trainer> GetAll()
        {
            return _dbContext.Trainers.ToList();
        }

        public Trainer? GetById(int id)
        {
            return _dbContext.Trainers.Find(id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public void Update(Trainer member)
        {
            _dbContext.Update(member);
            _dbContext.SaveChanges();
        }


    }
}
