using ApiGameServer.Models.DTO;

namespace ApiGameServer.Service.Interfaces;

public interface IMatchingService
{
    public Task<MatchingResponse> RequestMatchingAsync(MatchingRequest request);
    public Task<CheckMatchingResponse> CheckMatchingAsync(CheckMatchingRequest request);
    public Task<CancelMatchingResponse> CancelMatchingAsync(CancelMatchingRequest request);
}
