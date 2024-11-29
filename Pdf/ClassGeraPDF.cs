using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ProjetoAcademiaWPF.Model;
using iText.Kernel.Colors;

namespace ProjetoAcademiaWPF.Pdf
{
    public class ClassGeraPDF
    {
        public static string PathArquivo(string nome)
        {
            SaveFileDialog savePath = new()
            {
                Title = "Selecione o local e o nome para salvar seu relatório",
                Filter = "Arquivo|*.pdf",
                FileName = nome + "-" + Convert.ToString(DateTime.Now).Replace("/", "-").Replace(":", "-") + ".pdf"
            };
            return (savePath.ShowDialog() == true) ? Convert.ToString(savePath.FileName) : "ProjetoAcademia.pdf";
        }

        public static void AbrePdf(string local)
        {
            _ = new Process { StartInfo = new ProcessStartInfo(local) { UseShellExecute = true } }.Start();
        }

        public static void AlunosPdf(ObservableCollection<Aluno> Alunos)
        {
            // escolhe o local e o nome do arquivo
            string local = PathArquivo("Alunos");
            using Document document = new(new PdfDocument(new PdfWriter(local)), PageSize.A4.Rotate());
            document.Add(new Paragraph("Projeto Academia").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20));
            document.Add(new Paragraph("Alunos").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15));
            document.Add(new LineSeparator(new SolidLine()));
            Table table = new(6, false);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetTextAlignment(TextAlignment.LEFT);
            table.AddCell(new Cell().Add(new Paragraph("ID")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Nome")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("CPF")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Telefone")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Email")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Foto")).SetBorder(Border.NO_BORDER));
            foreach (var aluno in Alunos)
            {
                table.AddCell(new Cell().Add(new Paragraph(aluno.Id.ToString())).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(aluno.Nome)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(aluno.Cpf)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(aluno.Telefone)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(aluno.Email)).SetBorder(Border.NO_BORDER));
                if (aluno.Foto != null && aluno.Foto.Length > 0)
                {
                    var img = new Image(ImageDataFactory.Create((byte[])aluno.Foto));
                    img.SetAutoScale(true);
                    img.ScaleToFit(50f, 50f);
                    table.AddCell(new Cell().Add(img).SetBorder(Border.NO_BORDER));
                }
                else
                {
                    table.AddCell("Sem Foto");
                }
            }
            document.Add(table);
            //abre o pdf gerado
            AbrePdf(local);
        }

        public static void MatriculaPdf(ObservableCollection<Matricula> Matriculas)
        {
            string local = PathArquivo("Matriculas");
            using Document document = new(new PdfDocument(new PdfWriter(local)), PageSize.A4.Rotate());

            document.Add(new Paragraph("Projeto Academia").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20));
            document.Add(new Paragraph("Matrículas").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15));
            document.Add(new LineSeparator(new SolidLine()));

            // Verifica se a coleção está vazia
            if (Matriculas == null || Matriculas.Count == 0)
            {
                document.Add(new Paragraph("Nenhuma matrícula cadastrada.").SetTextAlignment(TextAlignment.CENTER).SetFontSize(12));
                return; // Encerra o método, não há dados para exibir
            }

            Table table = new(10, false);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetTextAlignment(TextAlignment.LEFT);

            // Cabeçalho da tabela
            table.AddCell(new Cell().Add(new Paragraph("ID")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("ID Aluno")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("ID Atendente")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Plano")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Data Início")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Data Final")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Restrição Médica")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Observação Restrição")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Laudo Médico")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Objetivo")).SetBorder(Border.NO_BORDER));

            // Preencher a tabela com dados das matrículas
            foreach (var matricula in Matriculas)
            {
                table.AddCell(new Cell().Add(new Paragraph(matricula.Id.ToString())).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.IdAluno.ToString())).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.IdAtendente.ToString())).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.Plano.ToString())).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.DataInicio.ToString("dd/MM/yyyy"))).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.DataFim.ToString("dd/MM/yyyy"))).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.RestricaoMedica.ToString())).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.ObsRestricao ?? "Sem Observação")).SetBorder(Border.NO_BORDER));

                if (matricula.LaudoMedico != null && matricula.LaudoMedico.Length > 0)
                {
                    var img = new Image(ImageDataFactory.Create((byte[])matricula.LaudoMedico));
                    img.SetAutoScale(true);
                    img.ScaleToFit(50f, 50f);
                    table.AddCell(new Cell().Add(img).SetBorder(Border.NO_BORDER));
                }
                else
                {
                    table.AddCell(new Cell().Add(new Paragraph("Sem Laudo")).SetBorder(Border.NO_BORDER));
                }

                table.AddCell(new Cell().Add(new Paragraph(matricula.Objetivo)).SetBorder(Border.NO_BORDER));
            }

            // Adiciona a tabela ao documento
            document.Add(table);
        }


        public static void LogradourosPdf(ObservableCollection<Logradouro> Logradouros)
        {
            // escolhe o local e o nome do arquivo
            string local = PathArquivo("Logradouros");
            using Document document = new(new PdfDocument(new PdfWriter(local)), PageSize.A4.Rotate());
            document.Add(new Paragraph("Projeto Academia").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20));
            document.Add(new Paragraph("Logradouros").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15));
            document.Add(new LineSeparator(new SolidLine()));
            Table table = new(7, false);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetTextAlignment(TextAlignment.LEFT);
            table.AddCell(new Cell().Add(new Paragraph("ID")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("CEP")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Nome")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Bairro")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Cidade")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("UF")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Pais")).SetBorder(Border.NO_BORDER));
            foreach (var logradouro in Logradouros)
            {
                table.AddCell(new Cell().Add(new Paragraph(logradouro.Id.ToString())).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(logradouro.Cep)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(logradouro.Nome)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(logradouro.Bairro)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(logradouro.Cidade)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(logradouro.Uf)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(logradouro.Pais)).SetBorder(Border.NO_BORDER));

            }
            document.Add(table);
            //abre o pdf gerado
            AbrePdf(local);
        }

        public static void ColaboradoresPdf(ObservableCollection<Colaborador> colaboradores)
        {
            string local = PathArquivo("Colaboradores");
            using Document document = new(new PdfDocument(new PdfWriter(local)), PageSize.A4.Rotate());
            document.Add(new Paragraph("Projeto Academia").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20));
            document.Add(new Paragraph("Colaboradores").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15));
            document.Add(new LineSeparator(new SolidLine()));

            // Criar a tabela com larguras de coluna ajustadas
            Table table = new Table(new float[] { 1, 1, 3, 2, 3, 1, 1, 2, 2, 2, 1, 1 });

            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetMarginTop(10);
            table.SetMarginBottom(10);
            table.SetTextAlignment(TextAlignment.LEFT);

            // Adicionando cabeçalho
            string[] headers = { "ID", "CPF", "Nome", "Nascimento", "Email", "Logradouro ID", "Número", "Complemento", "Senha", "Admissão", "Tipo", "Vínculo" };
            foreach (var header in headers)
            {
                table.AddCell(new Cell().Add(new Paragraph(header).SetBackgroundColor(ColorConstants.LIGHT_GRAY)).SetBorder(Border.NO_BORDER));
            }

            // Preencher a tabela com os dados dos colaboradores
            foreach (var colaborador in colaboradores)
            {
                table.AddCell(new Cell().Add(new Paragraph(colaborador.Id.ToString()).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(colaborador.Cpf).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(colaborador.Nome).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(colaborador.Nascimento.ToString("dd/MM/yyyy")).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(colaborador.Email).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(colaborador.LogradouroId.ToString()).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(colaborador.Numero).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(colaborador.Complemento).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(colaborador.Senha).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(colaborador.Admissao.ToString("dd/MM/yyyy")).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(colaborador.Tipo.ToString()).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(colaborador.Vinculo.ToString()).SetFontSize(12)).SetBorder(Border.NO_BORDER));
            }

            // Adiciona a tabela ao documento
            document.Add(table);

            // Abre o PDF gerado
            AbrePdf(local);
        }

    }
}
