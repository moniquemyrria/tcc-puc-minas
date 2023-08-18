
using Kodigos.Infra.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kodigos.API.IntegracaoProtheus.RequestClient
{
    public class RequestClient<T>
         where T : class
    {

        public static async Task<KRetorno<T>> RequestT(HttpConstantes metodo, string requestURI,string data = "")
        {
            var retorno = new KRetorno<T>();
            try
            {
                HttpClient _Client;
                HttpClientHandler hnd = new HttpClientHandler
                {
                    UseCookies = false
                };

                _Client = new HttpClient(hnd);
                
                string path = Config.ConfigHttp.URL;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                _Client.DefaultRequestHeaders.ExpectContinue = false;
                _Client.DefaultRequestHeaders.ConnectionClose = false;


                HttpResponseMessage response;

                string pathComplete = path + Config.ConfigHttp.URLComplete + requestURI;

              


                _Client.DefaultRequestHeaders.Add("Prefer", "odata.maxpagesize=ALL");

                if (metodo == HttpConstantes.GET)
                {
                    _Client.Timeout = TimeSpan.FromMinutes(10);
                    response = await _Client.GetAsync(pathComplete);
                }
                else
                {
                    var dataJson = new StringContent(data, Encoding.UTF8, "application/json");
                    response = await _Client.PostAsync(@pathComplete, dataJson);
                }



                if (response.IsSuccessStatusCode)
                {
                    var responseJson = response.Content.ReadAsStringAsync();
                    T _T = JsonConvert.DeserializeObject<T>(responseJson.Result);
                    //T _T = JsonSerializer.Deserialize<T>(responseJson.Result);
                    retorno.Sucesso = true;
                    retorno.TRetorno = _T;

                }
                else
                {
                    retorno.Sucesso = false;
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        retorno.Message = "Não autorizado.";
                    }
                    else
                    {
                        try
                        {
                            var responseJson = response.Content.ReadAsStringAsync();
                            //ProtheusReturnError error = JsonConvert.DeserializeObject<ProtheusReturnError>(responseJson.Result);
                            retorno.Message = "Erro 505."; //error.Error.Message.Value;
                        }
                        catch (Exception e)
                        {
                            retorno.Message = e.Message;//KDesignException.GetException(e).Message;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                retorno.Sucesso = false;
                retorno.Message = e.Message;//KDesignException.GetException(e).Message;

            }
            return retorno;
        }
        }
    }
