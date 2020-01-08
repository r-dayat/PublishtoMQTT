using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace PublishtoMQTT
{
    public partial class Form1 : Form
    {
        private MqttClient _client;
        private byte _code;
        private String _host;
        private int _port;
        private String _username;
        private String _password;
        private String _topic;
        private String _message;
        private ushort _msgId;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            closeForm();
            System.Windows.Forms.Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeForm();
        }

        private void bConnect_Click(object sender, EventArgs e)
        {
            connectMqtt();
        }

        private void bDisconnect_Click(object sender, EventArgs e)
        {
            disconnectMqtt();
        }

        private void connectMqtt()
        {

            try
            {
                _host = txtHost.Text;
                _port = int.Parse(txtPort.Text);
                _username = txtUsername.Text;
                _password = txtPassword.Text;
                _topic = txtTopic.Text;
                _client = new MqttClient(_host, _port, false, null, null, MqttSslProtocols.None);
                _code = _client.Connect(Guid.NewGuid().ToString(), _username, _password, false, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true, _topic, "Connect", true, 60);

                if (_client.IsConnected)
                {
                    MessageBox.Show("Connect Succeed");
                    bDisconnect.Enabled = true;
                    bPublish.Enabled = true;
                    txtMessage.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Connect Failed");
                }
            }
            catch (Exception e)
            {

            }
            
        }

        private void disconnectMqtt()
        {
            try
            {
                if (_client.IsConnected)
                {
                    _client.Disconnect();
                    MessageBox.Show("Disconnect Succeed");
                }
                else
                {
                    MessageBox.Show("Client is not connect");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Disconnect Failed");
            }
        }

        private void closeForm()
        {
            try
            {
                disconnectMqtt();
            }
            catch (Exception ex)
            {

            }
            this.Dispose();
        }

        private void bPublish_Click(object sender, EventArgs e)
        {
            publishMqtt();
        }

        private void publishMqtt()
        {
            try
            {
                _topic = txtTopic.Text;
                _message = txtMessage.Text;
                if (_message != "")
                {
                    _msgId = _client.Publish(_topic, Encoding.UTF8.GetBytes(_message), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
                }
                else
                {
                    MessageBox.Show("Please enter the message !");
                }
                
            }
            catch (Exception e)
            {

            }

        }
    }
}
