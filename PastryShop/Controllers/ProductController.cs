using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PastryShop.Helper;
using PastryShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PastryShop.Controllers
{
    public class ProductController : Controller
    {
        PSAPI psAPI = new PSAPI();
        public async Task<IActionResult> Index()
        {
            List<Product> products = new List<Product>();
            HttpClient client = psAPI.Initial();

            //Ottenere la lista dei prodotti disponibili
            HttpResponseMessage responseMessage =
                await client.GetAsync("api/product");
            if (responseMessage.IsSuccessStatusCode)
            {
                var results = 
                    responseMessage.Content.ReadAsStringAsync().Result;
                products = 
                    JsonConvert.DeserializeObject<List<Product>>(results);
            }

            //Calcolare eventuali sconti
            Discount discount = new Discount();
            decimal promotion = 0;
            List<Product> DiscountedProducts = new List<Product>(); 
            foreach (Product prod in products)
            {
                promotion = discount.GetDiscount(prod.Price, prod.SellingDate);
                prod.Price = prod.Price - promotion;
                if (true || prod.Price > 0 && prod.Quantity > 0)//si può cambiare la condizione se si vuole trattare gli articoli in omaggio
                         DiscountedProducts.Add(prod);  
            }
            return View(DiscountedProducts);
        }

        public async Task<IActionResult> Edit(int id)
        {
            //https://localhost:44315/api/Product?id=5
            HttpClient client = psAPI.Initial();
            Product product = new Product();
            HttpResponseMessage responseMessage =
                await client.GetAsync($"api/product/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var results =
                    responseMessage.Content.ReadAsStringAsync().Result;
                product =
                    JsonConvert.DeserializeObject<Product>(results);
            }
            return View(product);
        }

        [HttpPost]
        public  async Task<IActionResult> Edit(int id, Product product)
        {
            //https://localhost:44315/api/Product?id=5
            HttpClient client = psAPI.Initial();
            string jsonString = JsonConvert.SerializeObject(product);
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            HttpResponseMessage editProduct =
                await client.PutAsync($"api/Product?id={id}",content);
            //var result = editProduct.Result;
            if (editProduct.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View(product);
        }

        public IActionResult Create() { return View(); }

        [HttpPost]
        public IActionResult Create( Product product)
        {
            //ok
            HttpClient client = psAPI.Initial();
            string jsonString = JsonConvert.SerializeObject(product);
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var addProduct = 
                client.PostAsync("api/product", content);
            var result = addProduct.Result;
            if (result.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = psAPI.Initial();
            Product product = new Product();
            HttpResponseMessage responseMessage =
                await client.GetAsync($"api/product/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var results =
                    responseMessage.Content.ReadAsStringAsync().Result;
                product =
                    JsonConvert.DeserializeObject<Product>(results);
            }
            return View(product);
        }
        public IActionResult DeleteProduct(int Id)
        {
            HttpClient client = psAPI.Initial();
            
            var deleteProduct =
                 client.DeleteAsync($"api/product/{Id}");
            var result = deleteProduct.Result;
            if (result.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View("");
        }
        public async Task<IActionResult> Details(int id)
        {
            Product product = new Product();
            HttpClient client = psAPI.Initial();
            HttpResponseMessage responseMessage =
                await client.GetAsync($"api/product/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var results =
                    responseMessage.Content.ReadAsStringAsync().Result;
                product =
                    JsonConvert.DeserializeObject<Product>(results);
            }

            return View(product);
        }
    }
}
