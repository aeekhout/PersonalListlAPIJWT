using PersonalListlAPIJWT.Models;

namespace PersonalListlAPIJWT.Services
{
    public interface IComputerService
    {
        public Computer CreateComputer(Computer computer);

        public Computer UpdateComputer(Computer computer);

        public bool DeleteComputer(int id);

        public List<Computer> GetComputers();

        public Computer GetComputer(int id);


     
    }
}
