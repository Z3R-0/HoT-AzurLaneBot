using Application.DTO;
using Domain.ShipAggregate;

namespace Application.Interfaces;
public interface IShipApplicationService {
    public Task<(bool IsSuccess, string? Errors)> RegisterShipAsync(RegisterShip dto);
    public Task<(bool IsSuccess, string? Errors)> DeleteShipAsync(string shipName);
    public Task<Ship?> GetByNameAsync(string skinName);
    public Task<Ship?> GetRandomShipAsync(bool allowSkins);
    public Task<Ship?> GetByIdAsync(Guid shipId);
}
