using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EinfachesCRUD
  {
  public partial class Form1: Form
    {
    // Verbindung zur Datenbank herstellen (angepasster Verbindungsstring)
    private string connectionString = @"Server=DESKTOP-O9BMBCB\SQLEXPRESS;Database=Erstes-CRUD;Integrated Security=True;";

    public Form1()
      {
      InitializeComponent();
      }

    private void TestDatabaseConnection()
      {
      using ( SqlConnection con = new SqlConnection( connectionString ) )
        {
        try
          {
          con.Open();
          MessageBox.Show( "Verbindung zur Datenbank erfolgreich!" );
          }
        catch ( Exception ex )
          {
          MessageBox.Show( "Fehler bei der Verbindung zur Datenbank: " + ex.Message );
          }
        }
      }

    // Diese Methode kannst du dann zum Beispiel im Load-Event der Form aufrufen
    private void Form1_Load( object sender, EventArgs e )
      {
      TestDatabaseConnection();
      }

    // CREATE Operation (Datensatz hinzufügen)
    private void btnAdd_Click( object sender, EventArgs e )
      {
      using ( SqlConnection con = new SqlConnection( connectionString ) )
        {
        con.Open();
        string query = "INSERT INTO Students (Name) VALUES (@Name)";
        using ( SqlCommand cmd = new SqlCommand( query, con ) )
          {
          cmd.Parameters.AddWithValue( "@Name", txtName.Text );
          cmd.ExecuteNonQuery();
          MessageBox.Show( "Student hinzugefügt" );
          LoadData();
          }
        }
      }

    // READ Operation (Daten anzeigen)
    private void btnRead_Click( object sender, EventArgs e )
      {
      LoadData();
      }

    // Hilfsfunktion zum Laden der Daten in das DataGridView
    private void LoadData()
      {
      using ( SqlConnection con = new SqlConnection( connectionString ) )
        {
        con.Open();
        string query = "SELECT * FROM Students";
        using ( SqlDataAdapter da = new SqlDataAdapter( query, con ) )
          {
          DataTable dt = new DataTable();
          da.Fill( dt );
          dataGridViewStudents.DataSource = dt;
          }
        }
      }

    // UPDATE Operation (Datensatz aktualisieren)
    private void btnUpdate_Click( object sender, EventArgs e )
      {
      if ( dataGridViewStudents.SelectedRows.Count > 0 )
        {
        using ( SqlConnection con = new SqlConnection( connectionString ) )
          {
          con.Open();
          string query = "UPDATE Students SET Name = @Name WHERE ID = @ID";
          using ( SqlCommand cmd = new SqlCommand( query, con ) )
            {
            int id = Convert.ToInt32( dataGridViewStudents.SelectedRows[ 0 ].Cells[ 0 ].Value );
            cmd.Parameters.AddWithValue( "@ID", id );
            cmd.Parameters.AddWithValue( "@Name", txtName.Text );
            cmd.ExecuteNonQuery();
            MessageBox.Show( "Student aktualisiert" );
            LoadData();
            }
          }
        }
      }

    // DELETE Operation (Datensatz löschen)
    private void btnDelete_Click( object sender, EventArgs e )
      {
      if ( dataGridViewStudents.SelectedRows.Count > 0 )
        {
        using ( SqlConnection con = new SqlConnection( connectionString ) )
          {
          con.Open();
          string query = "DELETE FROM Students WHERE ID = @ID";
          using ( SqlCommand cmd = new SqlCommand( query, con ) )
            {
            int id = Convert.ToInt32( dataGridViewStudents.SelectedRows[ 0 ].Cells[ 0 ].Value );
            cmd.Parameters.AddWithValue( "@ID", id );
            cmd.ExecuteNonQuery();
            MessageBox.Show( "Student gelöscht" );
            LoadData();
            }
          }
        }
      }
    }
  }
