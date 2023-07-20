using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Newtonsoft.Json;
using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.Pages
{
    public class RegisterModel : PageModel
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [BindProperty]
        public User User { get; set; }

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

            var response = await httpClient.PostAsync("api/RegisterUser", content); 

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Kullanici basariyla kaydedildi.";
                return RedirectToPage("/Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Kullanici kaydi basarisiz!";
                return Page();
            }
        }

    }
}
