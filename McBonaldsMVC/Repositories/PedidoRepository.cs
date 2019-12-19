using System;
using System.Collections.Generic;
using System.IO;
using McBonaldsMVC.Models;

namespace McBonaldsMVC.Repositories {
    public class PedidoRepository : RepositoryBase {
        private const string PATH = "Database/Pedido.csv";
        public PedidoRepository () {
            if (!File.Exists (PATH)) {
                File.Create (PATH).Close ();
            }

        }
        public bool Inserir (Pedido pedido) {
            var quantidadePedidos = File.ReadAllLines(PATH).Length;
            pedido.Id = (ulong) ++quantidadePedidos;
            var linha = new string[] { PrepararPedidoCSV (pedido) };
            File.AppendAllLines (PATH, linha);

            return true;
        }

        //Verificar se o EMAIL é igual para verificação 
        public List<Pedido> ObterTodosPorCliente(string emailCliente)
        {
            var pedidos = ObterTodos();
            List<Pedido> pedidosCliente = new List<Pedido>();

            foreach (var pedido in pedidos)
            {
                if(pedido.Cliente.Email.Equals(emailCliente))
                {
                    pedidosCliente.Add(pedido);
                }
            }
            return pedidosCliente;
        }

        //metodo que manda o codigo para o repositorio para mandar para o banco
        //ObterPor - Consultar o banco de dados - verifica se os ids batem
        //e devolve 
        public Pedido ObterPor(ulong id)
            {
                var pedidosTotais = ObterTodos();
                foreach (var pedido in pedidosTotais)
                {
                    if(pedido.Id == id)
                    {
                        return pedido;
                    }
                }
                return null;
            }

        public List<Pedido> ObterTodos () {
            //ReadAllLines = Abre um arquivo de texto, lê todas as linhas do arquivo em uma matriz de cadeia de caracteres e o fecha
            var linhas = File.ReadAllLines (PATH);
            List<Pedido> pedidos = new List<Pedido>();

            foreach (var linha in linhas) {
                Pedido pedido = new Pedido ();

                pedido.Id = ulong.Parse(ExtrairValorDoCampo("id", linha));
                pedido.Status = uint.Parse(ExtrairValorDoCampo("status_pedido", linha));
                pedido.Cliente.Nome = ExtrairValorDoCampo("cliente_nome", linha);
                pedido.Cliente.Endereco = ExtrairValorDoCampo("cliente_endereco",linha);
                pedido.Cliente.Telefone = ExtrairValorDoCampo("cliente_telefone", linha);
                pedido.Cliente.Email = ExtrairValorDoCampo("cliente_email", linha);
                pedido.Hamburguer.Nome = ExtrairValorDoCampo("hamburguer_nome", linha);
                pedido.Hamburguer.Preco = double.Parse(ExtrairValorDoCampo("hamburguer_preco", linha));
                pedido.Shake.Nome = ExtrairValorDoCampo("shake_nome", linha);
                pedido.Shake.Preco = double.Parse(ExtrairValorDoCampo("shake_preco", linha));
                pedido.PrecoTotal = double.Parse(ExtrairValorDoCampo("preco_total", linha));
                pedido.DataDoPedido = DateTime.Parse(ExtrairValorDoCampo("data_pedido", linha));

                pedidos.Add(pedido);
            }
            return pedidos;
        }

        //METODO PARA ATUALIZAR PEDIDOS DE COMIDA
        public bool Atualizar(Pedido pedido)
        {
            //Ler a base - primeira coisa - recolher tudo que tem na tela de pedidos
            var pedidosTotais = File.ReadAllLines(PATH);
            //Transformando o pedido em string para poder ser gravado
            var pedidoCSV = PrepararPedidoCSV(pedido);
            //Linha que vai guardar todos os pedidos que foram feitos
            var linhaPedido = -1;
            var resultado = false;

            //Vetor = Length
            //Lista = Count
            for (int i = 0; i < pedidosTotais.Length; i++)
            {
                //ATUALIZAR O STATUS - SUBSTITUIR LINHA INTEIRA
                //pedido.ID = linha que a pessoa escreveu
                var idConvertido = ulong.Parse(ExtrairValorDoCampo("id",pedidosTotais[i]));
                if (pedido.Id.Equals(idConvertido))
                {
                    linhaPedido = i;
                    resultado = true;
                    break;
                }
            }

            //IF para quebrar um for
            //pedidoCSV - Informação nova
            if(resultado){
                pedidosTotais[linhaPedido] = pedidoCSV;
                File.WriteAllLines(PATH, pedidosTotais);
            }

            return resultado;
        }

        private string PrepararPedidoCSV (Pedido pedido) {
            Cliente c = pedido.Cliente;
            Hamburguer h = pedido.Hamburguer;
            Shake s = pedido.Shake;

            return $"id={pedido.Id};status_pedido={pedido.Status};cliente_nome={c.Nome};cliente_endereco={c.Endereco};cliente_telefone={c.Telefone};cliente_email={c.Email};hamburguer_nome={h.Nome};hamburguer_preco={h.Preco};shake_nome={s.Nome};shake_preco={s.Preco};data_pedido={pedido.DataDoPedido};preco_total={pedido.PrecoTotal}";
        }
    }
}