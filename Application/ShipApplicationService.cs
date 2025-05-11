using Application.Interfaces;
using Domain.Interfaces;
using Domain.ShipAggregate;

namespace Application;

public class ShipApplicationService(IShipRepository shipRepository, IUnitOfWork unitOfWork) : IShipApplicationService {
    private readonly IShipRepository _shipRepository = shipRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Ship?> GetRandomShip(bool allowSkins) {
        IEnumerable<Ship> possibleShips = [];
        if (allowSkins) {
            possibleShips = from ship in await _shipRepository.GetAllAsync()
                            select ship;
        }

        if (!possibleShips.Any())
            throw new InvalidOperationException("No ships available to select from.");

        var index = Random.Shared.Next(possibleShips.Count());
        return possibleShips.ElementAt(index);
    }

    public async Task RegisterShipAsync(Ship ship) {
        await _shipRepository.AddAsync(ship);
        await _unitOfWork.SaveChangesAsync();
    }
}
