using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net.Sockets;

namespace TestTCP
{
	[Activity (Label = "TestTCP", MainLauncher = true)]
	public class Activity1 : Activity
	{
		System.Text.ASCIIEncoding encoding=new System.Text.ASCIIEncoding();
		byte[] readBuffer = new byte[100];
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			
			Button button = FindViewById<Button> (Resource.Id.myButton);			
			button.Click += HandleButtonClick;
			
			System.Timers.Timer t = new System.Timers.Timer(3000);
			t.AutoReset = true;
			t.Elapsed += (sender, e) => {
				MakeTCP();
			};
			t.Start();
		}
		
		
		void HandleButtonClick (object sender, EventArgs e)
		{
			GC.Collect();
		}
		
		
		
		void MakeTCP ()
		{
			Android.Util.Log.Info("-------------", "---------------------------------------------");
			Android.Util.Log.Info("-------------", "---------------------------------------------");
			Android.Util.Log.Info("-------------", "Begin Connect");
			TcpClient tcpClient = new TcpClient();			
			tcpClient.BeginConnect("google.com", 80, ConnectCompleted, tcpClient);
		}
		
		
		void ConnectCompleted (IAsyncResult res)
		{
			Android.Util.Log.Info("-------------", "ConnectCompleted");
			TcpClient tcpClient = (TcpClient)res.AsyncState;
			tcpClient.EndConnect(res);
			
			
			byte[] buf = encoding.GetBytes( "GET\n\n" );
			Android.Util.Log.Info("-------------", "BeginWrite");
			tcpClient.GetStream().BeginWrite(buf, 0, buf.Length, WriteCompleted, tcpClient);
		}
				
		
		void WriteCompleted (IAsyncResult res){			
			Android.Util.Log.Info("-------------", "WriteCompleted");
			TcpClient tcpClient = (TcpClient)res.AsyncState;
			tcpClient.GetStream().EndWrite(res);
			
			
			Android.Util.Log.Info( "tcpClient.Connected", tcpClient.Connected.ToString());
			Android.Util.Log.Info("-------------", "BeginRead");
			tcpClient.GetStream().BeginRead(readBuffer, 0, readBuffer.Length, ReadCompleted, tcpClient);
		}
		
		
		void ReadCompleted (IAsyncResult res){
			Android.Util.Log.Info("-------------", "ReadCompleted");
			TcpClient tcpClient = (TcpClient)res.AsyncState;
			tcpClient.GetStream().EndRead(res);
			
			Android.Util.Log.Info("Server response", encoding.GetString( readBuffer ) );
		}
	}
}


