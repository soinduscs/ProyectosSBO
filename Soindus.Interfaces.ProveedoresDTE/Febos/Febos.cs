using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;

namespace Soindus.Interfaces.ProveedoresDTE
{
    public class Febos
    {
        public string Token { get; set; }
        public string RutEmpresa { get; set; }
        private string FiltroTipo;
        private string FiltroSN;
        private string FechaIni;
        private string FechaFin;
        private string FiltroFecha;
        private string Filtros;

        public Febos()
        {
        }

        /// <summary>
        /// Obtiene los documentos desde Febos según filtros aplicados
        /// </summary>
        /// <param name="TipoDocumento"></param>
        /// <param name="RutReceptor"></param>
        /// <param name="RutEmisor"></param>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        /// <param name="Pagina"></param>
        /// <returns></returns>
        public Local.Message ObtenerDocumentos(string TipoDocumento = "", string RutReceptor = "", string RutEmisor = "", string FechaInicial = "", string FechaFinal = "", string Pagina = "1")
        {
            // Filtro por Tipo de Documento
            if (!String.IsNullOrEmpty(TipoDocumento))
            {
                FiltroTipo = string.Format("tipoDocumento:{0}|", TipoDocumento);
            }
            else
            {
                FiltroTipo = string.Format("tipoDocumento:33,34|");
            }

            // Filtro por Socio de Negocio
            if (!String.IsNullOrEmpty(RutEmisor))
            {
                FiltroSN = string.Format("rutReceptor:{0}|rutEmisor:{1}|", RutReceptor, RutEmisor);
            }
            else
            {
                FiltroSN = string.Format("rutReceptor:{0}|", RutReceptor);
            }
            FiltroSN = FiltroSN.Replace(".", string.Empty);

            // Filtro por Fechas
            DateTime dt;
            String Mes = String.Empty;
            String Dia = String.Empty;
            // Fechas en formato AAAA-MM-DD
            if (!String.IsNullOrEmpty(FechaInicial) && !String.IsNullOrEmpty(FechaFinal))
            {
                FechaIni = String.Format("{0}-{1}-{2}", FechaInicial.Substring(0, 4), FechaInicial.Substring(4, 2), FechaInicial.Substring(6, 2));
                FechaFin = String.Format("{0}-{1}-{2}", FechaFinal.Substring(0, 4), FechaFinal.Substring(4, 2), FechaFinal.Substring(6, 2));
            }
            else
            {
                // Por defecto trae los últimos 30 días
                dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaFin = String.Format("{0}-{1}-{2}", dt.Year.ToString(), Mes, Dia);

                //dt = DateTime.Today.AddDays(-30);
                dt = DateTime.Today.AddDays(-1);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaIni = String.Format("{0}-{1}-{2}", dt.Year.ToString(), Mes, Dia);
            }
            FiltroFecha = string.Format("fechaEmision:{0}--{1}|", FechaIni, FechaFin);

            Filtros = string.Format("{0}{1}{2}estadoSii:0,1,2,3,4,5,6,7,8,9|incompleto:N", FiltroSN, FiltroFecha, FiltroTipo);

            Local.Message result = new Local.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.febos.cl/produccion/documentos";
                var client = new RestClient(sPath);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("token", Token);
                request.AddHeader("empresa", RutEmpresa);
                request.AddQueryParameter("campos", "trackId,tipoDocumento,folio,rutEmisor,razonSocialEmisor,rutReceptor,razonSocialReceptor,rutCesionario,razonSocialCesionario,indicadorDeTraslado,fechaCesion,codigoSii,fechaEmision,fechaRecepcion,fechaRecepcionSii,plazo,estadoComercial,estadoSii,fechaReciboMercaderia,formaDePago,montoTotal,iva,contacto,correoReceptor,fechaCesion,tipo,monto,lugar,comentario,fecha,medio,tpoTraVenta,tpoTranCompra,tpoTranCompraCodIva,tieneNc,tieneNd,cantSolicitudesDTE");
                request.AddQueryParameter("filtros", Filtros);
                request.AddQueryParameter("itemsPorPagina", "20");
                request.AddQueryParameter("orden", "-fechaEmision");
                request.AddQueryParameter("pagina", Pagina);
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

        /// <summary>
        /// Obtiene los documentos emitidos desde Febos según filtros aplicados
        /// </summary>
        /// <param name="TipoDocumento"></param>
        /// <param name="RutReceptor"></param>
        /// <param name="RutEmisor"></param>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        /// <param name="Pagina"></param>
        /// <returns></returns>
        public Local.Message ObtenerDocumentosEmitidos(string TipoDocumento = "", string RutEmisor = "", string RutReceptor = "", string FechaInicial = "", string FechaFinal = "", string Pagina = "1")
        {
            // Filtro por Tipo de Documento
            if (!String.IsNullOrEmpty(TipoDocumento))
            {
                FiltroTipo = string.Format("tipoDocumento:{0}|", TipoDocumento);
            }
            else
            {
                FiltroTipo = string.Format("tipoDocumento:33,34,43,46,52,56,61,110,111,112|");
            }

            // Filtro por Socio de Negocio
            if (!String.IsNullOrEmpty(RutReceptor))
            {
                FiltroSN = string.Format("rutEmisor:{0}|rutReceptor:{1}|", RutEmisor, RutReceptor);
            }
            else
            {
                FiltroSN = string.Format("rutEmisor:{0}|", RutEmisor);
            }
            FiltroSN = FiltroSN.Replace(".", string.Empty);

            // Filtro por Fechas
            DateTime dt;
            String Mes = String.Empty;
            String Dia = String.Empty;
            // Fechas en formato AAAA-MM-DD
            if (!String.IsNullOrEmpty(FechaInicial) && !String.IsNullOrEmpty(FechaFinal))
            {
                FechaIni = String.Format("{0}-{1}-{2}", FechaInicial.Substring(0, 4), FechaInicial.Substring(4, 2), FechaInicial.Substring(6, 2));
                FechaFin = String.Format("{0}-{1}-{2}", FechaFinal.Substring(0, 4), FechaFinal.Substring(4, 2), FechaFinal.Substring(6, 2));
            }
            else
            {
                // Por defecto trae los últimos 30 días
                dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaFin = String.Format("{0}-{1}-{2}", dt.Year.ToString(), Mes, Dia);

                //dt = DateTime.Today.AddDays(-30);
                dt = DateTime.Today.AddDays(-1);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaIni = String.Format("{0}-{1}-{2}", dt.Year.ToString(), Mes, Dia);
            }
            FiltroFecha = string.Format("fechaEmision:{0}--{1}|", FechaIni, FechaFin);

            Filtros = string.Format("{0}{1}{2}estadoSii:0,1,2,3,4,5,6,7,8,9|incompleto:N", FiltroSN, FiltroFecha, FiltroTipo);

            Local.Message result = new Local.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.febos.cl/produccion/documentos";
                var client = new RestClient(sPath);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("token", Token);
                request.AddHeader("empresa", RutEmpresa);
                request.AddQueryParameter("campos", "trackId,tipoDocumento,folio,rutEmisor,razonSocialEmisor,rutReceptor,razonSocialReceptor,rutCesionario,razonSocialCesionario,indicadorDeTraslado,fechaCesion,codigoSii,fechaEmision,fechaRecepcion,fechaRecepcionSii,plazo,estadoComercial,estadoSii,fechaReciboMercaderia,formaDePago,montoTotal,iva,contacto,correoReceptor,fechaCesion,tipo,monto,lugar,comentario,fecha,medio,tpoTraVenta,tpoTranCompra,tpoTranCompraCodIva,tieneNc,tieneNd,cantSolicitudesDTE");
                request.AddQueryParameter("filtros", Filtros);
                request.AddQueryParameter("itemsPorPagina", "20");
                request.AddQueryParameter("orden", "-fechaEmision");
                request.AddQueryParameter("pagina", Pagina);
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

        /// <summary>
        /// Obtiene el documento según ID Febos
        /// </summary>
        /// <param name="FebosId"></param>
        /// <returns></returns>
        public Local.Message ObtenerDocumento(string FebosId = "")
        {
            Local.Message result = new Local.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.febos.cl/produccion/documentos/" + FebosId;
                var client = new RestClient(sPath);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("token", Token);
                request.AddHeader("empresa", RutEmpresa);
                request.AddQueryParameter("xml", "si");
                request.AddQueryParameter("imagen", "no");
                request.AddQueryParameter("tipoImagen", "0");
                request.AddQueryParameter("regenerar", "no");
                request.AddQueryParameter("incrustar", "si");
                request.AddQueryParameter("xmlFirmado", "si");
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

        /// <summary>
        /// Obtiene el PDF del documento según ID Febos
        /// </summary>
        /// <param name="FebosId"></param>
        /// <returns></returns>
        public Local.Message ObtenerPDFDocumento(string FebosId = "")
        {
            Local.Message result = new Local.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.febos.cl/produccion/documentos/" + FebosId;
                var client = new RestClient(sPath);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("token", Token);
                request.AddHeader("empresa", RutEmpresa);
                request.AddQueryParameter("xml", "no");
                request.AddQueryParameter("imagen", "si");
                request.AddQueryParameter("tipoImagen", "0");
                request.AddQueryParameter("regenerar", "no");
                request.AddQueryParameter("incrustar", "no");
                request.AddQueryParameter("xmlFirmado", "no");
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

        /// <summary>
        /// Realiza el cambio del estado comercial de un documento en Febos
        /// </summary>
        /// <param name="FebosId"></param>
        /// <param name="Estado"></param>
        /// <param name="Recinto"></param>
        /// <param name="Motivo"></param>
        /// <returns></returns>
        public Local.Message CambiarEstadoComercial(string FebosId = "", EstadosComerciales Estado = EstadosComerciales.Acuse_Recibo, string Recinto = "Casa Matriz", string Motivo = "")
        {
            Local.Message result = new Local.Message();
            try
            {
                // usando restsharp
                string sPath = @"https://api.febos.cl/produccion/documentos/" + FebosId + @"/intercambio";
                var client = new RestClient(sPath);
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("token", Token);
                request.AddHeader("empresa", RutEmpresa);
                request.AddQueryParameter("vincularAlSii", "si");
                string accion = string.Empty;
                switch (Estado)
                {
                    case EstadosComerciales.Acuse_Recibo:
                        accion = "ADR";
                        break;
                    case EstadosComerciales.Aceptacion_Comercial:
                        accion = "ACD";
                        request.AddQueryParameter("recinto", Recinto);
                        request.AddQueryParameter("motivo", Motivo);
                        break;
                    case EstadosComerciales.Rechazo_Comercial:
                        accion = "RCD";
                        request.AddQueryParameter("recinto", Recinto);
                        request.AddQueryParameter("motivo", Motivo);
                        break;
                    case EstadosComerciales.Aceptacion_Recibo_Mercaderias:
                        accion = "ERM";
                        request.AddQueryParameter("recinto", Recinto);
                        request.AddQueryParameter("motivo", Motivo);
                        break;
                    case EstadosComerciales.Reclamo_Parcial:
                        accion = "RFP";
                        break;
                    case EstadosComerciales.Reclamo_Total:
                        accion = "RFT";
                        break;
                }
                request.AddQueryParameter("tipoAccion", accion);
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

        /// <summary>
        /// Enumerador de estados comerciales Febos
        /// </summary>
        public enum EstadosComerciales
        {
            Acuse_Recibo = 0, //Acuse de recibo para indicar que una factura ya fue recibida
            Aceptacion_Comercial = 1, //Aceptación Comercial del documento
            Rechazo_Comercial = 2, //Rechazo comercial del documento por alguna discrepancia en la formalidad del documento
            Aceptacion_Recibo_Mercaderias = 3, //Recibo de Mercaderías o Servicios conforme
            Reclamo_Parcial = 4, //Reclamo parcial del documento por inconformidad de la entrega del servicio o mercadería
            Reclamo_Total = 5  //Reclamo total del documento por inconformidad de la entrega del servicio o mercadería
        }
    }
}
