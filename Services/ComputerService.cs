using PersonalListlAPIJWT.Models;
using PersonalListlAPIJWT.Repositories;

namespace PersonalListlAPIJWT.Services
{
    public class ComputerService : IComputerService
    {
        public Computer CreateComputer(Computer computer)
        {
            computer.Id = ComputerRepository.Computers.Count + 1;
            ComputerRepository.Computers.Add(computer);

            return computer;
        }

        public bool DeleteComputer(int id)
        {
            var computer = ComputerRepository.Computers.FirstOrDefault(o => o.Id == id);

            if (computer is null) 
                return false;

            ComputerRepository.Computers.Remove(computer);

            return true;
        }

        public Computer GetComputer(int id)
        {
            var computer = ComputerRepository.Computers.FirstOrDefault(o => o.Id == id);

            if (computer is null) 
                return null;

            return computer;
        }

        public List<Computer> GetComputers()
        {
            var computers = ComputerRepository.Computers;

            return computers;
        }

        public Computer UpdateComputer(Computer computer)
        {
            var computerUpdate = ComputerRepository.Computers.FirstOrDefault(o => o.Id == computer.Id);

            if (computerUpdate is null) 
                return null;

            computerUpdate.Name = computer.Name;
            computerUpdate.Description = computer.Description;

            return computerUpdate;
        }
    }
}
