﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnFactoringCob.Comun
{
    public class Message
    {
        public int Id { get; set; }

        public string Mensaje { get; set; }

        public bool Success { get; set; }

        public object Recordset { get; set; }

        public string Content { get; set; }

        public int DocEntry { get; set; }

        public Clases.Factoring Factoring { get; set; }
    }
}
