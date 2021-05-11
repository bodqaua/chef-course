
namespace Chef.Models.Entities
{
    public class DbImage
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
    }
}
