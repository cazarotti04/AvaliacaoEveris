using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Models
{
    public class Produto
    {
        [Key]
        private int? cdgProduto { get; set; }
        public int? CDGPRODUTO
        {
            get { return cdgProduto; }
            set { cdgProduto = value; }
        }

        private string produto { get; set; }
        public string PRODUTO
        {
            get { return produto; }
            set { produto = value; }
        }

        private string empresa { get; set; }
        public string EMPRESA
        {
            get { return empresa; }
            set { empresa = value; }
        }

        private int? entrada { get; set; }
        public int? ENTRADA
        {
            get { return entrada; }
            set { entrada = value; }
        }

        private int? saida { get; set; }
        public int? SAIDA
        {
            get { return saida; }
            set { saida = value; }
        }

        private int estoque { get; set; }
        public int ESTOQUE
        {
            get { return estoque; }
            set { estoque = value; }
        }

        public Produto()
        {

        }
    }
}
