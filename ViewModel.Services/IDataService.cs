using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;

namespace CoffeeClientPrototype.ViewModel.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Cafe>> GetAllCafes();

        Task<IEnumerable<Comment>> GetCafeComments(int cafeId);

        Task SubmitComment(int cafeId, Comment comment);
    }
}
