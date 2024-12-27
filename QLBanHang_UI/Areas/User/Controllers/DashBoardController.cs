using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using QLBanHang_UI.Models;
using QLBanHang_UI.Models.Request;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace QLBanHang_UI.Areas.User.Controllers
{
    [Area("User")]
    public class DashBoardController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public DashBoardController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        // VIEW 
        // Khoi tao DashBoard
        public async Task<IActionResult> DashBoard()
        {
            var productsDto = await GetAllProduct();
            var categoriesDto = await GetAllCategory();
            var brandsDto = await GetAllBrand();
            
            var cart = new List<CartItemDto>();
            //Kiem tra neu Session cart chua ton tai
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (!SessionExtension.IsCartExist(HttpContext.Session))
                {
                    //var userId = Guid.Parse(Request.Cookies["UserId"]);
                    cart = await GetAllCartItem(userId);
                    HttpContext.Session.SetObjectAsJson("Cart", cart);
                }
                else
                {
                    cart = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
                }
                ViewData["Carts"] = cart;
            }
            var viewModel = new ViewModel()
            {
                Products = productsDto,
                Categorys = categoriesDto,
                Brands = brandsDto,
                Carts = cart
            };
            HttpContext.Session.SetObjectAsJson("ViewModel", viewModel);
            return View("~/Areas/User/Views/DashBoardUser.cshtml",viewModel);
        }

        //Action cho Search 
        [HttpGet] 
        public IActionResult SearchProduct(string search)
        {
            string encodeSearch = Uri.EscapeDataString(search);
            string targetUrl = $"/User/Dashboard/CategoryView?search={encodeSearch}&page=1";
            return Redirect(targetUrl);
        }

        //Action for brandName
        [HttpGet]
        public IActionResult BrandProduct(string brandName)
        {
            string encodeSearch = Uri.EscapeDataString(brandName);
            string targetUrl = $"/User/Dashboard/CategoryView?brandName={encodeSearch}&page=1";
            return Redirect(targetUrl);
        }

        //Action for Category
        [HttpGet]
        public IActionResult CategoryProduct(string category)
        {
            string encodeSearch = Uri.EscapeDataString(category);
            string targetUrl = $"/User/Dashboard/CategoryView?category={encodeSearch}&page=1";
            return Redirect(targetUrl);
        }


        //View Category
        public IActionResult CategoryView()
        {
            var viewModel = HttpContext.Session.GetObjectFromJson<ViewModel>("ViewModel");
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
            viewModel.Carts = cart;
            return View("~/Areas/User/Views/Category.cshtml", viewModel);
        }
        //Product Detail View
        [HttpGet]
        public async Task<IActionResult> ProductDetail(Guid id)
        {
            var product = await GetProductDetail(id);
            var productsDto = await GetAllProduct();
            productsDto = productsDto.Where(x => x.BrandName == product.BrandName).ToList(); // Search san pham lien quan toi nhan hang
            var categoriesDto = await GetAllCategory();
            var brandsDto = await GetAllBrand();
            var viewModel = new ViewModel()
            {
                Products = productsDto,
                Categorys = categoriesDto,
                Brands = brandsDto,
                Carts = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart")
            };
            ViewData["ProductDetail"] = product;
            
            return View("~/Areas/User/Views/ProductDetail.cshtml", viewModel);
        }

        //Remove Item PartialView
        [HttpGet]
        public IActionResult RemoveItem(Guid id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            cart.RemoveAll(x => x.CartItemId == id);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            // Trả về một PartialView cập nhật lại giỏ hàng sau khi xóa sản phẩm
            var View =  PartialView("~/Areas/User/Views/Shared/_CartPartial.cshtml", cart); // Đảm bảo bạn trả về PartialView với danh sách giỏ hàng mới
            return View;
        }


        //AddItemPartialView
        [HttpGet]
        public async Task<IActionResult> AddItem(Guid id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();

            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                var product = await GetProductDetail(id);
                cart.Add(new CartItemDto
                {
                    CartItemId = Guid.NewGuid(),
                    ProductId = id,
                    Product = product,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            var newCarts = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
            return PartialView("~/Areas/User/Views/Shared/_CartPartial.cshtml", newCarts);
        }


        //Add Product với Quantities
        [HttpPost]
        public async Task<IActionResult> AddItemProductDetail(Guid productId , int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItemDto>>("Cart");
            var cartId = cart.First().CartId;
            var cartItem = cart.FirstOrDefault(x => x.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity = cartItem.Quantity + quantity;
            }
            else
            {
                var product = await GetProductDetail(productId);

                var newCartItem = new CartItemDto
                {
                    CartId = cartId,
                    Quantity = quantity,
                    CartItemId = Guid.NewGuid(),
                    ProductId = productId,
                    Product = product
                };
                cart.Add(newCartItem);

            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return PartialView("~/Areas/User/Views/Shared/_CartPartial.cshtml", cart);
        }


        //Pagination Product
       /* public async Task<IActionResult> SearchProduct(string? searchQuery, int pageNumber)
        {
            var productList = await GetAllProduct();
            //Pagination
            var pageNumber1 =1;
            var pageSize = 8;
            var skipResult = (pageNumber -1)* pageSize;            
            productList.Skip(skipResult).Take(pageSize).ToList();

        }*/

        //LOGIC XU LY
        // Cac method call Product
        public async Task<List<ProductDto>> GetAllProduct()
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://localhost:7069/api/Product/GetAllProduct")
                };
           
                var httpResponse = await client.SendAsync(httpMessage);
                if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    return new List<ProductDto>();
                }

                var stringResponseBody = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                var productsDto = stringResponseBody?.ToList() ?? new List<ProductDto>();
                if (productsDto != null)
                {
                    Console.WriteLine("OK");
                }
                int count = 0;
                foreach (var product in productsDto)
                {
                    product.ImageUrl = product.ImageUrl;
                    count++;
                    if (count == 8)
                    {
                        break;
                    }
                }
                return productsDto;
            }
            catch (Exception ex)
            {
                return new List<ProductDto>();
            }
        }

        public async Task<List<CategoryDto>> GetAllCategory()
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://localhost:7069/api/Category/GetAllCategories")
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new List<CategoryDto>();
                }
                var response = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<CategoryDto>>();
                var categoriesDto = response?.ToList() ?? new List<CategoryDto>();
                return categoriesDto;
            }
            catch (Exception ex)
            {
                return new List<CategoryDto>();
            }
        }

        public async Task<List<BrandDto>> GetAllBrand()
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://localhost:7069/api/Brand/GetAllBrands")
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new List<BrandDto>();
                }
                var brandsDto = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<BrandDto>>();
                return brandsDto.ToList() ?? new List<BrandDto>();
            }
            catch(Exception ex)
            {
                return new List<BrandDto>();
            }
        }
        
        

        
        public async Task<ProductDto> GetProductDetail(Guid id)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7069/api/Product/GetProductById/{id}")
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                { 
                    return new ProductDto();
                }
                var product = await httpResponse?.Content.ReadFromJsonAsync<ProductDto>();
                product.ImageUrl = product.ImageUrl;
                if (product.ImageUrl != null) 
                {
                    for(int i = 0; i < product.ProductImages.Count; i++)
                    {
                        product.ProductImages[i] = product.ProductImages[i];
                    }
                    
                }
                return product;

            }
            catch (Exception ex)    
            {
                return new ProductDto();
            }
        }

        // Cac Method Xu ly Cart
        //Lấy tất cả CartItem lên
        public async Task<List<CartItemDto>> GetAllCartItem(Guid id)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7069/api/Cart/GetAllCartItem/{id}")
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new List<CartItemDto>();
                }
                var response = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<CartItemDto>>();
                var cartItemsDto = response?.ToList() ?? new List<CartItemDto>();
                return cartItemsDto;
            }
            catch(Exception ex)
            {
                return new List<CartItemDto>();
            }
        }

        //Add To cart
        public async Task<CartItemDto> AddToCart(CartItemRequest cartItemRequest)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7069/api/Cart/AddCartItem"),
                    Content = new StringContent(JsonSerializer.Serialize(cartItemRequest),Encoding.UTF8,"application/json")
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                var response = await httpResponse.Content.ReadFromJsonAsync<CartItemDto>();
                return response;
            }
            catch (Exception ex)
            {
                return new CartItemDto();
            }
        }

        //DeleteCart
        public async Task<List<CartItemDto>> DeleteCartItem(List<CartItemRequest> cartItemRequests)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7069/api/Cart/DeleteCartItem"),
                    Content = new StringContent(JsonSerializer.Serialize(cartItemRequests), Encoding.UTF8, "application/json")
                };
                var httpResponse = await client.SendAsync(httpMessage);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new List<CartItemDto>();
                }
                var response = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<CartItemDto>>();
                var cartItemsDto = response?.ToList() ?? new List<CartItemDto>();
                return cartItemsDto;
            }
            catch (Exception ex)
            {
                return new List<CartItemDto>();
            }
        }
    }

}
