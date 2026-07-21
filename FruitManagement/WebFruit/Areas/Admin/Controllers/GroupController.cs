using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using WebFruit.Areas.Admin.Models;
using WebFruit.Models.EF;

namespace WebFruit.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GroupController : Controller
    {
        private readonly Fruit _DbContext;
        public GroupController(Fruit DbContext)
        {
            _DbContext = DbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> getList(jDataTable model)
        {
            var items = (from i in _DbContext.Groups select i);
            int recordsTotal = 0;
            if (!string.IsNullOrEmpty(model.columns[model.order[0].column].name) && !string.IsNullOrEmpty(model.order[0].dir))
            {
                items = items.OrderBy(model.columns[model.order[0].column].name + " " + model.order[0].dir);
            }
            if(!string.IsNullOrEmpty(model.search.value))
            {
                items = items.Where(i => i.Name.Contains(model.search.value));
            }
            recordsTotal= items.Count();
            var data = await items.Select(i => new
            { i.Id,
              i.Name
            }).Skip(model.start).Take(model.length).ToListAsync();
            var jsonData = new
            {
                draw = model.draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = data
            };
            return Ok(jsonData);
        }
    }
}