﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoEverisAPI.Models
{
    public class Produto
    {
        //[Key]
        //private int? cdgProduto { get; set; }
        //public int? CDGPRODUTO
        //{
        //    get { return cdgProduto; }
        //    set { cdgProduto = value; }
        //}

        //private string produto { get; set; }
        //public string PRODUTO
        //{
        //    get { return produto; }
        //    set { produto = value; }
        //}

        //private string empresa { get; set; }
        //public string EMPRESA
        //{
        //    get { return empresa; }
        //    set { empresa = value; }
        //}

        //private int? entrada { get; set; }
        //public int? ENTRADA
        //{
        //    get { return entrada; }
        //    set { entrada = value; }
        //}

        //private int? saida { get; set; }
        //public int? SAIDA
        //{
        //    get { return saida; }
        //    set { saida = value; }
        //}

        //private int estoque { get; set; }
        //public int ESTOQUE
        //{
        //    get { return estoque; }
        //    set { estoque = value; }
        //}

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