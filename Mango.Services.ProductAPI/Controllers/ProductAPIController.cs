using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : Controller
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Product> objList = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDTO>>(objList);
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:long}")]
        public ResponseDTO Get(int id)
        {
            try
            {
                Product obj = _db.Products.First(u => u.Id == id);
                _response.Result = _mapper.Map<ProductDTO>(obj);
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        public ResponseDTO GetByName(string name)
        {
            try
            {
                Product obj = _db.Products.First(u => u.Name.ToLower() == name.ToLower());
                _response.Result = _mapper.Map<ProductDTO>(obj);
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpGet]
        [Route("GetByCategory/{categoryName}")]
        public ResponseDTO GetByCategory(string categoryName)
        {
            try
            {
                IEnumerable<Product> objList = _db.Products.Where(u => u.Category.ToLower() == categoryName.ToLower());
                _response.Result = _mapper.Map<IEnumerable<ProductDTO>>(objList);
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Post([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
                _db.Products.Add(product);
                _db.SaveChanges();

                if (productDTO.Image != null)
                {
                    string fileName = product.Id + Path.GetExtension(productDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDTO.Image.CopyTo(fileStream);
                    }

                    var baseUrl =
                        $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + filePath;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }

                _db.Products.Update(product);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Put([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product obj = _mapper.Map<Product>(productDTO);
                _db.Products.Update(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDTO>(obj);
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id:long}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Delete(long id)
        {
            try
            {
                Product obj = _db.Products.First(u => u.Id == id);
                _db.Products.Remove(obj);
                _db.SaveChanges();

                _response.Message = "Product has been deleted!";
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
