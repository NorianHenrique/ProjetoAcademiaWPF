using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcademiaWPF.Model
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Nome { get; set; }
        public DateTime Nascimento { get; set; }
        public string Email { get; set; }
        public int LogradouroId { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Senha { get; set; }
        public byte[] Foto { get; set; }

        public Aluno()
        {
            Nascimento = DateTime.Now;
        }
    }
}
