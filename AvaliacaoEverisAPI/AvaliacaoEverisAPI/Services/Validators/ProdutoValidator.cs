using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using AvaliacaoEverisAPI.Models;

namespace AvaliacaoEverisAPI.Services.Validators
{
    public class ProdutoValidator
    {
        //@brief Busca a connection string definida nos paraetros do projeto
        string connectionString = ConnectionDb.connectionString;

        /**
        *   @brief Método para validar as informações de um produto
        *
        *   @param produto Objeto enviado que representa o produto a ser validado
        *
        *   @return true caso o produto seja valido, false caso contrario
        */
        public bool ValidaProdutoNovo(Produto produto)
        {
            if (produto.EMPRESA == "" || produto.EMPRESA == null)
                return false;
            if (produto.ENTRADA < 0 || produto.SAIDA < 0 || produto.ESTOQUE < 0)
                return false;
            if (produto.NOMEPRODUTO == "" || produto.NOMEPRODUTO == null)
                return false;

            return true;
        }

        /**
        *   @brief Método para validar se o produto com X código existe
        *
        *   @param cdgProduto Número inteiro que representa o código do produto a ser verificado
        *
        *   @return true caso o produto com tal código exista, false caso contrário
        */
        public bool ProdutoExistente(int cdgProduto)
        {
            using(SqlConnection conexao = new SqlConnection(connectionString))
            {
                var verificaProduto = new Produto();
                var instancia = new Produto();

                string consulta = "SELECT * FROM Produtos WHERE cdgProduto = " + cdgProduto;

                verificaProduto = conexao.Query<Produto>(consulta).FirstOrDefault();


                if (verificaProduto == null)
                    return false;
                else
                {
                    if (verificaProduto.Equals(instancia))
                        return false;

                    return true;
                }

                
                
            }
        }
    }
}
