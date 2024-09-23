using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productServices;

        public ProductsController(IProductService productServices)
        {
            _productServices = productServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get()
        {
            await _productServices.GetAllAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            await _productServices.GetByIdAsync(id);
            return Ok();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetByFilter([FromBody] FilterSearchModel filterSearch)
        {
            await _productServices.GetByFilterAsync(filterSearch);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> Post([FromBody] ProductModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _productServices.AddAsync(value);
                if (value == null)
                {
                    return BadRequest();
                }
                return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int Id, [FromBody] ProductModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = await _productServices.GetByIdAsync(Id);
                
                if (product == null)
                {
                    return NotFound();
                }

                product = value;

                if (value == null)
                {
                    return BadRequest();
                }

                await _productServices.UpdateAsync(product);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _productServices.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("category")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetCategories()
        {
            await _productServices.GetAllProductCategoriesAsync();
            return Ok();
        }

        [HttpPost("category")]
        public async Task<ActionResult> AddCategory([FromBody] ProductCategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _productServices.AddCategoryAsync(categoryModel);
                if (categoryModel == null)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("category/{id}")]
        public async Task<ActionResult> UpdateCategory(int Id, [FromBody] ProductCategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _productServices.UpdateCategoryAsync(categoryModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("category/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _productServices.RemoveCategoryAsync(id);
            return Ok();
        }
    }
}
