using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Linnworks.UITests.Core
{
    public class RestApiHelper
    {
        private readonly HttpClient _httpClient;
        private readonly CookieContainer _cookieContainer;
        
        public RestApiHelper()
        {
            _cookieContainer = new CookieContainer();
            _httpClient = new HttpClient(new HttpClientHandler {CookieContainer = _cookieContainer})
            {
                BaseAddress = new Uri(Constants.ServiceUrl),
                
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task FastLoginAsync()
        {
            
            var uri = _httpClient.BaseAddress + "api/auth/login";
            using (var content = new StringContent("{\"token\":\"bccf905c-6592-40f2-8db1-c976791fa40a\"}", Encoding.UTF8, "application/json"))
            {
                var response = await _httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
                var cookie = _cookieContainer.GetCookies(new Uri( uri)).Cast<Cookie>().FirstOrDefault();
                DriverManager.Current.Driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(cookie?.Name, cookie?.Value, cookie?.Path));
                DriverManager.Current.Driver.Navigate().Refresh();
            }
        }
    }
}
