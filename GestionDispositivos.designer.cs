
namespace ControlEntradaSalida
{
    partial class GestionDispositivos
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.listView = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nombre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.descripcion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.direccionip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.puerto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.usuario = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.conectado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.activo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.predeterminado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ultimavez = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonNuevo = new System.Windows.Forms.Button();
            this.buttonEliminar = new System.Windows.Forms.Button();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.listView);
            this.groupBox.Location = new System.Drawing.Point(12, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(546, 188);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Dispositivos biométricos";
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.nombre,
            this.descripcion,
            this.direccionip,
            this.puerto,
            this.usuario,
            this.conectado,
            this.activo,
            this.predeterminado,
            this.ultimavez});
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(6, 19);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(534, 163);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
            // 
            // id
            // 
            this.id.Text = "Id";
            this.id.Width = 36;
            // 
            // nombre
            // 
            this.nombre.Text = "Nombre";
            this.nombre.Width = 95;
            // 
            // descripcion
            // 
            this.descripcion.Text = "Descripción";
            this.descripcion.Width = 104;
            // 
            // direccionip
            // 
            this.direccionip.Text = "Dirección IP";
            this.direccionip.Width = 85;
            // 
            // puerto
            // 
            this.puerto.Text = "Puerto";
            this.puerto.Width = 72;
            // 
            // usuario
            // 
            this.usuario.Text = "Usuario";
            this.usuario.Width = 63;
            // 
            // conectado
            // 
            this.conectado.Text = "Conectado";
            this.conectado.Width = 71;
            // 
            // activo
            // 
            this.activo.Text = "Activo";
            // 
            // predeterminado
            // 
            this.predeterminado.Text = "Predeterminado";
            // 
            // ultimavez
            // 
            this.ultimavez.Text = "Última vez";
            // 
            // buttonNuevo
            // 
            this.buttonNuevo.Location = new System.Drawing.Point(18, 206);
            this.buttonNuevo.Name = "buttonNuevo";
            this.buttonNuevo.Size = new System.Drawing.Size(75, 23);
            this.buttonNuevo.TabIndex = 1;
            this.buttonNuevo.Text = "&Nuevo";
            this.buttonNuevo.UseVisualStyleBackColor = true;
            this.buttonNuevo.Click += new System.EventHandler(this.buttonNuevo_Click);
            // 
            // buttonEliminar
            // 
            this.buttonEliminar.Location = new System.Drawing.Point(99, 206);
            this.buttonEliminar.Name = "buttonEliminar";
            this.buttonEliminar.Size = new System.Drawing.Size(75, 23);
            this.buttonEliminar.TabIndex = 2;
            this.buttonEliminar.Text = "&Eliminar";
            this.buttonEliminar.UseVisualStyleBackColor = true;
            this.buttonEliminar.Click += new System.EventHandler(this.buttonEliminar_Click);
            // 
            // GestionDispositivos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 240);
            this.Controls.Add(this.buttonEliminar);
            this.Controls.Add(this.buttonNuevo);
            this.Controls.Add(this.groupBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GestionDispositivos";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestion de dispositivos";
            this.Load += new System.EventHandler(this.GestionDispositivos_Load);
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader nombre;
        private System.Windows.Forms.ColumnHeader descripcion;
        private System.Windows.Forms.ColumnHeader direccionip;
        private System.Windows.Forms.ColumnHeader usuario;
        private System.Windows.Forms.ColumnHeader conectado;
        private System.Windows.Forms.ColumnHeader puerto;
        private System.Windows.Forms.Button buttonNuevo;
        private System.Windows.Forms.Button buttonEliminar;
        private System.Windows.Forms.ColumnHeader activo;
        private System.Windows.Forms.ColumnHeader predeterminado;
        private System.Windows.Forms.ColumnHeader ultimavez;
    }
}