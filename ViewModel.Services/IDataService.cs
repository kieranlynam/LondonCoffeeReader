using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeClientPrototype.Model;

namespace CoffeeClientPrototype.ViewModel.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Cafe>> GetAllCafes();

        Task<IEnumerable<Feedback>> GetAllCafeFeedback(int cafeId);

        Task SubmitFeedback(int cafeId, Feedback feedback);
    }
}
