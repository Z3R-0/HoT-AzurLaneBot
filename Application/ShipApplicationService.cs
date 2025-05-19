using Application.DTO;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.ShipAggregate;

namespace Application;

public class ShipApplicationService(IShipRepository shipRepository, IUnitOfWork unitOfWork) : IShipApplicationService {
    private readonly IShipRepository _shipRepository = shipRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Ship?> GetRandomShipAsync(bool allowSkins) {
        IEnumerable<Ship> possibleShips = [];
        if (allowSkins) {
            possibleShips = from ship in await _shipRepository.GetAllAsync()
                            select ship;
        }

        if (!possibleShips.Any())
            return null;

        var index = Random.Shared.Next(possibleShips.Count());
        return possibleShips.ElementAt(index);
    }

    public async Task<(bool IsSuccess, string? Errors)> DeleteShipAsync(string shipName) {
        var ship = await _shipRepository.GetByNameAsync(shipName);
        if (ship == null) {
            return (false, $"[Input Error]: No ship found with name '{shipName}'.");
        }

        await _shipRepository.DeleteAsync(ship.Id);
        await _unitOfWork.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool IsSuccess, string? Errors)> RegisterShipAsync(RegisterShip dto) {
        var existingShip = await _shipRepository.GetByNameAsync(dto.ShipName);
        if (existingShip != null) {
            return (false, $"[Input Error]: A ship with the name '{dto.ShipName}' already exists.");
        }

        var newShip = new Ship {
            Name = dto.ShipName,
            Rarity = dto.Rarity
        };

        await _shipRepository.AddAsync(newShip);
        await _unitOfWork.SaveChangesAsync();

        return (true, null);
    }

    public async Task<Ship?> GetByNameAsync(string shipName) {
        return await _shipRepository.GetByNameAsync(shipName);
    }

    public async Task<Ship?> GetByIdAsync(Guid shipId) {
        return await _shipRepository.GetByIdAsync(shipId);
    }
}
