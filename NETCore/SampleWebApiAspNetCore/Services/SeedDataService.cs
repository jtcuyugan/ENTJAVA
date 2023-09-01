using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(GenshinDbContext context)
        {
            context.GenshinCharacters.Add(new GenshinEntity() { Rarity = 5, Vision = "Cryo", Name = "Kamisato Ayaka", Created = DateTime.Now });
            context.GenshinCharacters.Add(new GenshinEntity() { Rarity = 4, Vision = "Anemo", Name = "Lynette", Created = DateTime.Now });
            context.GenshinCharacters.Add(new GenshinEntity() { Rarity = 5, Vision = "Electro", Name = "Yae Miko", Created = DateTime.Now });
            context.GenshinCharacters.Add(new GenshinEntity() { Rarity = 5, Vision = "Pyro", Name = "Diluc", Created = DateTime.Now });
            context.GenshinCharacters.Add(new GenshinEntity() { Rarity = 5, Vision = "Hydro", Name = "Tartaglia", Created = DateTime.Now });
            context.GenshinCharacters.Add(new GenshinEntity() { Rarity = 4, Vision = "Dendro", Name = "Collei", Created = DateTime.Now });
            context.GenshinCharacters.Add(new GenshinEntity() { Rarity = 5, Vision = "Geo", Name = "Navia", Created = DateTime.Now });

            context.SaveChanges();
        }
    }
}
