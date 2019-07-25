using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utils.Models;
using Services;

namespace AvaliacaoEverisAPI.Controllers
{
    public class ProdutosController : Controller
    {
        // GET: Produtos
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult InsereProduto()
        {
            try
            {
                var produto = new Produto()
                {
                    EMPRESA = "Teste",
                    ENTRADA = 1,
                    SAIDA = 1,
                    ESTOQUE = 10,
                    PRODUTO = "Cafe"
                };

                var service = new ProdutoService();



                var result = service.InsereProduto(produto);

                if (result == null)
                    return Json("Houve um erro ao criar seu objeto, por favor, verifique os campos");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}

