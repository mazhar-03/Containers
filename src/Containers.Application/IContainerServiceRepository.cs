using Containers.Models;

namespace Containers.Application;

public interface IContainerServiceRepository
{
    IEnumerable<Container> GetAllContainers();
    bool Create(Container container);
    bool Delete(int id);
}