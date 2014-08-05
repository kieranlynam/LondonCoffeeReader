using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using londoncoffeeService.DataObjects;
using londoncoffeeService.Models;

namespace londoncoffeeService.Controllers
{
    public class ReviewController : TableController<ReviewData>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var context = new DataContext();
            this.DomainManager = new EntityDomainManager<ReviewData>(context, this.Request, this.Services);
        }

        public IQueryable<ReviewData> GetAllReviewData()
        {
            return this.Query(); 
        }

        public SingleResult<ReviewData> GetReviewData(string id)
        {
            return this.Lookup(id);
        }

        public async Task<ReviewData> PatchReviewData(string id, Delta<ReviewData> patch)
        {
            using (var context = new DataContext())
            {
                var review = context.Reviews.First(r => r.Id == id);
                bool hadVoteBefore = review.CoffeeRating.HasValue || review.AtmosphereRating.HasValue;

                var patchedReview = await this.UpdateAsync(id, patch);

                var hasVoteAfter = patchedReview.CoffeeRating.HasValue || patchedReview.AtmosphereRating.HasValue;
                if (hadVoteBefore != hasVoteAfter)
                {
                    var cafe = context.Cafes.First(c => c.Id == patchedReview.CafeId);
                    
                    if (hasVoteAfter)
                    {
                        cafe.NumberOfVotes++;
                    }
                    else
                    {
                        cafe.NumberOfVotes--;
                    }

                    context.SaveChanges();
                }

                return patchedReview;
            }
        }

        public async Task<IHttpActionResult> PostReviewData(ReviewData item)
        {
            var current = await InsertAsync(item);
            var result = this.CreatedAtRoute("Tables", new { id = current.Id }, current);

            if (item.CoffeeRating.HasValue || item.AtmosphereRating.HasValue)
            {
                using (var context = new DataContext())
                {
                    var cafe = context.Cafes.First(c => c.Id == item.CafeId);
                    cafe.NumberOfVotes++;
                    context.SaveChanges();
                }    
            }

            return result;
        }

        public Task DeleteReviewData(string id)
        {
            using (var context = new DataContext())
            {
                var review = context.Reviews.First(r => r.Id == id);

                if (review.CoffeeRating.HasValue || review.AtmosphereRating.HasValue)
                {
                    var cafe = context.Cafes.First(c => c.Id == review.CafeId);
                    cafe.NumberOfVotes--;
                    context.SaveChanges();
                }
            }

            return this.DeleteAsync(id);
        }
    }
}