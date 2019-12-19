using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace McBonaldsMVC.Controllers
{
    public class AbstractController : Controller
    {
        protected const string SESSION_CLIENTE_EMAIL = "cliente_email";
        protected const string SESSION_CLIENTE_NOME = "cliente_nome";
        protected const string SESSION_TIPO_USUARIO = "tipo_usuario";

        //Metodos que auxiliam a obter informaçõe, para diminuir a escrita
        //ObterUsuarioSession() e ObterUsuarioSession()

        //HttpContext.Session GetString ou SetString
        //Deixa Visivel para todos os controllers


        //METOQUE QUE TEM FUNÇÃO DE:
        //Todos os dados do usuario seram apagados com um tempo limite
        protected string ObterUsuarioSession()
        {
            
            var email = HttpContext.Session.GetString(SESSION_CLIENTE_EMAIL);
            if (!string.IsNullOrEmpty(email))
            {
                return email;
            } 
            else
            {
                return "";
            }
        }
        protected string ObterUsuarioNomeSession()
        {
            var nome = HttpContext.Session.GetString(SESSION_CLIENTE_NOME);
            if (!string.IsNullOrEmpty(nome))
            {
                return nome;
            } 
            else
            {
                return "";
            }
        }

        protected string ObterUsuarioTipoSession()
        {
            var TipoUsuario = HttpContext.Session.GetString(SESSION_TIPO_USUARIO);
            if (!string.IsNullOrEmpty(TipoUsuario))
            {
                return TipoUsuario;
            } 
            else
            {
                return "";
            }
        }


        
    }
}