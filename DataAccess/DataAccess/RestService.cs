
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class RestService
    {
        HttpClient client;
        string grant_type = "password";


        //Il faut reference les models dans cette libraiarie pour le USer;
        

        public RestService()
        {

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }
        public async Task<Token> Login(User user)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("grant_type", grant_type));
            postData.Add(new KeyValuePair<string, string>("email", user.email));
            postData.Add(new KeyValuePair<string, string>("password", user.password));
            var content = new FormUrlEncodedContent(postData);
            var webUrl = "www.HappySpark.com";
            var response = await PostReponse<Token>(webUrl,content);
            DateTime dt = new DateTime();
            dt = DateTime.Today;
            response.expire_date = dt.AddSeconds(response.expire_in);
            return response;

        }
        public async Task<T> PostReponseLogin<T>(string webUrl, FormUrlEncodedContent content) where T : class
        {
            // ici entrer l'url de l'hebergeur
          
            var response = await client.PostAsync(webUrl, content);
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<Token>(jsonResult);
            return responseObject;
        }


        public async Task<T> PostResponse<T>(string wenUrl, string jsonString) where T : class
        {
            var token = AppContext.TokenDatabase.GetToken();
            string ContentType = "application/json";
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.AuthenticationHeaderValue("Bearer", Token.access_token);
            try
            {
                var result = await client.PostAsync(webUrl, new StringContent(jsonString, Encoding.UTF8, ContentType));

                if(result.)
                var jsonResult = result.Content.ReadAsStringAsync().Result;
                var ContentResp = JsonConvert.DeserializeObject<T>(jsonResult);
                return ContentResp;
            }
            return null;

        }

        public async Task<T> GEtResponse<T>(string webUrl) where T : class
        {
            var Token = AppContext.TokenDatabase.GetToken();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.AuthenticationHeaderValue("Bearer", Token.access_token);
            var response = await client.GetAsync(webUrl);
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            var ContentResp = JsonConvert.DeserializeObject<T>(jsonResult);
            return ContentResp;
        }


    }
}
