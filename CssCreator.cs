using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Data;
using System . Drawing;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace WebHelper
{
	public partial class CssCreator : Form
	{
        AngularGradient ag;
        RadialGradient rg;
        Background bkg;
        Gradient g;

        public CssCreator( )
		{
			InitializeComponent();
            Gecko.Xpcom.Initialize(AppDomain.CurrentDomain.BaseDirectory+@"\xulrunner\");
            ag = new AngularGradient(false, false, "WRMULT.gif",
                -90, colors: new RGB[] { "#74c2e1", "#0191c8" });
            rg = new RadialGradient(false, false, "mnm_black250.png",
                radial: new Radial(size1: rdsSize.closest_corner,
                    position1: direction.center, position2: direction.center),
                colors: new RGB[] { "#74c2e1", "#0191c8", Color.Transparent }); ;

            g = ag;
            bkg = new Background(color: "gray", gradient: g);
            bkg.repeat = background_repeat.no_repeat;
            bkg.position = 1;

            propertyGrid1.SelectedObject = ag;
            propertyGrid2.SelectedObject = bkg;
            comboBox1.Items.Add("wrmult.gif");
            comboBox1.Items.Add("Application.png");
            comboBox1.Items.Add("Chart.png");
            comboBox1.Items.Add("mnm_black250.png");
            comboBox1.SelectedIndex = 0;

            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            radioButton2.CheckedChanged += radioButton1_CheckedChanged;
            button1.Click += result_Click;

            comboBox1.TextChanged += ComboBox1_TextChanged;
        }

        private void ComboBox1_TextChanged(object sender, EventArgs e)
        {
            g.imageUrl = comboBox1.Text;
            propertyGrid1.Refresh();
            propertyGrid2.Refresh();
        }

        private void result_Click(object sender , EventArgs e)
		{
            bkg.gradient = g;
            this.textBox1.Text = "{" + bkg + "}";          

            System.IO.File.WriteAllText(Application.StartupPath + @"\style.css","div.result"+ textBox1.Text);                       
            view.Navigate(Application.StartupPath+@"\demo.html");
		}

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                g = ag;
            }
            else {
                g = rg;
            }
            propertyGrid1.SelectedObject = g;
        }
    }
}
