using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IGenshinRepository
    {
        GenshinEntity GetSingle(int id);
        void Add(GenshinEntity item);
        void Delete(int id);
        GenshinEntity Update(int id, GenshinEntity item);
        IQueryable<GenshinEntity> GetAll(QueryParameters queryParameters);
        ICollection<GenshinEntity> GetRandomGenChar();
        int Count();
        bool Save();
    }
}
