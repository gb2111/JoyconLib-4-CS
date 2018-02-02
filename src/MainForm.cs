using Joycon4CS;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoyConTest
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			drawOrigin = new Point(picture1.Width / 2, picture1.Height / 2);
			cube1 = new Math3D.Math3D.Cube(200, 100, 20);
			cube2 = new Math3D.Math3D.Cube(200, 100, 20);

		}

		JoyconManager joyconManager = new JoyconManager();

		Point drawOrigin;
		Math3D.Math3D.Cube cube1;
		Math3D.Math3D.Cube cube2;


		private void buttonScan_Click(object sender, EventArgs e)
		{
			joyconManager.Scan();

			UpdateDebugType();
			UpdateInfo();

		}

		private void UpdateDebugType()
		{
			foreach (var j in joyconManager.j)
				j.debug_type = Joycon.DebugType.NONE;
		}



		private void buttonStart_Click(object sender, EventArgs e)
		{
			joyconManager.Start();
			timerUpdate.Enabled = true;
		}

		private void timerUpdate_Tick(object sender, EventArgs e)
		{
			joyconManager.Update();

			UpdateInfo();
		}

		private void UpdateInfo()
		{
			if (joyconManager.j.Count > 0)
			{
				var j = joyconManager.j[0];

				label1.Text = j.ToString();

				cube1.InitializeCube();
				cube1.RotateX = (float)(j.GetVector().eulerAngles.Y * 180.0f / Math.PI);
				cube1.RotateY = (float)(j.GetVector().eulerAngles.Z * 180.0f / Math.PI);
				cube1.RotateZ = (float)(j.GetVector().eulerAngles.X * 180.0f / Math.PI);

				picture1.Image = cube1.DrawCube(drawOrigin);
			}
			else
			{
				if (label1.Text != "")
					label1.Text = "not found";
			}

			if (joyconManager.j.Count > 1)
			{
				var j = joyconManager.j[1];

				label2.Text = j.ToString();
				picture2.Image = cube2.DrawCube(drawOrigin);


				cube2.InitializeCube();
				cube2.RotateX = (float)(j.GetVector().eulerAngles.Y * 180.0f / Math.PI);
				cube2.RotateY = (float)(j.GetVector().eulerAngles.Z * 180.0f / Math.PI);
				cube2.RotateZ = (float)(j.GetVector().eulerAngles.X * 180.0f / Math.PI);

				picture1.Image = cube2.DrawCube(drawOrigin);

			}
			else
			{
				if (label2.Text != "")
					label2.Text = "not found";
			}



		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			joyconManager.OnApplicationQuit();
		}


	}
}
