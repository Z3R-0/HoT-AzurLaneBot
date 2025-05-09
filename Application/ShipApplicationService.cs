using Domain.Interfaces;
using Domain.ShipAggregate;

namespace Application;

public class ShipApplicationService(IShipRepository shipRepository, IUnitOfWork unitOfWork) {
    private readonly IShipRepository _shipRepository = shipRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task RegisterShipAsync(Ship ship) {
        await _shipRepository.AddAsync(ship);
        await _unitOfWork.SaveChangesAsync();
    }
}
