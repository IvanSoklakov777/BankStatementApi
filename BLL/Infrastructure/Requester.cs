using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace BankStatementApi.BLL.Infrastructure
{
    public class Requester
    {
        private readonly IServiceInfrastructure _serviceInfrastructure;
        public Requester(IServiceInfrastructure serviceInfrastructure)
        {
            _serviceInfrastructure = serviceInfrastructure;       
        }

        public async Task<TEntity> PostRequest<TEntity>(string sectionUrlName, string path, object body) where TEntity : class
        {
            var clientHandler = new HttpClientHandler();
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslpolicy) => { return true; };
            using var httpClient = new HttpClient(clientHandler);          
            var defaultURL = _serviceInfrastructure.Configuration.GetSection("Url").GetSection(sectionUrlName).Value;
            var url = $"{defaultURL}{path}";
            var json = JsonConvert.SerializeObject(body);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, data);
            return await response.Content.ReadFromJsonAsync<TEntity>();
        }

        public async Task<TEntity> GetRequest<TEntity>(string sectionUrlName, string path) where TEntity : class
        {
            var clientHandler = new HttpClientHandler();
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslpolicy) => { return true; };
            using var httpClient = new HttpClient(clientHandler);

            var defaultURL = _serviceInfrastructure.Configuration.GetSection("Url").GetSection(sectionUrlName).Value;
            var url = $"{defaultURL}{path}";
            return await httpClient.GetFromJsonAsync<TEntity>(url);
        }      
    }
}
