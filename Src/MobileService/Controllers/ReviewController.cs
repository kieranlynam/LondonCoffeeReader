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

        public Task<ReviewData> PatchReviewData(string id, Delta<ReviewData> patch)
        {
            return this.UpdateAsync(id, patch);
        }

        public async Task<IHttpActionResult> PostReviewData(ReviewData item)
        {
            ReviewData current = await InsertAsync(item);
            return this.CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        public Task DeleteReviewData(string id)
        {
             return this.DeleteAsync(id);
        }
    }
}