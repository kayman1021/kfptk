using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace p00
{
    class PPP_Groupbox : GroupBox
    {
        public PPP_Groupbox(string Name, int SizeX, int SizeY, int TabNumber)
        {
            Text = Name;
            Location = new System.Drawing.Point((TabNumber - 1) * SizeX, 0);
            Size = new System.Drawing.Size(SizeX, SizeY);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12);
            PPP_Button button_Add = new PPP_Button("Add", 100, 24, 4, 24, 0);
            PPP_Button button_Remove = new PPP_Button("Remove", 100, 24, 104, 24, 1);
            PPP_Button button_Clear = new PPP_Button("Clear", 100, 24, 204, 24, 2);
            PPP_Listbox listbox_Elements = new PPP_Listbox("Elements", 300, 200, 4, 48, 3);
            PPP_RadioButton radiobutton_Single_DNG_14bit = new PPP_RadioButton("Single DNG 14bit(MLV export)", 300, 24, 4, 248,4);

            this.Controls.Add(button_Add);
            this.Controls.Add(button_Remove);
            this.Controls.Add(button_Clear);
            this.Controls.Add(listbox_Elements);
            this.Controls.Add(radiobutton_Single_DNG_14bit);
        }
    }
    class PPP_Button : Button
    {
        public PPP_Button(string Name, int SizeX, int SizeY, int LocationX, int LocationY, int TabIndex)
        {
            this.Size = new System.Drawing.Size(100, 24);
            this.Location = new System.Drawing.Point(LocationX, LocationY);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10);
            this.Name = Name;
            this.TabIndex = TabIndex;
            this.Text = Name;
            this.UseVisualStyleBackColor = true;
        }
    }

    class PPP_Listbox : ListBox
    {
        public PPP_Listbox(string Name, int SizeX, int SizeY, int LocationX, int LocationY, int TabIndex)
        {
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10);
            this.FormattingEnabled = true;
            this.ItemHeight = 10;
            this.Location = new System.Drawing.Point(LocationX, LocationY);
            this.Name = Name;
            this.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Size = new System.Drawing.Size(SizeX, SizeY);
            this.TabIndex = TabIndex;
        }
    }

    class PPP_RadioButton : RadioButton
    {
        public PPP_RadioButton(string Name, int SizeX, int SizeY, int LocationX, int LocationY, int TabIndex)
        {
            //this.AutoSize = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10);
            this.Location = new System.Drawing.Point(LocationX, LocationY);
            //this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = Name;
            this.Size = new System.Drawing.Size(SizeX, SizeY);
            this.TabIndex = TabIndex;
            this.Text = Name;
            //this.UseVisualStyleBackColor = true;
        }
    }
}
