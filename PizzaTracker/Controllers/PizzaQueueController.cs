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

        /// <summary>
        /// Get the current Pizza queue
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PizzaQueue> GetPizzaQueue(string token)
        {
            var user = _repo.GetUserByEncrypted(token, PizzaTrackerRepo.PizzaRole.Employee);
            return _repo.GetpPizzaQueues().OrderByDescending(x=>x.Order.Date).ToList();
        }

        /// <summary>
        /// Update the status of a Pizza queue item
        /// </summary>
        /// <param name="token"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public PizzaQueue PostPizzaQueue(string token, [FromBody]PizzaQueueVm vm)
        {
            var newStatus = (PizzaTrackerRepo.PizzaStatus)vm.StatusId;
            return _repo.UpdatePizzaQueue(vm.Id, newStatus, "Pizza Update", "Your pizza status was updated to: " + newStatus);
        }
        
        /// <summary>
        /// Set a Pizza queue item to inactive
        /// </summary>
        /// <param name="token"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public IHttpActionResult DeletePizzaQueue(string token, int orderid)
        {
            var user = _repo.GetUserByEncrypted(token, PizzaTrackerRepo.PizzaRole.Employee);
            _repo.SetPizzaQueueActive(orderid, false);
            return Ok();
        }
    }
}
