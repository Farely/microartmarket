using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceAuthContoller : ControllerBase
    {

        private readonly ILogger<ServiceAuthContoller> _logger;

        public ServiceAuthContoller(ILogger<ServiceAuthContoller> logger)
        {
            _logger = logger;
        }
        /// <summary>
                ///  Получает картину по Id.
                /// </summary>
                /// <returns>Возвращает токен</returns>
                /// <param name="authModel"></param>
                /// >Возвращает токен
                /// </returns>
                /// <response code="200">Возвращает токен</response>
                /// <response code="401">Авторизация не удалась</response>
                [HttpPost("authService")]
                [AllowAnonymous]
                [Consumes("application/x-www-form-urlencoded")]
                public async Task<IActionResult> AuthService([FromForm] AuthRequest authModel)
                {
                
                    _logger.LogInformation(Request.Host.Value + Request.Path);
                    try
                    {
                        //AuthRequest authModel = JsonConvert.DeserializeObject<AuthRequest>(data);
                        var client = new HttpClient();
                        var nvc = new List<KeyValuePair<string, string>>();
                        nvc.Add(new KeyValuePair<string, string>("client_id", authModel.client_id));
                        nvc.Add(new KeyValuePair<string, string>("client_secret", authModel.client_secret));
                        nvc.Add(new KeyValuePair<string, string>("grant_type", authModel.grant_type));
                        nvc.Add(new KeyValuePair<string, string>("scope", authModel.scope));
                     
                  
                        
                        var request = new HttpRequestMessage(HttpMethod.Post,string.Format("{0}://{1}", Request.Scheme, Request.Host) +  $"/connect/token");
                        request.Content = new FormUrlEncodedContent(nvc);
                        var responce = await client.SendAsync(request);
                        
                        client.Dispose();
                        return (Ok(await responce.Content.ReadAsStringAsync()));
                    }
                    catch (Exception e)
                    {
                        _logger.LogCritical(e.ToString());
                        throw;
                    }
                }


    }
}