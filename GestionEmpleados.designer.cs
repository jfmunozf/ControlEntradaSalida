
namespace ControlEntradaSalida
{
    partial class GestionEmpleados
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.buttonFiltrar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxTarjeta = new System.Windows.Forms.TextBox();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.listView = new System.Windows.Forms.ListView();
            this.documento = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.estado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tarjeta = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nombres = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.apellidos = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.foto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonNuevo = new System.Windows.Forms.Button();
            this.buttonEliminar = new System.Windows.Forms.Button();
            this.buttonAgregar = new System.Windows.Forms.Button();
            this.textBoxApellidos = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxNombres = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDocumento = new System.Windows.Forms.TextBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonCapturarFoto = new System.Windows.Forms.Button();
            this.pictureBoxUsuario = new System.Windows.Forms.PictureBox();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUsuario)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.buttonFiltrar);
            this.groupBox.Controls.Add(this.label6);
            this.groupBox.Controls.Add(this.textBoxTarjeta);
            this.groupBox.Controls.Add(this.cmbEstado);
            this.groupBox.Controls.Add(this.label10);
            this.groupBox.Controls.Add(this.listView);
            this.groupBox.Controls.Add(this.buttonNuevo);
            this.groupBox.Controls.Add(this.buttonEliminar);
            this.groupBox.Controls.Add(this.buttonAgregar);
            this.groupBox.Controls.Add(this.buttonCapturarFoto);
            this.groupBox.Controls.Add(this.textBoxApellidos);
            this.groupBox.Controls.Add(this.label3);
            this.groupBox.Controls.Add(this.textBoxNombres);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.textBoxDocumento);
            this.groupBox.Controls.Add(this.pictureBoxUsuario);
            this.groupBox.Location = new System.Drawing.Point(12, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(741, 426);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            // 
            // buttonFiltrar
            // 
            this.buttonFiltrar.Image = global::ControlEntradaSalida.Properties.Resources.Filter_16x;
            this.buttonFiltrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonFiltrar.Location = new System.Drawing.Point(533, 177);
            this.buttonFiltrar.Name = "buttonFiltrar";
            this.buttonFiltrar.Size = new System.Drawing.Size(113, 23);
            this.buttonFiltrar.TabIndex = 15;
            this.buttonFiltrar.Text = "&Filtrar";
            this.buttonFiltrar.UseVisualStyleBackColor = true;
            this.buttonFiltrar.Click += new System.EventHandler(this.buttonFiltrar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(530, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "&No. Tarjeta";
            // 
            // textBoxTarjeta
            // 
            this.textBoxTarjeta.Location = new System.Drawing.Point(533, 34);
            this.textBoxTarjeta.Name = "textBoxTarjeta";
            this.textBoxTarjeta.ReadOnly = true;
            this.textBoxTarjeta.Size = new System.Drawing.Size(195, 20);
            this.textBoxTarjeta.TabIndex = 3;
            // 
            // cmbEstado
            // 
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Items.AddRange(new object[] {
            "ACTIVO",
            "INACTIVO"});
            this.cmbEstado.Location = new System.Drawing.Point(312, 33);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(163, 21);
            this.cmbEstado.TabIndex = 2;
            this.cmbEstado.Text = "ACTIVO";
            this.cmbEstado.Validating += new System.ComponentModel.CancelEventHandler(this.cmbEstado_Validating);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(309, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "&Estado*";
            // 
            // listView
            // 
            this.listView.CheckBoxes = true;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.documento,
            this.estado,
            this.tarjeta,
            this.nombres,
            this.apellidos,
            this.foto});
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(6, 216);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(729, 204);
            this.listView.TabIndex = 16;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.Click += new System.EventHandler(this.listView_Click);
            // 
            // documento
            // 
            this.documento.Text = "Documento";
            this.documento.Width = 74;
            // 
            // estado
            // 
            this.estado.Text = "Estado";
            // 
            // tarjeta
            // 
            this.tarjeta.Text = "No. Tarjeta";
            this.tarjeta.Width = 74;
            // 
            // nombres
            // 
            this.nombres.Text = "Nombres";
            this.nombres.Width = 91;
            // 
            // apellidos
            // 
            this.apellidos.Text = "Apellidos";
            this.apellidos.Width = 94;
            // 
            // foto
            // 
            this.foto.Text = "Foto";
            // 
            // buttonNuevo
            // 
            this.buttonNuevo.Image = global::ControlEntradaSalida.Properties.Resources.NewItem_16x;
            this.buttonNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonNuevo.Location = new System.Drawing.Point(176, 177);
            this.buttonNuevo.Name = "buttonNuevo";
            this.buttonNuevo.Size = new System.Drawing.Size(113, 23);
            this.buttonNuevo.TabIndex = 14;
            this.buttonNuevo.Text = "&Nuevo";
            this.buttonNuevo.UseVisualStyleBackColor = true;
            this.buttonNuevo.Click += new System.EventHandler(this.buttonNuevo_Click);
            // 
            // buttonEliminar
            // 
            this.buttonEliminar.Image = global::ControlEntradaSalida.Properties.Resources.DeleteAllRows_16x;
            this.buttonEliminar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonEliminar.Location = new System.Drawing.Point(414, 177);
            this.buttonEliminar.Name = "buttonEliminar";
            this.buttonEliminar.Size = new System.Drawing.Size(113, 23);
            this.buttonEliminar.TabIndex = 13;
            this.buttonEliminar.Text = "&Eliminar";
            this.buttonEliminar.UseVisualStyleBackColor = true;
            this.buttonEliminar.Click += new System.EventHandler(this.buttonEliminar_Click);
            // 
            // buttonAgregar
            // 
            this.buttonAgregar.Image = global::ControlEntradaSalida.Properties.Resources.Save_grey_16x;
            this.buttonAgregar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAgregar.Location = new System.Drawing.Point(295, 177);
            this.buttonAgregar.Name = "buttonAgregar";
            this.buttonAgregar.Size = new System.Drawing.Size(113, 23);
            this.buttonAgregar.TabIndex = 12;
            this.buttonAgregar.Text = "&Guardar";
            this.buttonAgregar.UseVisualStyleBackColor = true;
            this.buttonAgregar.Click += new System.EventHandler(this.buttonAgregar_Click);
            // 
            // textBoxApellidos
            // 
            this.textBoxApellidos.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxApellidos.Location = new System.Drawing.Point(125, 141);
            this.textBoxApellidos.Name = "textBoxApellidos";
            this.textBoxApellidos.Size = new System.Drawing.Size(450, 20);
            this.textBoxApellidos.TabIndex = 5;
            this.textBoxApellidos.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxApellidos_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(125, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "&Apellidos*";
            // 
            // textBoxNombres
            // 
            this.textBoxNombres.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxNombres.Location = new System.Drawing.Point(125, 86);
            this.textBoxNombres.Name = "textBoxNombres";
            this.textBoxNombres.Size = new System.Drawing.Size(450, 20);
            this.textBoxNombres.TabIndex = 4;
            this.textBoxNombres.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxNombres_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(122, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Nombres*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Documento*";
            // 
            // textBoxDocumento
            // 
            this.textBoxDocumento.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxDocumento.Location = new System.Drawing.Point(125, 35);
            this.textBoxDocumento.Name = "textBoxDocumento";
            this.textBoxDocumento.Size = new System.Drawing.Size(151, 20);
            this.textBoxDocumento.TabIndex = 1;
            this.textBoxDocumento.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxDocumento_Validating);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // buttonCapturarFoto
            // 
            this.buttonCapturarFoto.Image = global::ControlEntradaSalida.Properties.Resources.CaptureFrame_16x;
            this.buttonCapturarFoto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCapturarFoto.Location = new System.Drawing.Point(6, 177);
            this.buttonCapturarFoto.Name = "buttonCapturarFoto";
            this.buttonCapturarFoto.Size = new System.Drawing.Size(113, 23);
            this.buttonCapturarFoto.TabIndex = 11;
            this.buttonCapturarFoto.Text = "&Capturar foto";
            this.buttonCapturarFoto.UseVisualStyleBackColor = true;
            this.buttonCapturarFoto.Click += new System.EventHandler(this.buttonCapturarFoto_Click);
            // 
            // pictureBoxUsuario
            // 
            this.pictureBoxUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxUsuario.Location = new System.Drawing.Point(6, 19);
            this.pictureBoxUsuario.Name = "pictureBoxUsuario";
            this.pictureBoxUsuario.Size = new System.Drawing.Size(113, 142);
            this.pictureBoxUsuario.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxUsuario.TabIndex = 0;
            this.pictureBoxUsuario.TabStop = false;
            // 
            // GestionEmpleados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 450);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GestionEmpleados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestion de empleados";
            this.Load += new System.EventHandler(this.GestionUsuarios_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUsuario)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDocumento;
        private System.Windows.Forms.PictureBox pictureBoxUsuario;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader documento;
        private System.Windows.Forms.ColumnHeader tarjeta;
        private System.Windows.Forms.ColumnHeader nombres;
        private System.Windows.Forms.ColumnHeader apellidos;
        private System.Windows.Forms.Button buttonNuevo;
        private System.Windows.Forms.Button buttonEliminar;
        private System.Windows.Forms.Button buttonAgregar;
        private System.Windows.Forms.Button buttonCapturarFoto;
        private System.Windows.Forms.TextBox textBoxApellidos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxNombres;
        private System.Windows.Forms.ColumnHeader foto;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ColumnHeader estado;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxTarjeta;
        private System.Windows.Forms.Button buttonFiltrar;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}