using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AcessoBancoDados;
using System.Net.Mail;

namespace TesteSql_IP
{
    public partial class formDefault : Form
    {
        public formDefault()
        {
            InitializeComponent();
        }

        private AcessoBancoDados.AcessoDadosSqlServer acessoDadosSqlServer = new AcessoDadosSqlServer();
        private ObjetoCliente objetoCliente = new ObjetoCliente();

        private void formDefault_Load(object sender, EventArgs e)
        {
            CarregarLista();
        }

        private void CarregarLista()
        {
            try
            {
                DataTable dataTable = new DataTable();

                acessoDadosSqlServer.LimparParametros();
                dataTable = acessoDadosSqlServer.ExecutarConsulta(CommandType.StoredProcedure, "prcClienteConsultar");

                if(dataTable.Rows.Count > 0)
                {
                    listView_Lista.Items.Clear();

                    foreach(DataRow rows in dataTable.Rows)
                    {
                        ListViewItem lvi = new ListViewItem(rows["IDCliente"].ToString());
                        lvi.SubItems.Add(rows["Nome"].ToString());
                        lvi.SubItems.Add(rows["Email"].ToString());

                        listView_Lista.Items.Add(lvi);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool ValidarEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool ValidarDados(ObjetoCliente objetoCliente)
        {
            bool error = true;

            if(objetoCliente.Nome == "")
            {
                MessageBox.Show("O campo Nome é obrigatório", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                error = false;
            }

            if (ValidarEmail(objetoCliente.Email) == false)
            {
                MessageBox.Show("Email Invalido", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = false;
            }

            return error;
        }

        private void LimparCampos()
        {
            campNome.Clear();
            campEmail.Clear();

            campNome.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            objetoCliente.Nome = campNome.Text;
            objetoCliente.Email = campEmail.Text;

            if(ValidarDados(objetoCliente))
            {
                try
                {
                    acessoDadosSqlServer.LimparParametros();
                    acessoDadosSqlServer.AdcionarParametro("@Nome", objetoCliente.Nome);
                    acessoDadosSqlServer.AdcionarParametro("@Email", objetoCliente.Email);
                    acessoDadosSqlServer.ExecutarManipulacao(CommandType.StoredProcedure, "prcClienteAdcionar");

                    MessageBox.Show("Cliente cadastrado com sucesso.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarLista();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
