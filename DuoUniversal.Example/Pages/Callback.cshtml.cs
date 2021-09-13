
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DuoUniversal.Example.Pages
{
    public class CallbackModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IDuoClientProvider _duoClientProvider;

        public string AuthResponse { get; set; }

        public CallbackModel(ILogger<IndexModel> logger, IDuoClientProvider duoClientProvider)
        {
            _logger = logger;
            _duoClientProvider = duoClientProvider;
        }

        public async Task<IActionResult> OnGet(string state, string code)
        {
            Client duoClient = _duoClientProvider.GetDuoClient();

            // TODO need to match the states, will need session-like store
            // TODO need the username, probably from the session

            IdToken token = await duoClient.ExchangeAuthorizationCodeFor2faResult(code, "username"); // TODO hack for now
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            AuthResponse = JsonSerializer.Serialize(token, options);
            return Page();
        }
    }
}

