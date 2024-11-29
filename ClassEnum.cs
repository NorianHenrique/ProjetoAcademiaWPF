using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcademiaWPF
{
    public enum EnumColaboradorTipo
    {
        [Description("Administrador")]
        Administrador = 1,
        [Description("Atendente")]
        Atendente = 2,
        [Description("Instrutor")]
        Instrutor = 3,
    }

    public enum EnumColaboradorVinculo
    {
        [Description("CLT")]
        Clt = 1,
        [Description("Estágio")]
        Estágio = 2,
    }

    public enum EnumMatriculaPlano
    {
        [Description("Mensal")]
        Mensal = 'm',

        [Description("Trimestral")]
        Trimestral = 't',

        [Description("Semestral")]
        Semestral = 's',

        [Description("Anual")]
        Anual = 'a'
    }

    public enum EnumMatriculaRestricaoMedica
    {
        [Description("Nenhuma")]
        Nenhuma = 'N',

        [Description("Problemas Cardíacos")]
        ProblemasCardiacos = 'C',

        [Description("Problemas Respiratórios")]
        ProblemasRespiratorios = 'R',

        [Description("Lesões Musculares")]
        LesoesMusculares = 'L',

        [Description("Pressão Alta")]
        PressaoAlta = 'P',

        [Description("Diabetes")]
        Diabetes = 'D',

        [Description("Gravidez")]
        Gravidez = 'G',

        [Description("Outras")]
        Outras = 'O'
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }
    }
}
