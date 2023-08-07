namespace GameCatalogue.Services.Data
{
    using GameCatalogue.Data.Models;
    using GameCatalogue.Data;
    using GameCatalogue.Services.Data.Interfaces;
    using GameCatalogue.Services.Data.Models.Guide;
    using GameCatalouge.Web.ViewModels.Guide;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using GameCatalouge.Web.ViewModels.Guide.Enums;
    using System.Security.Principal;

    public class GuideService : IGuideService
    {
        private readonly GameCatalogueDbContext dbContext;

        public GuideService(GameCatalogueDbContext dbContext, UserManager<ModdedUser> userManager)
        {
            this.dbContext = dbContext;
        }
        public async Task<AllGuidesFilterServiceModel> AllAsync(GuideAllQueryModel queryModel)
        {
            IQueryable<Guide> guidesQuery = this.dbContext
                .Guides
                .Where(gu => gu.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";
                guidesQuery = guidesQuery
                    .Where(gu => EF.Functions.Like(gu.Title, wildCard) || EF.Functions.Like(gu.Author.UserName, wildCard));
            }

            guidesQuery = queryModel.GuideSorting switch
            {
                GuideSorting.Newest => guidesQuery
                .OrderByDescending(gu => gu.CreatedOn),
                GuideSorting.Oldest => guidesQuery
                .OrderBy(gu => gu.CreatedOn),
                _ => guidesQuery
                    .OrderByDescending(gu => gu.CreatedOn)
            };

            IEnumerable<GuideAllViewModel> pagedGuides = await guidesQuery
                .Skip((queryModel.CurrentPage - 1) * queryModel.GuidesPerPage)
                .Take(queryModel.GuidesPerPage)
                .Select(gu => new GuideAllViewModel
                {
                    Id = gu.Id,
                    Title = gu.Title,
                    AuthorName = gu.Author.UserName
                })
                .ToArrayAsync();

            int totalGuides = guidesQuery.Count();

            return new AllGuidesFilterServiceModel()
            {
                TotalGuidesCount = totalGuides,
                Guides = pagedGuides
            };
        }

        public async Task<string> Create(GuideFormModel formModel, string userId)
        {
            Guide guide = new Guide()
            {
                Title = formModel.Title,
                Content = formModel.Content,
                AuthorId = Guid.Parse(userId)
            };

            dbContext.Guides.Add(guide);
            await dbContext.SaveChangesAsync();

            return guide.Id.ToString();
        }
    }
}
