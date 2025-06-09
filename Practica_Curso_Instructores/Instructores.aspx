<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Instructores.aspx.cs" Inherits="Practica_Curso_Instructores.Instructores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
    <h3 class="text-center mt-4">Gestión de Instructores</h3>

    <div class="row mb-3">
        <div class="col-md-6">
            <label>Nombre</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
        </div>
        <div class="col-md-6">
            <label>Especialidad</label>
            <asp:TextBox ID="txtEspecialidad" runat="server" CssClass="form-control" OnTextChanged="txtEspecialidad_TextChanged" />
        </div>
        <div class="col-md-6 mt-2">
            <label>Correo</label>
            <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" />
        </div>
        <div class="col-md-6 mt-2">
            <label>Teléfono</label>
            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="10" />
        </div>
    </div>

    <asp:HiddenField ID="hfId" runat="server" />

    <div class="mb-4">
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success me-2" OnClick="btnGuardar_Click" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" />
    </div>

    <asp:GridView ID="gvInstructores" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" OnRowCommand="gvInstructores_RowCommand">
        <Columns>
            <asp:BoundField DataField="id_instructor" HeaderText="ID" />
            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="especialidad" HeaderText="Especialidad" />
            <asp:BoundField DataField="correo" HeaderText="Correo" />
            <asp:BoundField DataField="telefono" HeaderText="Teléfono" />
            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:Button ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("id_instructor") %>' Text="✏️" CssClass="btn btn-warning btn-sm me-1" />
                    <asp:Button ID="btnEliminar" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("id_instructor") %>' Text="🗑️" CssClass="btn btn-danger btn-sm" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMensaje" runat="server" CssClass="d-none" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            document.getElementById('<%= txtTelefono.ClientID %>').addEventListener('input', function () {
                this.value = this.value.replace(/[^0-9]/g, '');
            });
        });
    </script>
</asp:Content>



