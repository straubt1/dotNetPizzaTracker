using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.Models;
using PizzaTracker.ViewModels;

namespace PizzaTracker.Controllers
{
    public class PizzaQueueController : ApiController
    {
        private PizzaTrackerRepo _repo = new PizzaTrackerRepo(new PizzaContext());

        // GET: api/Order
        public IEnumerable<PizzaQueue> GetPizzaQueue()
        {
            return _repo.GetpPizzaQueues().ToList();
        }

        [HttpPost]
        public PizzaQueue PostPizzaQueue(PizzaQueueVm vm)
        {
            var newStatus = (PizzaTrackerRepo.PizzaStatus)vm.StatusId;
            if (vm.StatusId > 3)
            {
                return _repo.UpdatePizzaQueue(vm.Id, newStatus, "Pizza For James", "James sucks since he can go home and not work!!");
            }
            return _repo.UpdatePizzaQueue(vm.Id, newStatus, "Pizza Update", "Your pizza status was updated to: " + newStatus);
        }

        public IHttpActionResult DeletePizzaQueue(int id)
        {
            _repo.SetPizzaQueueActive(id, false);
            return Ok();
        }
    }
}
