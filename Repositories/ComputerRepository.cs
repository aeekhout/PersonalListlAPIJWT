using PersonalListlAPIJWT.Models;

namespace PersonalListlAPIJWT.Repositories
{
    public class ComputerRepository
    {
        public static List<Computer> Computers = new()
        {
            new()
            {
                Id = 1,
                Name = "Desktop",
                Description = "CPU I5, 16 GB Ram, 500GB Hard Disk"
            },
            new()
            {
                Id = 2,
                Name = "Laptop",
                Description = "CPU I7, 32 GB Ram, 2TB Hard Disk"
            },
            new()
            {
                Id = 3,
                Name = "Tablet",
                Description = "CPU ARM, 8 GB Ram, 32GB Hard Disk"
            }
        };
    }
}
