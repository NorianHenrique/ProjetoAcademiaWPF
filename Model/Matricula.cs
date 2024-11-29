using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcademiaWPF.Model
{
    public class Matricula
    {
        public int Id { get; set; }
        public int IdAluno { get; set; }
        public int IdAtendente { get; set; }
        public EnumMatriculaPlano Plano { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public EnumMatriculaRestricaoMedica RestricaoMedica { get; set; }
        public string ObsRestricao { get; set; }
        public byte[] LaudoMedico { get; set; }
        public string Objetivo { get; set; }

        public Matricula()
        {
            DataInicio = DateTime.Now;
            DataFim = DateTime.Now; // Definindo DataFim como 1 mês a partir de DataInicio, por exemplo
            ObsRestricao = string.Empty; // Inicializa como string vazia
            Objetivo = string.Empty; // Inicializa como string vazia
        }
    }
}
