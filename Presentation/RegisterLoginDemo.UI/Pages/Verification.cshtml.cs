using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Newtonsoft.Json;
using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.Pages
{
    public class VerificationModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VerificationModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string VerificationCode { get; set; }

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
            httpClient.BaseAddress = new Uri("https://localhost:7240/"); // Replace with your API base URL

            var userJson = HttpContext.Session.GetString("LoggedInUser");
            var user = JsonConvert.DeserializeObject<User>(userJson);

            var requestBody = new 
            {
                User = user,
                VerificationCode = VerificationCode
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/api/VerifyCodeAndSignIn", content);

            if (response.IsSuccessStatusCode)
            {
                var tokenJson = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject(tokenJson);

                TempData["Message"] = $"Dogrulama basarili. JwtToken = {token}";
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = "Dogrulama kodu yanlis veya suresi bitti.";
                return Page();
            }
        }
    }
}
