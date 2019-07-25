using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Models;
using System.Data.SqlClient;
using Dapper;

namespace Services.Validators
{
    public class ProdutoValidator
    {
        string connectionString = ConnectionDb.connectionString;

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

        public bool ProdutoExistente(int cdgProduto)
        {
            using(SqlConnection conexao = new SqlConnection(connectionString))
            {
                var verificaProduto = new Produto();

                string consulta = "SELECT * FROM Produtos WHERE cdgProduto = " + cdgProduto;

                verificaProduto = conexao.Query<Produto>(consulta).FirstOrDefault();

                return !verificaProduto.Equals(new Produto()) && verificaProduto != null;
            }
        }
    }
}
