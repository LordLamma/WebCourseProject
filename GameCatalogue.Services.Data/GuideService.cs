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

    public class GuideService : IGuideService
    {
        private readonly GameCatalogueDbContext dbContext;

        public GuideService(GameCatalogueDbContext dbContext)
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
                .OrderBy(gu => gu.CreatedOn),
                GuideSorting.Oldest => guidesQuery
                .OrderByDescending(gu => gu.CreatedOn),
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

        public async Task DeleteGuideByIdAsync(string guideId)
        {
            Guide guide = await this.dbContext
                .Guides
                .Where(gu => gu.IsDeleted == false)
                .FirstAsync(gu => gu.Id.ToString() == guideId);

            guide.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task EditGuideByIdAndFormModel(string guideId, GuideFormModel formModel)
        {
            Guide guide = await this.dbContext
                .Guides
                .Where(gu => gu.IsDeleted == false)
                .FirstAsync(gu => gu.Id.ToString() == guideId);

            guide.Title = formModel.Title;
            guide.Content = formModel.Content;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            bool result = await this.dbContext
                .Guides
                .AnyAsync(gu => gu.Id.ToString() == id);

            return result;
        }

        public async Task<GuideDetailsViewModel?> GetDetailsByIdAsync(string id)
        {
            Guide guide = await this.dbContext
                .Guides
                .Include(gu => gu.Author)
                .Where(gu => gu.IsDeleted == false)
                .FirstAsync(gu => gu.Id.ToString() == id);

            return new GuideDetailsViewModel
            {
                Id = guide.Id,
                Title = guide.Title,
                Content = guide.Content,
                AuthorName = guide.Author.UserName
            };
        }

        public async Task<GuidePreDeleteViewModel> GetGuideDetailsForDeleteByIdAsync(string guideId)
        {
            Guide guide = await this.dbContext
                .Guides
                .Where(gu => gu.IsDeleted == false)
                .FirstAsync(gu => gu.Id.ToString() == guideId);

            return new GuidePreDeleteViewModel
            {
                Title = guide.Title,
                Content = guide.Content
            };
            throw new NotImplementedException();
        }

        public async Task<GuideFormModel> GetGuideForEditByIdAsync(string guideId)
        {
            Guide guide = await this.dbContext
                .Guides
                .Where(gu => gu.IsDeleted == false)
                .FirstAsync(gu => gu.Id.ToString() == guideId);

            return new GuideFormModel
            {
                Title = guide.Title,
                Content = guide.Content,
            };
        }

        public async Task<bool> IsUserByIdWriterOfGuideById(string guideId, string userId)
        {
            Guide guide = await this.dbContext
                .Guides
                .Where(gu => gu.IsDeleted == false)
                .FirstAsync(gu => gu.Id.ToString() == guideId);

            return guide.AuthorId.ToString() == userId;
        }
    }
}
