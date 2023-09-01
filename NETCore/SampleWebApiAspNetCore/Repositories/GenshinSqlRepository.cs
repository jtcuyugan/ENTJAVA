using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class GenshinSqlRepository : IGenshinRepository
    {
        private readonly GenshinDbContext _genshinDbContext;

        public GenshinSqlRepository(GenshinDbContext genshinDbContext)
        {
            _genshinDbContext = genshinDbContext;
        }

        public GenshinEntity GetSingle(int id)
        {
            return _genshinDbContext.GenshinCharacters.FirstOrDefault(x => x.Id == id);
        }

        public void Add(GenshinEntity item)
        {
            _genshinDbContext.GenshinCharacters.Add(item);
        }

        public void Delete(int id)
        {
            GenshinEntity genChar = GetSingle(id);
            _genshinDbContext.GenshinCharacters.Remove(genChar);
        }

        public GenshinEntity Update(int id, GenshinEntity item)
        {
            _genshinDbContext.GenshinCharacters.Update(item);
            return item;
        }

        public IQueryable<GenshinEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<GenshinEntity> _allItems = _genshinDbContext.GenshinCharacters.OrderBy(queryParameters.OrderBy,
            queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Rarity.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _genshinDbContext.GenshinCharacters.Count();
        }

        public bool Save()
        {
            return (_genshinDbContext.SaveChanges() >= 0);
        }

        public ICollection<GenshinEntity> GetRandomGenChar()
        {
            List<GenshinEntity> toReturn = new List<GenshinEntity>();

            toReturn.Add(GetRandomGenChar("DPS"));
            toReturn.Add(GetRandomGenChar("Sub-DPS"));
            toReturn.Add(GetRandomGenChar("Support"));

            return toReturn;
        }

        private GenshinEntity GetRandomGenChar(string type)
        {
            return _genshinDbContext.GenshinCharacters
                .Where(x => x.Vision == type)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}
