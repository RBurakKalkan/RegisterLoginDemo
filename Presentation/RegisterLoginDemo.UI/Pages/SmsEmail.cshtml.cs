using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RegisterLoginDemo.Domain.Entities;
using System.Text;

namespace RegisterLoginDemo.UI.Pages
{
    public class SmsEmailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SmsEmailModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public User User { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(string verificationMethod)
        {
            var serializedUser = _httpContextAccessor.HttpContext.Session.GetString("LoggedInUser");
            if (!string.IsNullOrEmpty(serializedUser))
            {
                User = JsonConvert.DeserializeObject<User>(serializedUser);
            }
            if (User == null)
            {
                return RedirectToPage("/Login");
            }

            SendType deliveryMethod;
            if (verificationMethod == "SMS")
            {
                deliveryMethod = SendType.SMS;
            }
            else if (verificationMethod == "EMAIL")
            {
                deliveryMethod = SendType.Email;
            }
            else
            {
                return Page();
            }

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://localhost:7240/");

            var endpoint = "/api/SendVerificationCode";
            var requestBody = new { user = User, deliveryMethod = deliveryMethod };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Dogrulama kodu gonderildi.";
                return RedirectToPage("/Verification");
            }
            else
            {
                TempData["ErrorMessage"] = "Dogrulama kodu gonderilemedi.";
                return Page();
            }
        }
    }
}
