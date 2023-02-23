using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClasesDTE = Soindus.Clases.DTE;

namespace Soindus.Svc.ProveedoresDTE.Local
{
    public class Message
    {
        public int Id { get; set; }

        public string Mensaje { get; set; }

        public bool Success { get; set; }

        public object DTE { get; set; }

        public string Content { get; set; }

        public string TpoDocRef { get; set; }

        public string FolioRef { get; set; }

        public string DocEntryBase { get; set; }

        public string DocTypeBase { get; set; }

        public string ObjTypeBase { get; set; }

        public string CardCode { get; set; }

        public ClasesDTE.Documento objDTE { get; set; }
    }
}
