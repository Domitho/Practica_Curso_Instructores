<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Curso.aspx.cs" Inherits="Practica_Curso_Instructores.Curso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="text-center mt-4">Gestión de Cursos</h3>

    <div class="row mb-3">
        <div class="col-md-6">
            <label>Nombre del Curso</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
            <asp:Label ID="lblErrorNombre" runat="server" CssClass="text-danger" />
        </div>

        <div class="col-md-6">
            <label>Descripción</label>
            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" />
            <asp:Label ID="lblErrorDescripcion" runat="server" CssClass="text-danger" />
        </div>

        <div class="col-md-6 mt-2">
            <label>Fecha de Inicio</label>
            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" TextMode="Date" />
            <asp:Label ID="lblErrorFechaInicio" runat="server" CssClass="text-danger" />
        </div>

        <div class="col-md-6 mt-2">
            <label>Fecha de Fin</label>
            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" TextMode="Date" />
            <asp:Label ID="lblErrorFechaFin" runat="server" CssClass="text-danger" />
        </div>

        <div class="col-md-12 mt-2">
            <label>Instructor</label>
            <asp:DropDownList ID="ddlInstructor" runat="server" CssClass="form-control" />
            <asp:Label ID="lblErrorInstructor" runat="server" CssClass="text-danger" />
        </div>
    </div>

    <asp:HiddenField ID="hfIdCurso" runat="server" />

    <div class="mb-4">
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Curso" CssClass="btn btn-primary me-2" OnClick="btnGuardar_Click" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary" OnClick="btnCancelar_Click" />
    </div>

    <asp:GridView ID="gvCursos" runat="server" CssClass="table table-hover" AutoGenerateColumns="False" OnRowCommand="gvCursos_RowCommand" DataKeyNames="id_curso">
    <Columns>
        <asp:BoundField DataField="id_curso" HeaderText="ID" />
        <asp:BoundField DataField="nombre_curso" HeaderText="Nombre" />
        <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
        <asp:BoundField DataField="fecha_inicio" HeaderText="Inicio" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:BoundField DataField="fecha_fin" HeaderText="Fin" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:BoundField DataField="nombre_instructor" HeaderText="Instructor" />
        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <asp:Button ID="btnEditar" runat="server" CommandName="Editar" Text="Editar" CssClass="btn btn-warning btn-sm me-1" />
                <asp:Button ID="btnEliminar" runat="server" CommandName="Eliminar" Text="Eliminar" CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('¿Estás seguro de eliminar este curso?');" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>
