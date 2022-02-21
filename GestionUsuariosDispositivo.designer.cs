
namespace ControlEntradaSalida
{
    partial class GestionUsuariosDispositivo
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
            this.listView = new System.Windows.Forms.ListView();
            this.employeeNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.userType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.validEnable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.beginTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.endTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.numOfCard = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.numOfFace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.estado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonConsultar = new System.Windows.Forms.Button();
            this.checkBoxSeleccionarTodos = new System.Windows.Forms.CheckBox();
            this.textBoxTotal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAgregarTarjeta = new System.Windows.Forms.Button();
            this.buttonBackup = new System.Windows.Forms.Button();
            this.buttonEliminar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.CheckBoxes = true;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.employeeNo,
            this.name,
            this.userType,
            this.validEnable,
            this.beginTime,
            this.endTime,
            this.numOfCard,
            this.numOfFace,
            this.estado});
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(12, 82);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(790, 327);
            this.listView.TabIndex = 17;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            // 
            // employeeNo
            // 
            this.employeeNo.Text = "employeeNo";
            this.employeeNo.Width = 72;
            // 
            // name
            // 
            this.name.Text = "name";
            this.name.Width = 285;
            // 
            // userType
            // 
            this.userType.Text = "userType";
            // 
            // validEnable
            // 
            this.validEnable.Text = "validEnable";
            this.validEnable.Width = 73;
            // 
            // beginTime
            // 
            this.beginTime.Text = "beginTime";
            this.beginTime.Width = 65;
            // 
            // endTime
            // 
            this.endTime.Text = "endTime";
            this.endTime.Width = 61;
            // 
            // numOfCard
            // 
            this.numOfCard.DisplayIndex = 7;
            this.numOfCard.Text = "numOfCard";
            this.numOfCard.Width = 63;
            // 
            // numOfFace
            // 
            this.numOfFace.DisplayIndex = 6;
            this.numOfFace.Text = "numOfFace";
            this.numOfFace.Width = 72;
            // 
            // estado
            // 
            this.estado.Text = "Estado operaciones";
            // 
            // buttonConsultar
            // 
            this.buttonConsultar.Image = global::ControlEntradaSalida.Properties.Resources.Search_16x;
            this.buttonConsultar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonConsultar.Location = new System.Drawing.Point(12, 23);
            this.buttonConsultar.Name = "buttonConsultar";
            this.buttonConsultar.Size = new System.Drawing.Size(90, 23);
            this.buttonConsultar.TabIndex = 19;
            this.buttonConsultar.Text = "Consultar";
            this.buttonConsultar.UseVisualStyleBackColor = true;
            this.buttonConsultar.Click += new System.EventHandler(this.buttonConsultar_Click);
            // 
            // checkBoxSeleccionarTodos
            // 
            this.checkBoxSeleccionarTodos.AutoSize = true;
            this.checkBoxSeleccionarTodos.Location = new System.Drawing.Point(12, 59);
            this.checkBoxSeleccionarTodos.Name = "checkBoxSeleccionarTodos";
            this.checkBoxSeleccionarTodos.Size = new System.Drawing.Size(111, 17);
            this.checkBoxSeleccionarTodos.TabIndex = 22;
            this.checkBoxSeleccionarTodos.Text = "Seleccionar todos";
            this.checkBoxSeleccionarTodos.UseVisualStyleBackColor = true;
            this.checkBoxSeleccionarTodos.CheckedChanged += new System.EventHandler(this.checkBoxSeleccionarTodos_CheckedChanged);
            // 
            // textBoxTotal
            // 
            this.textBoxTotal.Location = new System.Drawing.Point(237, 57);
            this.textBoxTotal.Name = "textBoxTotal";
            this.textBoxTotal.ReadOnly = true;
            this.textBoxTotal.Size = new System.Drawing.Size(49, 20);
            this.textBoxTotal.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(143, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Total de usuarios";
            // 
            // buttonAgregarTarjeta
            // 
            this.buttonAgregarTarjeta.Image = global::ControlEntradaSalida.Properties.Resources.Edit_grey_16x;
            this.buttonAgregarTarjeta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAgregarTarjeta.Location = new System.Drawing.Point(256, 415);
            this.buttonAgregarTarjeta.Name = "buttonAgregarTarjeta";
            this.buttonAgregarTarjeta.Size = new System.Drawing.Size(116, 23);
            this.buttonAgregarTarjeta.TabIndex = 21;
            this.buttonAgregarTarjeta.Text = "Agregar tarjeta";
            this.buttonAgregarTarjeta.UseVisualStyleBackColor = true;
            this.buttonAgregarTarjeta.Click += new System.EventHandler(this.buttonAgregarTarjeta_Click);
            // 
            // buttonBackup
            // 
            this.buttonBackup.Image = global::ControlEntradaSalida.Properties.Resources.Save_grey_16x;
            this.buttonBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonBackup.Location = new System.Drawing.Point(134, 415);
            this.buttonBackup.Name = "buttonBackup";
            this.buttonBackup.Size = new System.Drawing.Size(116, 23);
            this.buttonBackup.TabIndex = 20;
            this.buttonBackup.Text = "Backup";
            this.buttonBackup.UseVisualStyleBackColor = true;
            this.buttonBackup.Click += new System.EventHandler(this.buttonBackup_Click);
            // 
            // buttonEliminar
            // 
            this.buttonEliminar.Image = global::ControlEntradaSalida.Properties.Resources.DeleteAllRows_16x;
            this.buttonEliminar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonEliminar.Location = new System.Drawing.Point(12, 415);
            this.buttonEliminar.Name = "buttonEliminar";
            this.buttonEliminar.Size = new System.Drawing.Size(116, 23);
            this.buttonEliminar.TabIndex = 18;
            this.buttonEliminar.Text = "Eliminar";
            this.buttonEliminar.UseVisualStyleBackColor = true;
            this.buttonEliminar.Click += new System.EventHandler(this.buttonEliminar_Click);
            // 
            // GestionUsuariosDispositivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxTotal);
            this.Controls.Add(this.checkBoxSeleccionarTodos);
            this.Controls.Add(this.buttonAgregarTarjeta);
            this.Controls.Add(this.buttonBackup);
            this.Controls.Add(this.buttonConsultar);
            this.Controls.Add(this.buttonEliminar);
            this.Controls.Add(this.listView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GestionUsuariosDispositivo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestion de usuarios en dispositivo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader employeeNo;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader userType;
        private System.Windows.Forms.ColumnHeader validEnable;
        private System.Windows.Forms.ColumnHeader beginTime;
        private System.Windows.Forms.ColumnHeader endTime;
        private System.Windows.Forms.Button buttonEliminar;
        private System.Windows.Forms.Button buttonConsultar;
        private System.Windows.Forms.ColumnHeader numOfFace;
        private System.Windows.Forms.Button buttonBackup;
        private System.Windows.Forms.ColumnHeader numOfCard;
        private System.Windows.Forms.ColumnHeader estado;
        private System.Windows.Forms.Button buttonAgregarTarjeta;
        private System.Windows.Forms.CheckBox checkBoxSeleccionarTodos;
        private System.Windows.Forms.TextBox textBoxTotal;
        private System.Windows.Forms.Label label1;
    }
}