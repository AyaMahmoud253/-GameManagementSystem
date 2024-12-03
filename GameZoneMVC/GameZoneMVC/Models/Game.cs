namespace GameZoneMVC.Models
{
    public class Game:Base
    {
        public string Description { get; set; } = string.Empty;
        public string Cover { get; set; }= string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        public ICollection<GameDevice> Devices { get; set; }=new List<GameDevice>();
    }
}
