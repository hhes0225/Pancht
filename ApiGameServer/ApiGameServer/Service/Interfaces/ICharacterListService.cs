using ApiGameServer.Models.DAO;
using ApiGameServer.Models.DTO;

namespace ApiGameServer.Service.Interfaces;

public interface ICharacterListService
{
    public Task<CharacterListResponse> CharacterListAsync(CharacterListRequest request);
}
