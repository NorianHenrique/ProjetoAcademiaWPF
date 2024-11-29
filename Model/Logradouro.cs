using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcademiaWPF.Model
{
    public class Logradouro: ICloneable
    {
        public int  Id { get; set; }
        public string Cep { get; set; }
        public string Nome { get; set; }

        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Pais { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }


    }
}
