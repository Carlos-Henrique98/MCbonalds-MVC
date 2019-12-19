using System;
using McBonaldsMVC.Enums;
using McBonaldsMVC.Models;
using McBonaldsMVC.Repositories;
using McBonaldsMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace McBonaldsMVC.Controllers {
    public class PedidoController : AbstractController {
        PedidoRepository pedidoRepository = new PedidoRepository ();
        HamburguerRepository hamburguerRepository = new HamburguerRepository();
        ShakesRepository shakeRepository = new ShakesRepository();
        ClienteRepository clienteRepository = new ClienteRepository();


        //Só retorna um ViewModel o metodo Index()
        public IActionResult Index () {

            PedidoViewModel pvm = new PedidoViewModel();
            pvm.Hamburgueres = hamburguerRepository.ObterTodos();
            pvm.Shakes = shakeRepository.ObterTodos();


            //Escrever o nome da pessoa que acessa o pedido
            //var usuarioLogado = HttpContext.Session.GetString();
            var usuarioLogado = ObterUsuarioSession();
            var nomeUsuarioLogado = ObterUsuarioNomeSession();
            if (!string.IsNullOrEmpty(nomeUsuarioLogado))
            {
                pvm.NomeUsuario = nomeUsuarioLogado;
            }
            
            //Guarda o objeto cliente - armazena os dados da tela de pedidos
            var clienteLogado = clienteRepository.ObterPor(usuarioLogado);
            if (clienteLogado != null)
            {
                pvm.Cliente = clienteLogado;
            }

            pvm.NomeView = "Pedido";
            pvm.UsuarioEmail = usuarioLogado;
            pvm.UsuarioNome = nomeUsuarioLogado;



            return View (pvm);
        }

        public IActionResult Registrar (IFormCollection form) {
            ViewData["Action"] = "Pedido";
            Pedido pedido = new Pedido ();

            //OUTRO JEITO DE SE FAZER:
            // var nomeShake = form["shake"];
            //shake.Preco = shakeRepository.ObterPreco(nomeShake);
            //shake.Preco = precoShake;
            //shake.Nome = form["shake"];
            //shake.Preco = shakeRepository.ObterPreco(form["shake"]);
            var nomeShake = form["shake"];
            Shake shake = new Shake ();
            shake.Nome = nomeShake;
            shake.Preco = shakeRepository.ObterPrecoDe(nomeShake);

            pedido.Shake = shake;

            //OUTRO JEITO DE SE FAZER:
            // var nomeHamburguer = form["hamburguer"];
            //Hamburguer.Nome = nomeHamburguer;
            //Hamburguer.Nome = HamburguerRepository.ObterPreco(nomeHamburguer); 

            var nomeHamburguer = form["hamburguer"];
            Hamburguer hamburguer = new Hamburguer (
                nomeHamburguer, 
                hamburguerRepository.ObterPrecoDe(nomeHamburguer));

            pedido.Hamburguer = hamburguer;

            Cliente cliente = new Cliente () {
                Nome = form["nome"],
                Endereco = form["endereco"],
                Telefone = form["telefone"],
                Email = form["email"]
            };

            pedido.Cliente = cliente;

            pedido.DataDoPedido = DateTime.Now;

            pedido.PrecoTotal = hamburguer.Preco + shake.Preco;

            if (pedidoRepository.Inserir (pedido)) {
                return View ("Sucesso", new RespostaViewModel()
                {
                    NomeView = "Pedido",
                    UsuarioEmail = ObterUsuarioSession(),
                    UsuarioNome = ObterUsuarioNomeSession()
                    
                });
            } else {
                return View ("Erro", new RespostaViewModel()
                {
                    NomeView = "Pedido",
                    UsuarioEmail = ObterUsuarioSession(),
                    UsuarioNome = ObterUsuarioNomeSession()
                    
                });
            }
        }
    
        public IActionResult Aprovar(ulong id)
        {
            Pedido pedido = pedidoRepository.ObterPor(id);
            pedido.Status = (uint) StatusPedido.APROVADO;

            //ATUALIZAR PEDIDOS É NO PEDIDOREPOSITORY
            //Aparece a tela do DASHBOARD com os dados atualizados
            if(pedidoRepository.Atualizar(pedido))
            {
                return RedirectToAction("Dashboard", "Administrador");
            }
            else {
                return View("Erro", new RespostaViewModel()
                {
                    Mensagem = "Houve um erro ao Aprovar pedido.",
                    NomeView = "Dashboard",
                    UsuarioEmail = ObterUsuarioSession(),
                    UsuarioNome = ObterUsuarioNomeSession()
                    
                });
            }
        }

        //TELA DashBoard.cshtml id = @pedido.Id
        public IActionResult Reprovar(ulong id)
        {
            Pedido pedido = pedidoRepository.ObterPor(id);
            pedido.Status = (uint) StatusPedido.REPROVADO;
            //ATUALIZAR PEDIDOS É NO PEDIDOREPOSITORY
            //Aparece a tela do DASHBOARD com os dados atualizados
            if(pedidoRepository.Atualizar(pedido))
            {
                return RedirectToAction("Dashboard", "Administrador");
            }
            else {
                return View("Erro", new RespostaViewModel()
                {
                    Mensagem = "Houve um erro ao Reprovar pedido.",
                    NomeView = "Dashboard",
                    UsuarioEmail = ObterUsuarioSession(),
                    UsuarioNome = ObterUsuarioNomeSession()
                    
                });
            }
        }
    }    
}