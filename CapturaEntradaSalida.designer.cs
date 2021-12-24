
namespace ControlEntradaSalida
{
    partial class CapturaEntradaSalida
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listViewEventos = new System.Windows.Forms.ListView();
            this.num = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fecha = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hora = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.evento = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.documento = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nombres = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listViewEventos);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 307);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // listViewEventos
            // 
            this.listViewEventos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.num,
            this.fecha,
            this.hora,
            this.evento,
            this.documento,
            this.nombres});
            this.listViewEventos.FullRowSelect = true;
            this.listViewEventos.HideSelection = false;
            this.listViewEventos.Location = new System.Drawing.Point(6, 19);
            this.listViewEventos.Name = "listViewEventos";
            this.listViewEventos.Size = new System.Drawing.Size(764, 271);
            this.listViewEventos.TabIndex = 0;
            this.listViewEventos.UseCompatibleStateImageBehavior = false;
            this.listViewEventos.View = System.Windows.Forms.View.Details;
            // 
            // num
            // 
            this.num.Text = "Num";
            this.num.Width = 39;
            // 
            // fecha
            // 
            this.fecha.Text = "Fecha";
            this.fecha.Width = 79;
            // 
            // hora
            // 
            this.hora.Text = "Hora";
            // 
            // evento
            // 
            this.evento.Text = "Evento";
            this.evento.Width = 170;
            // 
            // documento
            // 
            this.documento.Text = "Documento";
            // 
            // nombres
            // 
            this.nombres.Text = "Nombres";
            // 
            // CapturaEntradaSalida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 333);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CapturaEntradaSalida";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Captura de entrada y salida";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GestionEventos_FormClosing);
            this.Load += new System.EventHandler(this.GestionEventos_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listViewEventos;
        private System.Windows.Forms.ColumnHeader num;
        private System.Windows.Forms.ColumnHeader fecha;
        private System.Windows.Forms.ColumnHeader hora;
        private System.Windows.Forms.ColumnHeader evento;
        private System.Windows.Forms.ColumnHeader documento;
        private System.Windows.Forms.ColumnHeader nombres;
    }
}