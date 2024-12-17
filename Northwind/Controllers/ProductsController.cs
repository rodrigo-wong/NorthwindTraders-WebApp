using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Northwind.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly string baseUrl;
        private readonly string appJson;

        public ProductsController(IConfiguration config)
        {
            baseUrl = config.GetValue<string>("BaseUrl");
            appJson = config.GetValue<string>("AppJson");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int categoryId = 1)
        {
            try
            {
                var categories = await GetCategoriesAsync();
                ViewBag.CategoryId = new SelectList(categories, "categoryId", "categoryName", categoryId);

                List<Product> products = new List<Product>();
                using (var client = new HttpClient())
                {
                    ConfigClient(client);
                    var response = await client.GetAsync($"products/bycategory/{categoryId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        products = JsonSerializer.Deserialize<List<Product>>(json);
                    }
                    else
                    {
                        throw new HttpRequestException($"Error fetching products: {response.StatusCode}");
                    }
                }
                return View(products);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error", new ErrorViewModel());
            }
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            List<Category> categories = new List<Category>();
            using (var client = new HttpClient())
            {
                ConfigClient(client);
                var response = await client.GetAsync("categories");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    categories = JsonSerializer.Deserialize<List<Category>>(json);
                }
                else
                {
                    throw new HttpRequestException($"Error fetching categories: {response.StatusCode}");
                }
            }
            return categories;
        }

        private void ConfigClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(appJson));
            client.BaseAddress = new Uri(baseUrl);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                Product product = null;
                using (var client = new HttpClient())
                {
                    ConfigClient(client);
                    var response = await client.GetAsync($"products/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        product = JsonSerializer.Deserialize<Product>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    else
                    {
                        throw new HttpRequestException($"Product with id {id} not found.");
                    }
                }

                if (product == null)
                {
                    return NotFound($"Product with id {id} not found.");
                }

                var categories = await GetCategoriesAsync();
                var category = categories.FirstOrDefault(c => c.categoryId == product.categoryId);
                ViewBag.CategoryName = category?.categoryName;

                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error", new ErrorViewModel());
            }
        }
    }
}
