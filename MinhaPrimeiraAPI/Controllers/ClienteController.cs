using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MinhaPrimeiraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly SqlServer _sql;

        public ClienteController()
        {
            _sql = new SqlServer();
        }

        [HttpPost("v1/Cliente")]
        public void inserirCliente(Entidades.Cliente cliente)
        {
            _sql.InserirCliente(cliente);
        }

        [HttpPut("v1/Cliente")]
        public void AtualizarCliente(Entidades.Cliente cliente)
        {
            _sql.AtualizarCliente(cliente);
        }

        [HttpDelete("v1/Cliente")]
        public void DeletarCliente(Entidades.Cliente cliente)
        {
            _sql.DeletarCliente(cliente);
        }

        [HttpGet("v1/Cliente")]

        public List<Entidades.Cliente> ListarClientes()
        {
            return _sql.ListarClientes();
        }

        [HttpGet("v1/Cliente")]
        
            public Entidades.Cliente ListarClientes(string cpf)
        {
            return _sql.SelecionarCliente(cpf);
        }
        



    }
        
        


    
}
