﻿using Application.DTO;

namespace Application.Interfaces;
public interface IGameApplicationService {
    public Task<GuessShipGameResult?> StartGuessShipGameAsync(bool allowSkins);
}
