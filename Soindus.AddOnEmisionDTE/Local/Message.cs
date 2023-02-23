using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soindus.AddOnEmisionDTE.Local
{
    public class Message
    {
        public int Id { get; set; }

        public string Mensaje { get; set; }

        public bool Success { get; set; }

        public object DTE { get; set; }

        public string Content { get; set; }
    }
}
