using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Models;

namespace Services.Validators
{
    public class ProdutoValidator
    {
        public bool ValidaProdutoNovo(Produto produto)
        {
            if (produto.EMPRESA == "" || produto.EMPRESA == null)
                return false;
            if (produto.ENTRADA < 0 || produto.SAIDA < 0 || produto.ESTOQUE < 0)
                return false;
            if (produto.PRODUTO == "" || produto.PRODUTO == null)
                return false;

            return true;
        }
    }
}
