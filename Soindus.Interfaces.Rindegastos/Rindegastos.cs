using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

namespace Soindus.Interfaces.Rindegastos
{
    public class Rindegastos
    {
        public string Token { get; set; }

        public Rindegastos()
        {
        }

        public Clases.Message ObtenerRendiciones(string Since = "", string Until = "", string TypeDateFilter = "1",
            string Currency = "", string Status = "", string ExpensePolicyId = "", string IntegrationStatus = "",
            string IntegrationCode = "", string IntegrationDate = "", string UserId = "",
            string OrderBy = "3", string Order = "DESC", string ResultsPerPage = "20", string Page = "1")
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/getExpenseReports";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(Since))
                {
                    request.AddQueryParameter("Since", Since);
                }
                if (!string.IsNullOrEmpty(Until))
                {
                    request.AddQueryParameter("Until", Until);
                }
                if (!string.IsNullOrEmpty(TypeDateFilter))
                {
                    request.AddQueryParameter("TypeDateFilter", TypeDateFilter);
                }
                if (!string.IsNullOrEmpty(Currency))
                {
                    request.AddQueryParameter("Currency", Currency);
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    request.AddQueryParameter("Status", Status);
                }
                if (!string.IsNullOrEmpty(ExpensePolicyId))
                {
                    request.AddQueryParameter("ExpensePolicyId", ExpensePolicyId);
                }
                if (!string.IsNullOrEmpty(IntegrationStatus))
                {
                    request.AddQueryParameter("IntegrationStatus", IntegrationStatus);
                }
                if (!string.IsNullOrEmpty(IntegrationCode))
                {
                    request.AddQueryParameter("IntegrationCode", IntegrationCode);
                }
                if (!string.IsNullOrEmpty(IntegrationDate))
                {
                    request.AddQueryParameter("IntegrationDate", IntegrationDate);
                }
                if (!string.IsNullOrEmpty(UserId))
                {
                    request.AddQueryParameter("UserId", UserId);
                }
                if (!string.IsNullOrEmpty(OrderBy))
                {
                    request.AddQueryParameter("OrderBy", OrderBy);
                }
                if (!string.IsNullOrEmpty(Order))
                {
                    request.AddQueryParameter("Order", Order);
                }
                if (!string.IsNullOrEmpty(ResultsPerPage))
                {
                    request.AddQueryParameter("ResultsPerPage", ResultsPerPage);
                }
                if (!string.IsNullOrEmpty(Page))
                {
                    request.AddQueryParameter("Page", Page);
                }

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message ObtenerRendicion(string Id)
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/getExpenseReport";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(Id))
                {
                    request.AddQueryParameter("Id", Id);
                }

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message CambiarEstadoRendicion(string Id, string IntegrationStatus, string IntegrationCode, string IntegrationDate)
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/setExpenseReportIntegration";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                var _params = new Clases.EstadoIntegracion();
                if (!string.IsNullOrEmpty(Id))
                {
                    _params.Id = Int32.Parse(Id);
                }
                if (!string.IsNullOrEmpty(IntegrationStatus))
                {
                    _params.IntegrationStatus = Int32.Parse(IntegrationStatus);
                }
                if (!string.IsNullOrEmpty(IntegrationCode))
                {
                    _params.IntegrationCode = IntegrationCode;
                }
                if (!string.IsNullOrEmpty(IntegrationDate))
                {
                    _params.IntegrationDate = IntegrationDate;
                }

                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                var _json = JsonConvert.SerializeObject(_params, settings);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(_json);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message CambiarEstadoPersonalizado(string Id, string IdAdmin, string CustomStatus, string CustomMessage)
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/setExpenseReportCustomStatus";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                var _params = new Clases.EstadoPersonalizado();
                if (!string.IsNullOrEmpty(Id))
                {
                    _params.Id = Int32.Parse(Id);
                }

