using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace MinhaPrimeiraAPI
{
    public class SqlServer
    {
        private readonly SqlConnection _conexao;

        public SqlServer()
        {
            string stringConexao = File.ReadAllText(@"C:\Users\glcac\Documents\Curso RUMO\servidorSql.txt");
            _conexao = new SqlConnection(stringConexao);
        }

        public void InserirCliente(Entidades.Cliente cliente)
        {
            try
            {
                // abre a conexao
                _conexao.Open();

                // apenas a query de insert
                string query = @"INSERT INTO Cliente
                                       (Nome
                                       ,Cpf
                                       ,Idade
                                       ,Genero
                                       ,Nacionalidade)
                                 VALUES
                                       (@Nome
                                       ,@Cpf
                                       ,@Idade
                                       ,@Genero
                                       ,@Nacionalidade);";

                // recebe a conexao para preparar instrucoes a serem enviadas para a base
                using (var cmd = new SqlCommand(query, _conexao))
                {
                    // adiciona o parametro e valor mapeado na query acima
                    cmd.Parameters.AddWithValue("@Nacionalidade", cliente.Nacionalidade);
                    cmd.Parameters.AddWithValue("@Idade", cliente.Idade);
                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@Cpf", cliente.Cpf);
                    cmd.Parameters.AddWithValue("@Genero", cliente.Sexo);

                    // envia o insert para o banco de dados
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                // fecha a conexao
                _conexao.Close();
            }
        }
        public void AtualizarCliente(Entidades.Cliente cliente)
        {
            try
            {
                _conexao.Open();

                string query = @"UPDATE Cliente
                                   SET Nome = @Nome
                                      ,Genero = @Genero
                                      ,Nacionalidade = @Nacionalidade
                                      ,Idade = @Idade
                                 WHERE Cpf = @Cpf";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@Nacionalidade", cliente.Nacionalidade);
                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@Cpf", cliente.Cpf);
                    cmd.Parameters.AddWithValue("@Idade", cliente.Idade);
                    cmd.Parameters.AddWithValue("@Genero", cliente.Sexo);

                    cmd.ExecuteNonQuery();

                }
            }
            finally
            {
                _conexao.Close();
            }
        }
        public bool VerificarExistenciaCliente(string cpf)
        {
            try
            {
                _conexao.Open();

                string query = @"select Count(Cpf) AS total 
                                 from Cliente WHERE Cpf = @Cpf;";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@Cpf", cpf);

                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            finally
            {
                _conexao.Close();
            }
        }
        public void DeletarCliente(Entidades.Cliente cliente)
        {
            try
            {
                _conexao.Open();

                string query = @"DELETE FROM Cliente
                                 WHERE Cpf = @Cpf";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@Cpf", cliente.Cpf);
                    cmd.ExecuteNonQuery();

                }
            }
            finally
            {
                _conexao.Close();
            }
        }
        public Entidades.Cliente SelecionarCliente(string cpf)
        {
            try
            {
                _conexao.Open();

                string query = @"Select * FROM Cliente
                                 WHERE Cpf = @Cpf";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@Cpf", cpf);
                    var rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        var cliente = new Entidades.Cliente();
                        cliente.Cpf = cpf;
                        cliente.Nome = rdr["Nome"].ToString();
                        cliente.Nacionalidade = rdr["Nacionalidade"].ToString();
                        cliente.Idade = Convert.ToInt16(rdr["Idade"]);
                        cliente.Sexo = rdr["Genero"].ToString();

                        return cliente;
                    }
                    else
                    {
                        throw new InvalidOperationException("Cpf " + cpf + " não encontrado!");
                    }
                }
            }
            finally
            {
                _conexao.Close();
            }
        }
        public List<Entidades.Cliente> ListarClientes()
        {
            var clientes = new List<Entidades.Cliente>();
            try
            {
                _conexao.Open();

                string query = @"Select * FROM Cliente";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var cliente = new Entidades.Cliente();
                        cliente.Cpf = rdr["Cpf"].ToString();
                        cliente.Nome = rdr["Nome"].ToString();
                        cliente.Nacionalidade = rdr["Nacionalidade"].ToString();
                        cliente.Idade = Convert.ToInt16(rdr["Idade"]);
                        cliente.Sexo = rdr["Genero"].ToString();

                        clientes.Add(cliente);
                    }
                }
            }
            finally
            {
                _conexao.Close();
            }

            return clientes;
        }
    }
}
