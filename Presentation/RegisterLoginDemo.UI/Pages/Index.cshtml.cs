using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Newtonsoft.Json;
using RegisterLoginDemo.Application.ViewModel;

namespace RegisterLoginDemo.Pages
{
    public class LoginModel : PageModel
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [BindProperty]
        public LoginRequest User { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://localhost:7240/");

            var json = JsonConvert.SerializeObject(User);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/api/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var userJson = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("LoggedInUser", userJson);

                return RedirectToPage("/SmsEmail");
            }
            else
            {
                TempData["ErrorMessage"] = "Yanlis kullanici adi veya sifre.";
                return Page();
            }
        }
    }
}
