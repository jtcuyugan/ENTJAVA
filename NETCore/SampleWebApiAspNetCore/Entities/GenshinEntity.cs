namespace SampleWebApiAspNetCore.Entities
{
    public class GenshinEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Vision { get; set; }
        public int Rarity { get; set; }
        public DateTime Created { get; set; }
    }
}
