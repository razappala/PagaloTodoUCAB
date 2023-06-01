using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using UCABPagaloTodoWeb.Models;
using UCABPagaloTodoWeb.Models.CurrentUser;

namespace UCABPagaloTodoWeb.Controllers;

public class ConsumerController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ConsumerController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<IActionResult> Index()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PagaloTodoApi");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser().Token);
            var response = await client.GetAsync("/consumers");
            response.EnsureSuccessStatusCode();
            var items = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            IEnumerable<ConsumerModel> consumers = JsonSerializer.Deserialize<IEnumerable<ConsumerModel>>(items, options)!;
            return View(consumers);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    public IActionResult Create()
    {
        return View();
    }
    
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Create(ConsumerRequest consumer)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PagaloTodoApi");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser().Token);
            var response = await client.PostAsJsonAsync("/consumers", consumer);
            var result = await response.Content.ReadAsStringAsync();
            TempData["success"] = "Consumer Created Successfully";
            return RedirectToAction("Index");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    [HttpGet]
    [Route("showConsumer/{id:Guid}", Name = "showConsumer")]
    public async Task<IActionResult> Show(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PagaloTodoApi");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser().Token);
            var response = await client.GetAsync($"/consumers/{id}");
            response.EnsureSuccessStatusCode();
            var items = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            ConsumerModel consumer = JsonSerializer.Deserialize<ConsumerModel>(items, options)!;
            return View(consumer);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PagaloTodoApi");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser().Token);
            var response = await client.DeleteAsync($"/consumers/{id}");
            response.EnsureSuccessStatusCode();
            var items = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            Guid consumer = JsonSerializer.Deserialize<Guid>(items, options)!;
            TempData["success"] = "Service Deleted Successfully";
            return RedirectToAction("Index");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    
    [HttpGet]
    [Route("updateConsumer/{id:Guid}", Name = "updateConsumer")]
    public async Task<IActionResult> Update(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PagaloTodoApi");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser().Token);
            var consumersResponse = await client.GetAsync($"/consumers/{id}");
            consumersResponse.EnsureSuccessStatusCode();
            var consumerItem = await consumersResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            ConsumerModel consumer = JsonSerializer.Deserialize<ConsumerModel>(consumerItem, options)!;
            ViewBag.Id = id;
            ConsumerRequest consumerRequest = new ConsumerRequest()
            {
                Username = consumer.Username,
                Email = consumer.Email,
                ConsumerId = consumer.ConsumerId,
                Name = consumer.Name,
                LastName = consumer.LastName,
                Status = consumer.Status
            };
            return View(consumerRequest);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    
    [HttpPost]
    [Route("putConsumer/{id:Guid}", Name = "putConsumer")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Update(ConsumerRequest consumer, Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PagaloTodoApi");
            // var stringContent = new StringContent(JsonSerializer.Serialize(Consumer));
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser().Token);
            var response = await client.PutAsJsonAsync($"/consumers/{id}", consumer);
            var result = await response.Content.ReadAsStringAsync();
            TempData["success"] = "Consumer Updated Successfully";
            if (CurrentUser.GetUser().UserType == "consumer")
            {
                return RedirectToAction("Index2", "Home");
            }
            return RedirectToAction("Index");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }

    }

    [HttpGet]
    //TODO: No traer todo de la bdd sino definir una ruta para obtener true/false
    public async Task<JsonResult> CheckUsername(string username)
    {
        var client = _httpClientFactory.CreateClient("PagaloTodoApi");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser().Token);
        var response = await client.GetAsync("/consumers");
        response.EnsureSuccessStatusCode();
        var items = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        IEnumerable<ConsumerModel> consumers = JsonSerializer.Deserialize<IEnumerable<ConsumerModel>>(items, options)!;
        bool isUnique = true;
        foreach (ConsumerModel consumer in consumers)
        {
            if (consumer.Username == username && consumer.Id != CurrentUser.GetUser().Id)
            {
                isUnique = false;
                break;
            }
        }
        return Json(isUnique);
    }
    
    //TODO: Paso anterior a pagar
    public async Task<IActionResult> MakePayment()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PagaloTodoApi");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser().Token);
            var response = await client.GetAsync("/services");
            response.EnsureSuccessStatusCode();
            var items = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            IEnumerable<ServiceModel> services =
                JsonSerializer.Deserialize<IEnumerable<ServiceModel>>(items, options)!;
            ViewBag.Message = services;
            return View();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    
    
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> MakePayment(PaymentRequest payment)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PagaloTodoApi");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser().Token);
            var response = await client.PostAsJsonAsync("/payments", payment);
            var result = await response.Content.ReadAsStringAsync();
            TempData["success"] = "Payment Done Successfully";
            return RedirectToAction("Index2", "Home");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    [Route("/payments/{id:Guid}", Name = "paymentsConsumer")]
    public async Task<IActionResult> GetPayments(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PagaloTodoApi");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser().Token);
            var response = await client.GetAsync($"/payments/consumer/{id}");
            response.EnsureSuccessStatusCode();
            var items = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            IEnumerable<PaymentModel> payments = JsonSerializer.Deserialize<IEnumerable<PaymentModel>>(items, options)!;
            return View(payments);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    
    public IActionResult RegistrerConsumer()
    {
        return View();
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> RegistrerConsumer(ConsumerRequest consumer)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PagaloTodoApi");
            var response = await client.PostAsJsonAsync("/consumers", consumer);
            var result = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Consumer", "Login");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    
    [Route("consumer/updatePassword/{token:guid?}")]
    public IActionResult UpdatePassword(Guid? token)
    {
        ViewBag.Token = token;
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("consumer/updatePswd/{token:guid?}", Name="updatePswdPost")]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest request, Guid? token)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PagaloTodoApi");
            if (token == null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser().Token);
                var id = CurrentUser.GetUser().Id;
                var response = await client.PutAsJsonAsync($"/consumers/{id}/password", request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                TempData["success"] = "Password changed successfully";
                return RedirectToAction("Index2", "Home");
            }
            var responseReset = await client.PostAsJsonAsync($"/api/login/resetpassword?token={token}", request);
            responseReset.EnsureSuccessStatusCode();
            var resultReset = await responseReset.Content.ReadAsStringAsync();
            TempData["success"] = "Password set successfully";
            return RedirectToAction("Index", "Home");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            ModelState.AddModelError(string.Empty, "La contraseña no cumple con los requisitos. Por favor, inténtalo de nuevo.");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Se ha producido un error inesperado. Por favor, inténtalo de nuevo más tarde.");
        }

        return View(request);
    }
}