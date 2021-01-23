using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;

namespace CodeProjectSerialComms
{
   public partial class Form1 : Form
    {
        SerialPort ComPort = new SerialPort();  

        internal delegate void SerialDataReceivedEventHandlerDelegate(
                 object sender, SerialDataReceivedEventArgs e);

        internal delegate void SerialPinChangedEventHandlerDelegate(
                 object sender, SerialPinChangedEventArgs e);
        private SerialPinChangedEventHandler SerialPinChangedEventHandler1;
        delegate void SetTextCallback(string text);
         string InputData = String.Empty;
        

         internal void PinChanged(object sender, SerialPinChangedEventArgs e)
       {
            SerialPinChange SerialPinChange1 = 0;
            bool signalState = false;
            SerialPinChange1 = e.EventType;
            lblCTSStatus.BackColor = Color.Green;
            lblDSRStatus.BackColor = Color.Green;
            lblRIStatus.BackColor = Color.Green;
            lblBreakStatus.BackColor = Color.Green;

            switch (SerialPinChange1)
           {
               case SerialPinChange.Break:
                    lblBreakStatus.BackColor = Color.Red;
                    //MessageBox.Show("Break is Set");
                 break;
               case SerialPinChange.CDChanged:
                    signalState = ComPort.CtsHolding;
                  //  MessageBox.Show("CD = " + signalState.ToString());
                 break;
               case SerialPinChange.CtsChanged:
                    signalState = ComPort.CDHolding;
                    lblCTSStatus.BackColor = Color.Red;
                   //MessageBox.Show("CTS = " + signalState.ToString());
                  break;
              case SerialPinChange.DsrChanged:
                   signalState = ComPort.DsrHolding;
                   lblDSRStatus.BackColor = Color.Red;
                   // MessageBox.Show("DSR = " + signalState.ToString());
                break;
              case SerialPinChange.Ring:
                    lblRIStatus.BackColor = Color.Red;
                    //MessageBox.Show("Ring Detected");
                   break;
           }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

            SerialPinChangedEventHandler1 = new SerialPinChangedEventHandler(PinChanged);
            ComPort.PinChanged += SerialPinChangedEventHandler1;
            ComPort.Open();
            ComPort.RtsEnable = true;
            ComPort.DtrEnable = true;
            btnTest.Enabled = false;

        }
       private void btnPortState_Click(object sender, EventArgs e)
       {
            if (btnPortState.Text == "Closed")
            {
                btnPortState.Text = "Open";
                ComPort.PortName = Convert.ToString(cboPorts.Text);
                ComPort.BaudRate = Convert.ToInt32(cboBaudRate.Text);
                ComPort.DataBits = Convert.ToInt16(cboDataBits.Text);
                ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cboStopBits.Text);
                ComPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), cboHandShaking.Text);
                ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), cboParity.Text);
                ComPort.Open();
            }
            else if (btnPortState.Text == "Open")
            {
                btnPortState.Text = "Closed";
                ComPort.Close();
            }
        }

        private void rtbOutgoing_KeyPress(object sender, KeyPressEventArgs e)
        {
          if (e.KeyChar == (char)13) // enter key  
          {
               ComPort.Write("\r\n");
               rtbOutgoing.Text = "";
           }
           else if (e.KeyChar < 32 || e.KeyChar > 126)
            {
              e.Handled = true; // ignores anything else outside printable ASCII range  
            }
            else
            {
               ComPort.Write(e.KeyChar.ToString());
            }
        }
        private void btnHello_Click(object sender, EventArgs e)
        {
            ComPort.Write("Hello World!");
        }
        private void btnHyperTerm_Click(object sender, EventArgs e)
        {
            string Command1 = txtCommand.Text;
            string CommandSent;
            int Length, j = 0;

            Length = Command1.Length;
            for (int i = 0; i < Length; i++)
            {
                CommandSent = Command1.Substring(j, 1);
                ComPort.Write(CommandSent);
                j++;
            }
       }
    }
}