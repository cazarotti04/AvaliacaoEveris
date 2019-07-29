using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoEverisAPI.Models
{
    public class Produto
    {
        public int? CDGPRODUTO { get; set; }
        public string EMPRESA { get; set; }
        public string NOMEPRODUTO { get; set; }

        public int? ENTRADA { get; set; }

        public int? SAIDA { get; set; }

        public int ESTOQUE { get; set; }

        public Produto()
        {

        }
    }
}