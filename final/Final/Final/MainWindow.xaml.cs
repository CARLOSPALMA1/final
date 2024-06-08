using Final;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace Final
{
    public partial class MainWindow : Window
    {
        private readonly List<Alumno> alumnos = new List<Alumno>();

        public MainWindow()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            using (SqlConnection conn = ConexionBD.GetConnection())
            {
                conn.Open();

                string query = "SELECT a.Carnet, a.NombreCompleto, a.Telefono, a.Grado, a.UsuarioID, u.NombreUsuario " +
                               "FROM Alumnos a " +
                               "LEFT JOIN Usuarios u ON a.UsuarioID = u.UsuarioID";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                alumnos.Clear();

                while (reader.Read())
                {
                    Alumno alumno = new Alumno
                    {
                        Carnet = reader.GetString(0),
                        NombreCompleto = reader.GetString(1),
                        Telefono = reader.GetString(2),
                        Grado = reader.GetString(3),
                        UsuarioID = reader.GetInt32(4),
                        NombreUsuario = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                    };

                    alumnos.Add(alumno);
                }

                reader.Close();
                conn.Close();

                AlumnosDataGrid.ItemsSource = alumnos;
            }
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            AgregarAlumnoWindow agregarAlumnoWindow = new AgregarAlumnoWindow();
            if (agregarAlumnoWindow.ShowDialog() == true)
            {
                Alumno nuevoAlumno = agregarAlumnoWindow.NuevoAlumno;
                alumnos.Add(nuevoAlumno);
                AlumnosDataGrid.Items.Refresh();
            }
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (AlumnosDataGrid.SelectedItem is Alumno alumnoSeleccionado)
            {
                EditarAlumnoWindow editarAlumnoWindow = new EditarAlumnoWindow(alumnoSeleccionado);
                if (editarAlumnoWindow.ShowDialog() == true)
                {
                    Alumno alumnoEditado = editarAlumnoWindow.AlumnoEditado;
                    int index = alumnos.IndexOf(alumnoSeleccionado);
                    alumnos[index] = alumnoEditado;
                    AlumnosDataGrid.Items.Refresh();
                }
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (AlumnosDataGrid.SelectedItem is Alumno alumnoSeleccionado)
            {
                if (MessageBox.Show($"¿Está seguro de eliminar al alumno {alumnoSeleccionado.NombreCompleto}?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (SqlConnection conn = ConexionBD.GetConnection())
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("DELETE FROM Alumnos WHERE Carnet = @Carnet", conn);
                        cmd.Parameters.AddWithValue("@Carnet", alumnoSeleccionado.Carnet);
                        cmd.ExecuteNonQuery();

                        conn.Close();
                    }

                    alumnos.Remove(alumnoSeleccionado);
                    AlumnosDataGrid.Items.Refresh();
                }
            }
        }
    }

    public class Alumno
    {
        public string Carnet { get; set; }
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public string Grado { get; set; }
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
    }
}