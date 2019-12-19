using McBonaldsMVC.Enums;
using McBonaldsMVC.Repositories;
using McBonaldsMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace McBonaldsMVC.Controllers {
    //HERDAR DE ABSTRACTCONTROLLER QUE ANTES ERA CONTROLLER
    public class AdministradorController : AbstractController {
        //Pega do banco de dados e joga na tela
        PedidoRepository pedidoRepository = new PedidoRepository ();

        //Pra marcar as requisições como GET - ATRIBUTO
        [HttpGet]
        public IActionResult Dashboard () {

            //TRATAR DAS COISAS QUE ESTÃO LOGADAS 
            //LOGAR COMO ADMINISTRADOR
            var tipoUsuarioSessao = uint.Parse (ObterUsuarioTipoSession ());
            if (tipoUsuarioSessao.Equals ((uint) TipoUsuario.ADMINISTRADOR)) {
                var pedidos = pedidoRepository.ObterTodos ();
                //COPIAR McBonaldsMVC.ViewModels.DashBoardViewModel do DASHBOARDVIEWMODEL
                DashboardViewModel dashboardViewModel = new DashboardViewModel ();

                foreach (var pedido in pedidos) {
                    switch (pedido.Status) {
                        case (uint) StatusPedido.APROVADO:
                            dashboardViewModel.PedidosAprovados++;
                            break;
                        case (uint) StatusPedido.REPROVADO:
                            dashboardViewModel.PedidosReprovados++;
                            break;
                        default:
                            dashboardViewModel.PedidosPendentes++;
                            dashboardViewModel.Pedidos.Add (pedido);
                            break;
                    }
                }
                dashboardViewModel.NomeView = "Dashboard";
                dashboardViewModel.UsuarioEmail = ObterUsuarioSession ();

                return View (dashboardViewModel);

            }
            return View ("Erro", new RespostaViewModel () {
                NomeView = "Dashboard",
                    Mensagem = "Você não pode acessar essa parte do site"
            });
        }
    }
}