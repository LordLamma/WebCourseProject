﻿namespace GameCatalogue.Services.Data.Interfaces
{
	using GameCatalogue.Services.Data.Models.Game;
	using GameCatalouge.Web.ViewModels.Game;
	using GameCatalouge.Web.ViewModels.Home;

    public interface IGameService
    {
        Task<IEnumerable<IndexViewModel>> LastThreeGames();
        Task<IEnumerable<GameAllViewModel>> ThreeRandomGames(string userId);
        Task<string> Create(GameFormModel formModel, string developerId);
        Task<AllGamesFilterServiceModel> AllAsync(AllGamesQueryModel queryModel);
        Task<IEnumerable<GameAllViewModel>> AllByDeveloperIdAsync(string developerId);
        Task<GameDetailsViewModel?> GetDetailsByIdAsync(string gameId);
        Task<bool> ExistsByIdAsync(string gameId);
        Task<GameFormModel> GetGameForEditByIdAsync(string gameId);
        Task<bool> IsDeveloperByIdProducerOfGameByIdAsync(string gameId, string developerId);
        Task EditGameByIdAndFormModel(string gameId, GameFormModel formModel);
        Task<GamePreDeleteDetailsViewModel> GetGameDetailsForDeleteByIdAsync(string gameId);
        Task DeleteGameByIdAsync(string gameId);
    }
}
