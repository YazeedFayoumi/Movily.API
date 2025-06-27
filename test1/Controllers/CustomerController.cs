using Microsoft.AspNetCore.Mvc;
using test1.Models;

namespace test1.Controllers
{
    public class CustomerController : Controller
    {
        //List of Customers 
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        // render view
        //somethn
        [HttpGet]
        public ViewResult Create()
        {
            return View(); 
        }
        [HttpPost]
        public ActionResult CreateCustomer(Customer customer)
        {
            return View();
        }
        public ActionResult RemoveCustomer() 
        {

            return View();
        }
    }
}
