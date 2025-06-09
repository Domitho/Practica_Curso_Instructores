using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace Practica_Curso_Instructores
{
    public partial class Instructores : System.Web.UI.Page
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexionPractica"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Listar();
            }
        }

        protected void Listar()
        {
            SqlDataAdapter da = new SqlDataAdapter("sp_listar_instructores", cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gvInstructores.DataSource = dt;
            gvInstructores.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            LimpiarMensajesError();

            if (!CamposValidos())
                return;

            cn.Open();
            SqlCommand cmd = new SqlCommand(hfId.Value == "" ? "sp_insertar_instructor" : "sp_actualizar_instructor", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
            cmd.Parameters.AddWithValue("@especialidad", txtEspecialidad.Text.Trim());
            cmd.Parameters.AddWithValue("@correo", txtCorreo.Text.Trim());
            cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text.Trim());

            if (hfId.Value != "")
                cmd.Parameters.AddWithValue("@id_instructor", hfId.Value);

            cmd.ExecuteNonQuery();
            cn.Close();

            string mensaje = hfId.Value == "" ? "Instructor agregado correctamente" : "Instructor actualizado correctamente";
            MostrarAlerta("success", mensaje);

            Limpiar();
            Listar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void gvInstructores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar" || e.CommandName == "Eliminar")
            {
                Button btn = (Button)e.CommandSource;
                GridViewRow row = (GridViewRow)btn.NamingContainer;
                string id = row.Cells[0].Text;

                if (e.CommandName == "Editar")
                {
                    hfId.Value = id;
                    txtNombre.Text = row.Cells[1].Text;
                    txtEspecialidad.Text = row.Cells[2].Text;
                    txtCorreo.Text = row.Cells[3].Text;
                    txtTelefono.Text = row.Cells[4].Text;
                }
                else if (e.CommandName == "Eliminar")
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_eliminar_instructor", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_instructor", id);
                    cmd.ExecuteNonQuery();
                    cn.Close();

                    MostrarAlerta("success", "Instructor eliminado correctamente");
                    Listar();
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

            if (string.IsNullOrWhiteSpace(txtEspecialidad.Text))
            {
                lblErrorEspecialidad.Text = "Por favor, ingrese la especialidad.";
                valido = false;
            }

            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                lblErrorCorreo.Text = "Por favor, ingrese el correo.";
                valido = false;
            }

            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                lblErrorTelefono.Text = "Por favor, ingrese el teléfono.";
                valido = false;
            }
            else if (!txtTelefono.Text.All(char.IsDigit))
            {
                lblErrorTelefono.Text = "Solo se permiten números.";
                valido = false;
            }

            return valido;
        }

        private void LimpiarMensajesError()
        {
            lblErrorNombre.Text = "";
            lblErrorEspecialidad.Text = "";
            lblErrorCorreo.Text = "";
            lblErrorTelefono.Text = "";
        }

        private void Limpiar()
        {
            txtNombre.Text = "";
            txtEspecialidad.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            hfId.Value = "";
            LimpiarMensajesError();
        }

        private void MostrarAlerta(string tipo, string mensaje)
        {
            string script = $"Swal.fire({{ icon: '{tipo}', title: '{mensaje}', showConfirmButton: false, timer: 2000 }});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }
}
