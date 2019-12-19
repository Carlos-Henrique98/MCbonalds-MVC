using System;
using McBonaldsMVC.Repositories;
using McBonaldsMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using McBonaldsMVC.Enums;

namespace McBonaldsMVC.Controllers
{
    //LOGOFF - duas opções, clicando no botão e usando timeout

    //Trocou a herança de Controller para AbstractController
    public class ClienteController : AbstractController
    {

        private ClienteRepository clienteRepository = new ClienteRepository();
        private PedidoRepository pedidoRepository = new PedidoRepository();

        [HttpGet]
        public IActionResult Login()
        {
            return View(new BaseViewModel()
            {
                NomeView = "Login",
                UsuarioEmail = ObterUsuarioSession(),
                UsuarioNome = ObterUsuarioNomeSession()
            });
        }

        [HttpPost]
        public IActionResult Login(IFormCollection form)
        {
            ViewData["Action"] = "Login";
            try
            {
                System.Console.WriteLine("==================");
                System.Console.WriteLine(form["email"]);
                System.Console.WriteLine(form["senha"]);
                System.Console.WriteLine("==================");

                var usuario = form["email"];
                var senha = form["senha"];

                //este metodo verifica se o EMAIL existe ou não
                var cliente = clienteRepository.ObterPor(usuario);

                if(cliente != null)
                {
                    //Verifica se a senha e o email são iguais à escolha do usuario
                    //EXCLUIDO - if(cliente.Email.Equals(usuario) && cliente.Senha.Equals(senha))
                    if(cliente.Senha.Equals(senha))
                    {
                        switch(cliente.TipoUsuario)
                        {
                            case (uint) TipoUsuario.CLIENTE:
                            //Deixa visivel para todos os controllers as informações
                            //Exemplo: email, nome
                            // HttpContext.Session.SetString - permite que os dados seja compartilhado entre controllers
                            HttpContext.Session.SetString(SESSION_CLIENTE_EMAIL, usuario);
                            HttpContext.Session.SetString(SESSION_CLIENTE_NOME, cliente.Nome.ToString());
                            return RedirectToAction("Historico","Cliente");

                            default:
                            HttpContext.Session.SetString(SESSION_CLIENTE_EMAIL, usuario);
                            HttpContext.Session.SetString(SESSION_CLIENTE_NOME, cliente.Nome);
                            HttpContext.Session.SetString(SESSION_TIPO_USUARIO, cliente.TipoUsuario.ToString());
                            
                            //RedirectToAction - passa a ação do metodo para outro
                            //ANTES ERA:
                            // return View ("Historico", "Cliente");
                            //DEPOIS:
                            //Redirecionar para a ação = chama um metodo (Historico)
                            return RedirectToAction("Dashboard","Administrador");
                        }
                    }
                    else 
                    {
                        //VIEW - Retorna uma tela

                        //DUAS OPÇÕES QEU FAZEM A MESMA COISA 1 E 2:

                        //1 - RespostaViewModel rvm = new RespostaViewModel();
                        //1 - return View("Erro", rvm);

                        //2 - Criando o objeto sem armazena-lo no cliente controller
                        //2 - Tudo que está dentro de "" vai para o parametro string mensagem, do construtor RespostaViewModel
                        return View("Erro", new RespostaViewModel("Senha incorreta"));
                    }

                } 
                else
                {
                    return View("Erro", new RespostaViewModel($"Usuário {usuario} não encontrado"));
                }

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.StackTrace);
                return View("Erro");
            }
        }
    
        public IActionResult Historico ()
        {
            //envia o nome para o CLiente
            var emailCliente = ObterUsuarioSession();
            //Metodo pedidoRepository = lista todos os pedidos
            var pedidosCliente = pedidoRepository.ObterTodosPorCliente(emailCliente);

            return View(new HistoricoViewModel()
            {
                Pedidos = pedidosCliente,
                NomeView = "Histórico",
                UsuarioEmail = ObterUsuarioSession(),
                UsuarioNome = ObterUsuarioNomeSession()
            });
        }

        //Metodo de LOGOFF
        public IActionResult Logoff()
        {

            //Tira os dados da seção e depois limpa os dados
            //Depois redireciona para o Index, Home
            HttpContext.Session.Remove(SESSION_CLIENTE_EMAIL);
            HttpContext.Session.Remove(SESSION_CLIENTE_NOME);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}