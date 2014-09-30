using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.Models;

namespace PizzaTracker.Test.Repo
{
    [TestClass]
    public class PizzaRepoTests
    {
        private PizzaTrackerRepo _repo;

        [TestInitialize()]
        public void Initialize()
        {
            _repo = new PizzaTrackerRepo(new PizzaContext());
        }

        [TestMethod]
        public void AddAndReadNewOrder()
        {
            var userId = 1;

            var pizza = new Pizza
            {
                CrustId = 1,
                SauceId = 1,
                SizeId = 1,
                Name = "Test Pizza",
                Toppings = new List<ToppingOption>
                {
                   new ToppingOption{ ToppingId = 1,Side = PizzaSide.Full},
                   new ToppingOption{ ToppingId = 2,Side = PizzaSide.Left}
                }
            };

            var newOrder = _repo.PlaceOrderForUser(userId, new List<Pizza> { pizza });

            Assert.IsNotNull(newOrder);

            var userOrders = _repo.GetOrdersForUser(userId);
            Assert.IsTrue(userOrders.Any(x => x.Id == newOrder.Id));

            var orderFromDb = _repo.GetOrderById(newOrder.Id);
            Assert.IsTrue(orderFromDb.OrderEvents.Count == 1);
            Assert.IsTrue(orderFromDb.OrderEvents.Any(x => x.Description.Contains("Order was placed")));
        }

        [TestMethod]
        public void SetShowOnOrder()
        {
            var userId = 1;

            var userOrders = _repo.GetOrdersForUser(userId).ToList();
            Assert.IsTrue(userOrders.Any());
            var orderId = userOrders.First().Id;
            _repo.SetOrderShow(orderId, false);
            var readOrder = _repo.GetOrderById(orderId);
            Assert.IsFalse(readOrder.Show);

            _repo.SetOrderShow(orderId, true);
            Assert.IsTrue(readOrder.Show);
        }

        [TestMethod]
        public void GetNextMessage()
        {
            var userId = 1;

            //clear out messages
            while (_repo.GetNextMessage(userId) != null)
            {
            }

            var next = _repo.GetNextMessage(userId);
            Assert.IsNull(next);

            //add pizza
            var pizza = new Pizza
            {
                CrustId = 1,
                SauceId = 1,
                SizeId = 1,
                Name = "Test Pizza",
                Toppings = new List<ToppingOption>
                {
                   new ToppingOption{ ToppingId = 1,Side = PizzaSide.Full},
                   new ToppingOption{ ToppingId = 2,Side = PizzaSide.Left}
                }
            };
            var newOrder = _repo.PlaceOrderForUser(userId, new List<Pizza> { pizza });

            //make sure message got queued
            next = _repo.GetNextMessage(userId);
            Assert.IsNotNull(next);
            //make sure message got dequeued
            next = _repo.GetNextMessage(userId);
            Assert.IsNull(next);
        }

        [TestMethod]
        public void UpdatePizzaOrder()
        {
            var userId = 1;

            //clear out messages
            while (_repo.GetNextMessage(userId) != null)
            { }
            var next = _repo.GetNextMessage(userId);
            Assert.IsNull(next);

            const string messageTitleSet = "Pizza in the Oven!";
            var messagBodySet = "Your pizza was placed into the oven at " + PizzaTime.Now.ToShortTimeString();
            var pizzaQ = _repo.GetpPizzaQueues().First();
            _repo.UpdatePizzaQueue(pizzaQ.Id, PizzaTrackerRepo.PizzaStatus.IntheOven, messageTitleSet, messagBodySet);

            //make sure message got queued
            next = _repo.GetNextMessage(userId);
            Assert.IsNotNull(next);
            Assert.AreEqual(next.MessageTitle, messageTitleSet);
            Assert.AreEqual(next.MessageBody, messagBodySet);
            //make sure message got dequeued
            next = _repo.GetNextMessage(userId);
            Assert.IsNull(next);
        }

        [TestMethod]
        public void SetActiveOnPizzaQueue()
        {
            var userId = 1;

            var pizzaQs = _repo.GetpPizzaQueues().ToList();
            Assert.IsTrue(pizzaQs.Any());

            var pizzaQId = pizzaQs.First().Id;;

            _repo.SetPizzaQueueActive(pizzaQId, true);
            var readQueue = _repo.GetPizzaQueueById(pizzaQId);
            Assert.IsTrue(readQueue.Active);
            
            _repo.SetPizzaQueueActive(pizzaQId, false);
            readQueue = _repo.GetPizzaQueueById(pizzaQId);
            Assert.IsFalse(readQueue.Active);
        }
    }
}
