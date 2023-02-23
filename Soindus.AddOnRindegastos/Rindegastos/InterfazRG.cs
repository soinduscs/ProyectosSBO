using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterRG = Soindus.Interfaces.Rindegastos;

namespace Soindus.AddOnRindegastos
{
    public class InterfazRG
    {
        public Clases.ResponseExpenseReports ResponseExpenseReports { get; set; }
        public Clases.ExpenseReport ExpenseReport { get; set; }
        public Clases.ResponseExpenses ResponseExpenses { get; set; }
        public Clases.Expense Expense { get; set; }
        public Clases.ResponseUsers ResponseUsers { get; set; }
        public Clases.User User { get; set; }
        public Clases.ResponseFunds ResponseFunds { get; set; }
        public Clases.Fund Fund { get; set; }
        private Clases.Configuracion ExtConf = new Clases.Configuracion();

        public InterfazRG()
        {
        }

        public InterRG.Clases.Message ObtenerRendiciones(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiMjgwNzMiLCJjb21wYW55X2lkIjoiMTk4MzkiLCJyYW5kb20iOiJyYW5kQVBJNWYwNTAxNzhkN2E4NjQuNTEzMzY0MjYifQ.dAqiu2hs0IOvbviJK-YTxkzSEqTURQQgOvbHa7hwoNA";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Since = string.Empty;
            string Until = string.Empty;
            string TypeDateFilter = string.Empty;
            string Currency = string.Empty;
            string Status = string.Empty;
            string ExpensePolicyId = string.Empty;
            string IntegrationStatus = string.Empty;
            string IntegrationCode = string.Empty;
            string IntegrationDate = string.Empty;
            string UserId = string.Empty;
            string OrderBy = string.Empty;
            string Order = string.Empty;
            string ResultsPerPage = string.Empty;
            string Page = string.Empty;
            try
            {
                Since = args[0];
                Until = args[1];
                TypeDateFilter = args[2];
                Currency = args[3];
                Status = args[4];
                ExpensePolicyId = args[5];
                IntegrationStatus = args[6];
                IntegrationCode = args[7];
                IntegrationDate = args[8];
                UserId = args[9];
                OrderBy = args[10];
                Order = args[11];
                ResultsPerPage = args[12];
                Page = args[13];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.ObtenerRendiciones(Since, Until, TypeDateFilter, 
                Currency, Status, ExpensePolicyId, IntegrationStatus,
                IntegrationCode, IntegrationDate, UserId, 
                OrderBy, Order, ResultsPerPage, Page);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.ResponseExpenseReports>(rgResult.Content);
                ResponseExpenseReports = new Clases.ResponseExpenseReports();
                ResponseExpenseReports = _Datos;
            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message ObtenerRendicion(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Id = string.Empty;
            try
            {
                Id = args[0];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.ObtenerRendicion(Id);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.ExpenseReport>(rgResult.Content);
                ExpenseReport = new Clases.ExpenseReport();
                ExpenseReport = _Datos;
            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message CambiarEstadoRendicion(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Id = string.Empty;
            string IntegrationStatus = string.Empty;
            string IntegrationCode = string.Empty;
            string IntegrationDate = string.Empty;
            try
            {
                Id = args[0];
                IntegrationStatus = args[1];
                IntegrationCode = args[2];
                IntegrationDate = args[3];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.CambiarEstadoRendicion(Id, IntegrationStatus, IntegrationCode, IntegrationDate);

            if (rgResult.Success)
            {

            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message CambiarEstadoPersonalizado(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Id = string.Empty;
            string IdAdmin = string.Empty;
            string CustomStatus = string.Empty;
            string CustomMessage = string.Empty;
            try
            {
                Id = args[0];
                IdAdmin = args[1];
                CustomStatus = args[2];
                CustomMessage = args[3];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.CambiarEstadoPersonalizado(Id, IdAdmin, CustomStatus, CustomMessage);

            if (rgResult.Success)
            {

            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message ObtenerGastos(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiMjgwNzMiLCJjb21wYW55X2lkIjoiMTk4MzkiLCJyYW5kb20iOiJyYW5kQVBJNWYwNTAxNzhkN2E4NjQuNTEzMzY0MjYifQ.dAqiu2hs0IOvbviJK-YTxkzSEqTURQQgOvbHa7hwoNA";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Since = string.Empty;
            string Until = string.Empty;
            string Currency = string.Empty;
            string Status = string.Empty;
            string Category = string.Empty;
            string ReportId = string.Empty;
            string ExpensePolicyId = string.Empty;
            string IntegrationStatus = string.Empty;
            string IntegrationCode = string.Empty;
            string IntegrationDate = string.Empty;
            string UserId = string.Empty;
            string OrderBy = string.Empty;
            string Order = string.Empty;
            string ResultsPerPage = string.Empty;
            string Page = string.Empty;
            try
            {
                Since = args[0];
                Until = args[1];
                Currency = args[2];
                Status = args[3];
                Category = args[4];
                ReportId = args[5];
                ExpensePolicyId = args[6];
                IntegrationStatus = args[7];
                IntegrationCode = args[8];
                IntegrationDate = args[9];
                UserId = args[10];
                OrderBy = args[11];
                Order = args[12];
                ResultsPerPage = args[13];
                Page = args[14];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.ObtenerGastos(Since, Until, Currency,
                Status, Category, ReportId, ExpensePolicyId,
                IntegrationStatus, IntegrationCode, IntegrationDate, UserId,
                OrderBy, Order, ResultsPerPage, Page);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.ResponseExpenses>(rgResult.Content);
                ResponseExpenses = new Clases.ResponseExpenses();
                ResponseExpenses = _Datos;
            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message ObtenerGasto(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Id = string.Empty;
            try
            {
                Id = args[0];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.ObtenerGasto(Id);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.Expense>(rgResult.Content);
                Expense = new Clases.Expense();
                Expense = _Datos;
            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message CambiarEstadoGasto(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Id = string.Empty;
            string IntegrationStatus = string.Empty;
            string IntegrationCode = string.Empty;
            string IntegrationDate = string.Empty;
            try
            {
                Id = args[0];
                IntegrationStatus = args[1];
                IntegrationCode = args[2];
                IntegrationDate = args[3];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.CambiarEstadoGasto(Id, IntegrationStatus, IntegrationCode, IntegrationDate);

            if (rgResult.Success)
            {

            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message ObtenerUsuarios(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string OrderBy = string.Empty;
            string Order = string.Empty;
            string ResultsPerPage = string.Empty;
            string Page = string.Empty;
            try
            {
                OrderBy = args[0];
                Order = args[1];
                ResultsPerPage = args[2];
                Page = args[3];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.ObtenerUsuarios(OrderBy, Order, ResultsPerPage, Page);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.ResponseUsers>(rgResult.Content);
                ResponseUsers = new Clases.ResponseUsers();
                ResponseUsers = _Datos;
            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message ObtenerUsuario(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Id = string.Empty;
            try
            {
                Id = args[0];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.ObtenerUsuario(Id);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.User>(rgResult.Content);
                User = new Clases.User();
                User = _Datos;
            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message ObtenerFondos(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };
            
            string Status = string.Empty;
            string OrderBy = string.Empty;
            string Order = string.Empty;
            string ResultsPerPage = string.Empty;
            string Page = string.Empty;
            string From = string.Empty;
            string To = string.Empty;

            try
            {
                Status = args[0];
                OrderBy = args[1];
                Order = args[2];
                ResultsPerPage = args[3];
                Page = args[4];
                From = args[5];
                To = args[6];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.ObtenerFondos(Status, OrderBy, Order, ResultsPerPage, Page, From, To);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.ResponseFunds>(rgResult.Content);
                ResponseFunds = new Clases.ResponseFunds();
                ResponseFunds = _Datos;
            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message ObtenerFondo(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Id = string.Empty;
            try
            {
                Id = args[0];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.ObtenerFondo(Id);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.Fund>(rgResult.Content);
                Fund = new Clases.Fund();
                Fund = _Datos;
            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message CrearFondo(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string IdEmployee = string.Empty;
            string IdAdmin = string.Empty;
            string FundName = string.Empty;
            string FundCurrency = string.Empty;
            string FundCode = string.Empty;
            string FundAmount = string.Empty;
            string FundComment = string.Empty;
            string FundFlexibility = string.Empty;
            string FundAutoDeposit = string.Empty;
            string FundAutoBlock = string.Empty;
            string FundExpiration = string.Empty;
            string FundExpirationDate = string.Empty;
            try
            {
                IdEmployee = args[0];
                IdAdmin = args[1];
                FundName = args[2];
                FundCurrency = args[3];
                FundCode = args[4];
                FundAmount = args[5];
                FundComment = args[6];
                FundFlexibility = args[7];
                FundAutoDeposit = args[8];
                FundAutoBlock = args[9];
                FundExpiration = args[10];
                FundExpirationDate = args[11];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.CrearFondo(IdEmployee, IdAdmin, FundName,
                FundCurrency, FundCode, FundAmount, FundComment,
                FundFlexibility, FundAutoDeposit, FundAutoBlock,
                FundExpiration, FundExpirationDate);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.Fund>(rgResult.Content);
                Fund = new Clases.Fund();
                Fund = _Datos;
            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message ModificarFondo(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Id = string.Empty;
            string IdAdmin = string.Empty;
            string FundName = string.Empty;
            string FundCode = string.Empty;
            string FundComment = string.Empty;
            string FundFlexibility = string.Empty;
            string FundAutoDeposit = string.Empty;
            string FundAutoBlock = string.Empty;
            string FundExpiration = string.Empty;
            string FundExpirationDate = string.Empty;
            try
            {
                Id = args[0];
                IdAdmin = args[1];
                FundName = args[2];
                FundCode = args[3];
                FundComment = args[4];
                FundFlexibility = args[5];
                FundAutoDeposit = args[6];
                FundAutoBlock = args[7];
                FundExpiration = args[8];
                FundExpirationDate = args[9];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.ModificarFondo(Id, IdAdmin, FundName,
                FundCode, FundComment,
                FundFlexibility, FundAutoDeposit, FundAutoBlock,
                FundExpiration, FundExpirationDate);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.Fund>(rgResult.Content);
                Fund = new Clases.Fund();
                Fund = _Datos;
            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message DepositarFondo(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Id = string.Empty;
            string IdAdmin = string.Empty;
            string DepositAmount = string.Empty;
            try
            {
                Id = args[0];
                IdAdmin = args[1];
                DepositAmount = args[2];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.DepositarFondo(Id, IdAdmin, DepositAmount);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.Fund>(rgResult.Content);
                Fund = new Clases.Fund();
                Fund = _Datos;
            }
            result = rgResult;
            return result;
        }

        public InterRG.Clases.Message CambiarEstadoFondo(string[] args)
        {
            InterRG.Clases.Message result = new Interfaces.Rindegastos.Clases.Message();

            //string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNjQxMDMiLCJjb21wYW55X2lkIjoiMTU4MDUiLCJyYW5kb20iOiJyYW5kQVBJNWQ3MWQyOTk3ODU5YjguOTY3OTU2NzEifQ.JFY20KuH9Lcra1t3WHcSfzBeMQfeOXYPCcmqEB2eQ7w";
            string Token = ExtConf.Parametros.Token;

            var interRG = new InterRG.Rindegastos() { Token = Token };

            string Id = string.Empty;
            string IdAdmin = string.Empty;
            string FundStatus = string.Empty;
            try
            {
                Id = args[0];
                IdAdmin = args[1];
                FundStatus = args[2];
            }
            catch (Exception)
            {
            }

            var rgResult = interRG.CambiarEstadoFondo(Id, IdAdmin, FundStatus);

            if (rgResult.Success)
            {
                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.Fund>(rgResult.Content);
                Fund = new Clases.Fund();
                Fund = _Datos;
            }
            result = rgResult;
            return result;
        }
    }
}
