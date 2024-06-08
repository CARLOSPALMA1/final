// AgregarAlumnoWindow.xaml.cs
using Final;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Final
{
    public partial class AgregarAlumnoWindow : Window
    {
        public Alumno NuevoAlumno { get; private set; }

        public AgregarAlumnoWindow()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            using (SqlConnection conn = ConexionBD.GetConnection())
            {
                conn.Open();
                string query = "SELECT UsuarioID, NombreUsuario FROM Usuarios";
                SqlCommand cmd = new SqlCommand(query, conn);
                UsuarioComboBox.ItemsSource = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                conn.Close();
            }
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            string carnet = CarnetTextBox.Text;
            string nombreCompleto = NombreCompletoTextBox.Text;
            string telefono = TelefonoTextBox.Text;
            string grado = GradoTextBox.Text;
            int usuarioID = (int)UsuarioComboBox.SelectedValue;

            if (string.IsNullOrWhiteSpace(carnet) || string.IsNullOrWhiteSpace(nombreCompleto) ||
                string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(grado))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection conn = ConexionBD.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Alumnos (Carnet, NombreCompleto, Telefono, Grado, UsuarioID) VALUES (@Carnet, @NombreCompleto, @Telefono, @Grado, @UsuarioID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Carnet", carnet);
                cmd.Parameters.AddWithValue("@NombreCompleto", nombreCompleto);
                cmd.Parameters.AddWithValue("@Telefono", telefono);
                cmd.Parameters.AddWithValue("@Grado", grado);
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            NuevoAlumno = new Alumno
            {
                Carnet = carnet,
                NombreCompleto = nombreCompleto,
                Telefono = telefono,
                Grado = grado,
                UsuarioID = usuarioID
            };

            DialogResult = true;
            Close();
        }
    }
}