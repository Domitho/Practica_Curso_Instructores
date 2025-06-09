using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Practica_Curso_Instructores
{
    public partial class Curso : System.Web.UI.Page
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexionPractica"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarInstructores();
                ListarCursos();
            }
        }

        void CargarInstructores()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT id_instructor, nombre FROM Instructor", cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ddlInstructor.DataSource = dt;
            ddlInstructor.DataTextField = "nombre";
            ddlInstructor.DataValueField = "id_instructor";
            ddlInstructor.DataBind();
            ddlInstructor.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        void ListarCursos()
        {
            SqlDataAdapter da = new SqlDataAdapter("sp_listar_cursos", cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gvCursos.DataSource = dt;
            gvCursos.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            LimpiarMensajes();

            if (!CamposValidos()) return;

            cn.Open();
            SqlCommand cmd = new SqlCommand(hfIdCurso.Value == "" ? "sp_insertar_curso" : "sp_actualizar_curso", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
            cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());
            cmd.Parameters.AddWithValue("@fecha_inicio", txtFechaInicio.Text);
            cmd.Parameters.AddWithValue("@fecha_fin", txtFechaFin.Text);
            cmd.Parameters.AddWithValue("@fk_id_instructor", ddlInstructor.SelectedValue);

            if (hfIdCurso.Value != "")
                cmd.Parameters.AddWithValue("@id_curso", hfIdCurso.Value);

            cmd.ExecuteNonQuery();
            cn.Close();

            MostrarAlerta("success", hfIdCurso.Value == "" ? "Curso agregado correctamente" : "Curso actualizado correctamente");

            Limpiar();
            ListarCursos();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void gvCursos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar" || e.CommandName == "Eliminar")
            {
                int index = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
                string id = gvCursos.DataKeys[index].Value.ToString();
                GridViewRow row = gvCursos.Rows[index];

                if (e.CommandName == "Editar")
                {
                    hfIdCurso.Value = id;

                    txtNombre.Text = row.Cells[1].Text; // nombre_curso
                    txtDescripcion.Text = row.Cells[2].Text;
                    txtFechaInicio.Text = Convert.ToDateTime(row.Cells[3].Text).ToString("yyyy-MM-dd");
                    txtFechaFin.Text = Convert.ToDateTime(row.Cells[4].Text).ToString("yyyy-MM-dd");

                    // Encuentra el instructor por texto visible
                    ddlInstructor.SelectedIndex = ddlInstructor.Items.IndexOf(
                        ddlInstructor.Items.FindByText(row.Cells[5].Text)
                    );
                }
                else if (e.CommandName == "Eliminar")
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_eliminar_curso", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_curso", id);
                    cmd.ExecuteNonQuery();
                    cn.Close();

                    MostrarAlerta("success", "Curso eliminado correctamente");
                    ListarCursos();
                }
            }
        }


        private bool CamposValidos()
        {
            bool valido = true;

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                lblErrorNombre.Text = "Por favor, ingrese el nombre.";
                valido = false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                lblErrorDescripcion.Text = "Por favor, ingrese la descripción.";
                valido = false;
            }

            if (string.IsNullOrWhiteSpace(txtFechaInicio.Text))
            {
                lblErrorFechaInicio.Text = "Ingrese la fecha de inicio.";
                valido = false;
            }

            if (string.IsNullOrWhiteSpace(txtFechaFin.Text))
            {
                lblErrorFechaFin.Text = "Ingrese la fecha de fin.";
                valido = false;
            }

            if (ddlInstructor.SelectedValue == "")
            {
                lblErrorInstructor.Text = "Seleccione un instructor.";
                valido = false;
            }

            return valido;
        }

        private void LimpiarMensajes()
        {
            lblErrorNombre.Text = "";
            lblErrorDescripcion.Text = "";
            lblErrorFechaInicio.Text = "";
            lblErrorFechaFin.Text = "";
            lblErrorInstructor.Text = "";
        }

        private void Limpiar()
        {
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            ddlInstructor.SelectedIndex = 0;
            hfIdCurso.Value = "";
            LimpiarMensajes();
        }

        private void MostrarAlerta(string tipo, string mensaje)
        {
            string script = $"Swal.fire({{ icon: '{tipo}', title: '{mensaje}', showConfirmButton: false, timer: 2000 }});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }
}
