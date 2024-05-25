using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApiRest.Data;
using WebApiRest.Interface;

namespace WebApiRest.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound(id);
            }

            return Ok(data);

        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var addedproduct=await _repository.CreateAsync(product);
            return Created(string.Empty, addedproduct);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            var checkproduct = await _repository.GetByIdAsync(product.Id);
            if (checkproduct==null)
            {
                return NotFound(product.Id);
            }
            await _repository.UpdateAsync(product);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var checkproduct = await _repository.GetByIdAsync(id);
            if (checkproduct == null)
            {
                return NotFound(id);
            }
            await _repository.RemoveAsync(id);
            return NoContent();
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> Upload([FromForm]IFormFile formfile)
        {
            var newname =Guid.NewGuid()+"-"+Path.GetExtension(formfile.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newname);
            var stream = new FileStream(path, FileMode.Create);
            await formfile.CopyToAsync(stream);
            return Created(string.Empty, formfile);

        }
        [HttpGet("[Action]")]
        public IActionResult Test([FromForm] string name,[FromHeader] string auth)
        {
            var name2 = HttpContext.Request.Form["Name"];
            var authh = HttpContext.Request.Headers["auth"];
            return Ok();
        }
       
    }
}
