using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QueuingTest.Models;
using QueuingTest.Queuing;

namespace QueuingTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQueue<string> _queue;

        public HomeController(IQueue<string> queue)
        {
            _queue = queue;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(TestMessage model)
        {
            _queue.Enqueue(model.Text);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
