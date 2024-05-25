using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcConsumeApi.ResponseModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MvcConsumeApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task< IActionResult> Index()
        {
            var client= _httpClientFactory.CreateClient();
            var responsemessage= await client.GetAsync("http://localhost:57778/api/Products");
            if (responsemessage.StatusCode==System.Net.HttpStatusCode.OK)
            {
                var jsondata = await responsemessage.Content.ReadAsStringAsync();
                var result= JsonConvert.DeserializeObject<List<ProductResponseModel>>(jsondata);
                return View(result);
            }
            else
            {
                return View(null);
            }
           
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Create(ProductCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var jsondata = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var responsemessage= await client.PostAsync("http://localhost:57778/api/Products", content);
                if (responsemessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["errorMessage"] = $"{(int)responsemessage.StatusCode} hata kodu";
                    return View(model);
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task< IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responsemessage =await client.GetAsync($"http://localhost:57778/api/Products/{id}");
            if (responsemessage.StatusCode==System.Net.HttpStatusCode.OK)
            {
                var jsondata = await responsemessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProductUpdateModel>(jsondata);
                return View(data);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var jsondata = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
            var responsemessage = await client.PutAsync("http://localhost:57778/api/Products", stringContent);
            if (responsemessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");

            }
            return View();
        }
        public async Task< IActionResult> Remove(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responsemessage =await client.DeleteAsync($"http://localhost:57778/api/Products/{id}");
            if (responsemessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>  Upload(IFormFile file)
        {
            var client = _httpClientFactory.CreateClient();

            var stream = new MemoryStream();

            await file.CopyToAsync(stream);
            var bytes = stream.ToArray();

            ByteArrayContent byteArrayContent = new ByteArrayContent(bytes);
            byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(byteArrayContent, "formfile",file.FileName);

            await client.PostAsync("http://localhost:57778/api/Products/upload", formData);

            return RedirectToAction("Index", "Home");

        }
    }
}