                if (!string.IsNullOrEmpty(IdAdmin))
                {
                    _params.IdAdmin = Int32.Parse(IdAdmin);
                }
                if (!string.IsNullOrEmpty(CustomStatus))
                {
                    _params.CustomStatus = CustomStatus;
                }
                if (!string.IsNullOrEmpty(CustomMessage))
                {
                    _params.CustomMessage = CustomMessage;
                }

                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                var _json = JsonConvert.SerializeObject(_params, settings);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(_json);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message ObtenerGastos(string Since = "", string Until = "", string Currency = "",
            string Status = "", string Category = "", string ReportId = "", string ExpensePolicyId = "",
            string IntegrationStatus = "", string IntegrationCode = "", string IntegrationDate = "", string UserId = "",
            string OrderBy = "1", string Order = "DESC", string ResultsPerPage = "100", string Page = "1")
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/getExpenses";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(Since))
                {
                    request.AddQueryParameter("Since", Since);
                }
                if (!string.IsNullOrEmpty(Until))
                {
                    request.AddQueryParameter("Until", Until);
                }
                if (!string.IsNullOrEmpty(Currency))
                {
                    request.AddQueryParameter("Currency", Currency);
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    request.AddQueryParameter("Status", Status);
                }
                if (!string.IsNullOrEmpty(Category))
                {
                    request.AddQueryParameter("Category", Category);
                }
                if (!string.IsNullOrEmpty(ReportId))
                {
                    request.AddQueryParameter("ReportId", ReportId);
                }
                if (!string.IsNullOrEmpty(ExpensePolicyId))
                {
                    request.AddQueryParameter("ExpensePolicyId", ExpensePolicyId);
                }
                if (!string.IsNullOrEmpty(IntegrationStatus))
                {
                    request.AddQueryParameter("IntegrationStatus", IntegrationStatus);
                }
                if (!string.IsNullOrEmpty(IntegrationCode))
                {
                    request.AddQueryParameter("IntegrationCode", IntegrationCode);
                }
                if (!string.IsNullOrEmpty(IntegrationDate))
                {
                    request.AddQueryParameter("IntegrationDate", IntegrationDate);
                }
                if (!string.IsNullOrEmpty(UserId))
                {
                    request.AddQueryParameter("UserId", UserId);
                }
                if (!string.IsNullOrEmpty(OrderBy))
                {
                    request.AddQueryParameter("OrderBy", OrderBy);
                }
                if (!string.IsNullOrEmpty(Order))
                {
                    request.AddQueryParameter("Order", Order);
                }
                if (!string.IsNullOrEmpty(ResultsPerPage))
                {
                    request.AddQueryParameter("ResultsPerPage", ResultsPerPage);
                }
                if (!string.IsNullOrEmpty(Page))
                {
                    request.AddQueryParameter("Page", Page);
                }

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message ObtenerGasto(string Id)
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/getExpense";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(Id))
                {
                    request.AddQueryParameter("Id", Id);
                }

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message CambiarEstadoGasto(string Id, string IntegrationStatus, string IntegrationCode, string IntegrationDate)
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/setExpenseIntegration";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                var _params = new Clases.EstadoIntegracion();
                if (!string.IsNullOrEmpty(Id))
                {
                    _params.Id = Int32.Parse(Id);
                }
                if (!string.IsNullOrEmpty(IntegrationStatus))
                {
                    _params.IntegrationStatus = Int32.Parse(IntegrationStatus);
                }
                if (!string.IsNullOrEmpty(IntegrationCode))
                {
                    _params.IntegrationCode = IntegrationCode;
                }
                if (!string.IsNullOrEmpty(IntegrationDate))
                {
                    _params.IntegrationDate = IntegrationDate;
                }

                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                var _json = JsonConvert.SerializeObject(_params, settings);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(_json);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message ObtenerUsuarios(string OrderBy = "1", string Order = "DESC",
            string ResultsPerPage = "100", string Page = "1")
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/getUsers";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(OrderBy))
                {
                    request.AddQueryParameter("OrderBy", OrderBy);
                }
                if (!string.IsNullOrEmpty(Order))
                {
                    request.AddQueryParameter("Order", Order);
                }
                if (!string.IsNullOrEmpty(ResultsPerPage))
                {
                    request.AddQueryParameter("ResultsPerPage", ResultsPerPage);
                }
                if (!string.IsNullOrEmpty(Page))
                {
                    request.AddQueryParameter("Page", Page);
                }

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message ObtenerUsuario(string Id)
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/getUser";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(Id))
                {
                    request.AddQueryParameter("Id", Id);
                }

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message ObtenerFondos(string Status = "", string OrderBy = "1", string Order = "DESC",
            string ResultsPerPage = "100", string Page = "1", string From = "", string To = "")
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/getFunds";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(Status))
                {
                    request.AddQueryParameter("Status", Status);
                }
                if (!string.IsNullOrEmpty(OrderBy))
                {
                    request.AddQueryParameter("OrderBy", OrderBy);
                }
                if (!string.IsNullOrEmpty(Order))
                {
                    request.AddQueryParameter("Order", Order);
                }
                if (!string.IsNullOrEmpty(ResultsPerPage))
                {
                    request.AddQueryParameter("ResultsPerPage", ResultsPerPage);
                }
                if (!string.IsNullOrEmpty(Page))
                {
                    request.AddQueryParameter("Page", Page);
                }
                if (!string.IsNullOrEmpty(From))
                {
                    request.AddQueryParameter("From", From);
                }
                if (!string.IsNullOrEmpty(To))
                {
                    request.AddQueryParameter("To", To);
                }

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message ObtenerFondo(string Id)
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/getFund";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(Id))
                {
                    request.AddQueryParameter("Id", Id);
                }

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message CrearFondo(string IdEmployee, string IdAdmin, string FundName, string FundCurrency, string FundCode,
            string FundAmount, string FundComment, string FundFlexibility, string FundAutoDeposit, string FundAutoBlock,
            string FundExpiration, string FundExpirationDate)
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/createFund";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var _params = new Clases.CreaFondo();
                if (!string.IsNullOrEmpty(IdEmployee))
                {
                    _params.IdEmployee = Int32.Parse(IdEmployee);
                }
                if (!string.IsNullOrEmpty(IdAdmin))
                {
                    _params.IdAdmin = Int32.Parse(IdAdmin);
                }
                if (!string.IsNullOrEmpty(FundName))
                {
                    _params.FundName = FundName;
                }
                if (!string.IsNullOrEmpty(FundCurrency))
                {
                    _params.FundCurrency = FundCurrency;
                }
                if (!string.IsNullOrEmpty(FundCode))
                {
                    _params.FundCode = FundCode;
                }
                if (!string.IsNullOrEmpty(FundAmount))
                {
                    _params.FundAmount = double.Parse(FundAmount);
                }
                if (!string.IsNullOrEmpty(FundComment))
                {
                    _params.FundComment = FundComment;
                }
                if (!string.IsNullOrEmpty(FundFlexibility))
                {
                    _params.FundFlexibility = bool.Parse(FundFlexibility);
                }
                if (!string.IsNullOrEmpty(FundAutoDeposit))
                {
                    _params.FundAutoDeposit = bool.Parse(FundAutoDeposit);
                }
                if (!string.IsNullOrEmpty(FundAutoBlock))
                {
                    _params.FundAutoBlock = bool.Parse(FundAutoBlock);
                }
                if (!string.IsNullOrEmpty(FundExpiration))
                {
                    _params.FundExpiration = bool.Parse(FundExpiration);
                }
                if (!string.IsNullOrEmpty(FundExpirationDate))
                {
                    _params.FundExpirationDate = FundExpirationDate;
                }

                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                var _json = JsonConvert.SerializeObject(_params, settings);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(_json);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message ModificarFondo(string Id, string IdAdmin, string FundName, string FundCode,
            string FundComment, string FundFlexibility, string FundAutoDeposit, string FundAutoBlock,
            string FundExpiration, string FundExpirationDate)
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/updateFund";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                var _params = new Clases.ModificaFondo();
                if (!string.IsNullOrEmpty(Id))
                {
                    _params.Id = Int32.Parse(Id);
                }
                if (!string.IsNullOrEmpty(IdAdmin))
                {
                    _params.IdAdmin = Int32.Parse(IdAdmin);
                }
                if (!string.IsNullOrEmpty(FundName))
                {
                    _params.FundName = FundName;
                }
                if (!string.IsNullOrEmpty(FundCode))
                {
                    _params.FundCode = FundCode;
                }
                if (!string.IsNullOrEmpty(FundComment))
                {
                    _params.FundComment = FundComment;
                }
                if (!string.IsNullOrEmpty(FundFlexibility))
                {
                    _params.FundFlexibility = bool.Parse(FundFlexibility);
                }
                if (!string.IsNullOrEmpty(FundAutoDeposit))
                {
                    _params.FundAutoDeposit = bool.Parse(FundAutoDeposit);
                }
                if (!string.IsNullOrEmpty(FundAutoBlock))
                {
                    _params.FundAutoBlock = bool.Parse(FundAutoBlock);
                }
                if (!string.IsNullOrEmpty(FundExpiration))
                {
                    _params.FundExpiration = bool.Parse(FundExpiration);
                }
                if (!string.IsNullOrEmpty(FundExpirationDate))
                {
                    _params.FundExpirationDate = FundExpirationDate;
                }

                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                var _json = JsonConvert.SerializeObject(_params, settings);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(_json);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message DepositarFondo(string Id, string IdAdmin, string DepositAmount)
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/depositMoneyToFund";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var _params = new Clases.DepositaFondo();
                if (!string.IsNullOrEmpty(Id))
                {
                    _params.Id = Int32.Parse(Id);
                }
                if (!string.IsNullOrEmpty(IdAdmin))
                {
                    _params.IdAdmin = Int32.Parse(IdAdmin);
                }
                if (!string.IsNullOrEmpty(DepositAmount))
                {
                    _params.DepositAmount = double.Parse(DepositAmount);
                }

                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                var _json = JsonConvert.SerializeObject(_params, settings);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(_json);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Clases.Message CambiarEstadoFondo(string Id, string IdAdmin, string FundStatus)
        {
            Clases.Message result = new Clases.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.rindegastos.com:443/v1/setFundStatus";
                var client = new RestClient(sPath);
                client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Token);
                //request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                var _params = new Clases.EstadoFondo();
                if (!string.IsNullOrEmpty(Id))
                {
                    _params.Id = Int32.Parse(Id);
                }
                if (!string.IsNullOrEmpty(IdAdmin))
                {
                    _params.IdAdmin = Int32.Parse(IdAdmin);
                }
                if (!string.IsNullOrEmpty(FundStatus))
                {
                    _params.FundStatus = Int32.Parse(FundStatus);
                }

                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                var _json = JsonConvert.SerializeObject(_params, settings);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(_json);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    result.Content = response.Content;
                    result.Success = true;
                }
                else
                {
                    result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", response.ErrorMessage);
                    result.Success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }
    }
}
