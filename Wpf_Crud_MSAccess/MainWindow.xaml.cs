using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;

namespace Wpf_Crud_MSAccess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con;
        DataTable dt;

        public MainWindow()
        {
            InitializeComponent();
            //conecta com o banco de dados
            con = new OleDbConnection();
            con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "Cadastro.accdb";
            VinculaDados();
        }

        //Exibe registros no grid
        private void VinculaDados()
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select * from Alunos";
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                dgDados.ItemsSource = dt.AsDataView();

                if (dt.Rows.Count > 0)
                {
                    lblContador.Visibility = System.Windows.Visibility.Hidden;
                    dgDados.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    lblContador.Visibility = System.Windows.Visibility.Visible;
                    dgDados.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();

            if (con.State != ConnectionState.Open)
                con.Open();

            cmd.Connection = con;

            if(string.IsNullOrWhiteSpace(txtNomeAluno.Text))
            {
                MessageBox.Show("Informe o nome do cliente....");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContato.Text))
            {
                MessageBox.Show("Informe o contato do cliente....");
                return;
            }

            if (txtCodigoAluno.IsEnabled == true)
           {
                    if (ddlGenero.Text != "Selecione o Sexo")
                    {
                        cmd.CommandText = "insert into Alunos(Nome,Sexo,Contato,Endereco) Values('" + txtNomeAluno.Text + "','" + ddlGenero.Text + "','" + txtContato.Text + "','" + txtEndereco.Text + "')";
                        cmd.ExecuteNonQuery();
                        VinculaDados();
                        MessageBox.Show("Cliente adicionado com sucesso...");
                        LimparTudo();

                    }
                    else
                    {
                        MessageBox.Show("Selecione o sexo do cliente....");
                    }
                }
                else
                {
                    cmd.CommandText = "update Cliente set Nome='" + txtNomeAluno.Text + "',Sexo='" + ddlGenero.Text + "',Contato='" + txtContato.Text + "',Endereco='" + txtEndereco.Text + "' where ClienteId=" + txtCodigoAluno.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Detalhes do Cliente atualizado com sucesso...");
                    VinculaDados();
                }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimparTudo();
        }

        private void LimparTudo()
        {
            txtCodigoAluno.Text = "";
            txtNomeAluno.Text = "";
            ddlGenero.SelectedIndex = 0;
            txtContato.Text = "";
            txtEndereco.Text = "";
            btnAdicionar.Content = "Adicionar";
            txtCodigoAluno.IsEnabled = true;
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (dgDados.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)dgDados.SelectedItems[0];
                txtCodigoAluno.Text = row["AlunoId"].ToString();
                txtNomeAluno.Text = row["Nome"].ToString();
                ddlGenero.Text = row["Sexo"].ToString();
                txtContato.Text = row["Contato"].ToString();
                txtEndereco.Text = row["Endereco"].ToString();
                txtCodigoAluno.IsEnabled = false;
                btnAdicionar.Content = "Atualizar";
            }
            else
            {
                MessageBox.Show("Selecione um Cliente da lista...");
            }
        }

        private void btnDeletar_Click(object sender, RoutedEventArgs e)
        {

            if (dgDados.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Confirma exclusão deste registro ?", "Deletar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DataRowView row = (DataRowView)dgDados.SelectedItems[0];

                    OleDbCommand cmd = new OleDbCommand();
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "delete from Alunos where AlunoID=" + row["AlunoID"].ToString();
                    cmd.ExecuteNonQuery();
                    VinculaDados();
                    MessageBox.Show("Cliente deletado com sucesso...");
                    LimparTudo();
                }
            }
            else
            {
                MessageBox.Show("Selecione um Cliente da lista...");
            }
        }

        private void btnSair_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Deseja Encerrar a Aplicação ?", "Encerrar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
