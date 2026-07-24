using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
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
        [HttpGet]
        public async Task<IActionResult> GetItems(Guid id)
        {
            if (_DbContext.Groups == null)
                return NotFound();
                var item = await _DbContext.Groups.FindAsync(id);
                if (item == null)
                    return NotFound();
                return Ok(item);
            }
        [HttpPost]
        public async Task<IActionResult> Save(GroupViewModel model)
        {
            Core.Database.Models.Group item;
            if (model.Id == null)
            {
                item = new Core.Database.Models.Group();
                item.Id = Guid.NewGuid();
                await _DbContext.Groups.AddAsync(item);
            }
            else 
            {
                item = await _DbContext.Groups.FindAsync(model.Id);
            }
            item.Name = model.Name;
            await _DbContext.SaveChangesAsync();
            return Ok(item);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var memberInGroup = await _DbContext.Members.Where(m => m.GroupId == id).FirstOrDefaultAsync();
            if (memberInGroup == null)
            {
                var item = await _DbContext.Groups.FindAsync(id);
                _DbContext.Entry(item).State = EntityState.Deleted;
                await _DbContext.SaveChangesAsync();
                return Ok(true);
            }
            return Ok(false);
        }
    }
    }
