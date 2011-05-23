using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CorreioNet.Engine;

namespace CorreioNet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                var eventos = PostOfficeManagerAgent.TrackAllEvents(cmbPais.Text, txtCodigoRastreio.Text);

                if (eventos == null || eventos.Count == 0)
                    throw new Exception("Sem informações");

                var resultado = eventos[0].Description;

                MessageBox.Show(resultado, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                var numb = new List<String>();
                numb.Add("RR994368428CN");
                numb.Add("RA086279580CN");
                numb.Add("RT085047213HK");
                numb.Add("RT088346945HK");
                numb.Add("RA078008727CN");
                numb.Add("RA078755640CN");
                numb.Add("RA078796278CN");

                var eventos = PostOfficeManagerAgent.TrackLastEvent("CN", "RA078796278CN");

                

                MessageBox.Show("Ok", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }
    }
}
