using Sirma_Solutions_Assigment.DTO.DTO;

namespace Sirma_Solutions_Assigment.BussinessLogic.Contracts
{
    public interface ICSVService
    {
        Task<SharedWork> GetHardestWorkingPair(Stream stream);
    }
}
