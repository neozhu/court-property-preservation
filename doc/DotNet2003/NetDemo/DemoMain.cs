using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ImApiDotNet;

namespace NetDemo
{
	/// <summary>
	/// Form1 的摘要说明。
	/// </summary>
	public class FrmDemo : System.Windows.Forms.Form
	{
		private ImApiDotNet.APIClient apiclient;
		private System.Windows.Forms.TextBox textUrl;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnRecvRpt;
		private System.Windows.Forms.Button btnRecvSM;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.Button btnRelease;
		private System.Windows.Forms.Button btnInit;
		private System.Windows.Forms.TextBox textPassword;
		private System.Windows.Forms.Label lblPws;
		private System.Windows.Forms.TextBox textUsername;
		private System.Windows.Forms.Label lblUser;
		private System.Windows.Forms.TextBox textCode;
		private System.Windows.Forms.Label lblCode;
		private System.Windows.Forms.TextBox textIp;
		private System.Windows.Forms.Label lblIP;
		private System.Windows.Forms.TextBox textSmId;
		private System.Windows.Forms.Label lblSMID;
		private System.Windows.Forms.TextBox textContext;
		private System.Windows.Forms.Label lblContent;
		private System.Windows.Forms.TextBox textMobile;
		private System.Windows.Forms.Label lblMobile;
		private String[] retunvalues=new String[]{"初始化成功","连接数据库出错","数据库关闭失败","数据库插入错误","数据库删除错误","数据库查询错误","参数错误","API标识非法","消息内容太长","没有初始化或初始化失败","API接口处于暂停（失效）状态","短信网关未连接"};
		private String[] initvalues = new String[]{"成功","连接失败","用户名或密码错误","密码错误","接口编码不存在"};
		private System.Windows.Forms.GroupBox dbConGroup;
		private System.Windows.Forms.Label lblSmType;
		private System.Windows.Forms.RadioButton rbnNormal;
		private System.Windows.Forms.RadioButton rbnWap;
		private System.Windows.Forms.Label lblUrl;
		private System.Windows.Forms.Label lblphoneID;
		private System.Windows.Forms.Label labelTime;
		private System.Windows.Forms.TextBox txtSMTime;
		private System.Windows.Forms.Label lbSMTime;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lbDemo;
		private System.Windows.Forms.TextBox txtDB;
		private System.Windows.Forms.TextBox txtSrcID;
		private System.Windows.Forms.RadioButton rbnSrcPdu;
		private System.Windows.Forms.Label label2;
		

		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmDemo()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}



		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.textUrl = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRecvRpt = new System.Windows.Forms.Button();
            this.btnRecvSM = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnRelease = new System.Windows.Forms.Button();
            this.btnInit = new System.Windows.Forms.Button();
            this.dbConGroup = new System.Windows.Forms.GroupBox();
            this.txtDB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.lblPws = new System.Windows.Forms.Label();
            this.textUsername = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.textCode = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.textIp = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.textSmId = new System.Windows.Forms.TextBox();
            this.lblSMID = new System.Windows.Forms.Label();
            this.textContext = new System.Windows.Forms.TextBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.textMobile = new System.Windows.Forms.TextBox();
            this.lblMobile = new System.Windows.Forms.Label();
            this.lblSmType = new System.Windows.Forms.Label();
            this.rbnNormal = new System.Windows.Forms.RadioButton();
            this.rbnWap = new System.Windows.Forms.RadioButton();
            this.lblUrl = new System.Windows.Forms.Label();
            this.lblphoneID = new System.Windows.Forms.Label();
            this.txtSrcID = new System.Windows.Forms.TextBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.txtSMTime = new System.Windows.Forms.TextBox();
            this.lbSMTime = new System.Windows.Forms.Label();
            this.lbDemo = new System.Windows.Forms.Label();
            this.rbnSrcPdu = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.dbConGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // textUrl
            // 
            this.textUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textUrl.Enabled = false;
            this.textUrl.Location = new System.Drawing.Point(224, 408);
            this.textUrl.Name = "textUrl";
            this.textUrl.Size = new System.Drawing.Size(448, 26);
            this.textUrl.TabIndex = 31;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(608, 657);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(96, 32);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRecvRpt
            // 
            this.btnRecvRpt.Location = new System.Drawing.Point(480, 657);
            this.btnRecvRpt.Name = "btnRecvRpt";
            this.btnRecvRpt.Size = new System.Drawing.Size(107, 32);
            this.btnRecvRpt.TabIndex = 28;
            this.btnRecvRpt.Text = "receiveRPT";
            this.btnRecvRpt.Click += new System.EventHandler(this.btnRecvRpt_Click);
            // 
            // btnRecvSM
            // 
            this.btnRecvSM.Location = new System.Drawing.Point(363, 657);
            this.btnRecvSM.Name = "btnRecvSM";
            this.btnRecvSM.Size = new System.Drawing.Size(96, 32);
            this.btnRecvSM.TabIndex = 27;
            this.btnRecvSM.Text = "receiveSM";
            this.btnRecvSM.Click += new System.EventHandler(this.btnRecvSM_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(256, 657);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(85, 32);
            this.btnSend.TabIndex = 26;
            this.btnSend.Text = "send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnRelease
            // 
            this.btnRelease.Location = new System.Drawing.Point(149, 657);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(86, 32);
            this.btnRelease.TabIndex = 25;
            this.btnRelease.Text = "release";
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(43, 657);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(85, 32);
            this.btnInit.TabIndex = 24;
            this.btnInit.Text = "init";
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // dbConGroup
            // 
            this.dbConGroup.Controls.Add(this.txtDB);
            this.dbConGroup.Controls.Add(this.label1);
            this.dbConGroup.Controls.Add(this.textPassword);
            this.dbConGroup.Controls.Add(this.lblPws);
            this.dbConGroup.Controls.Add(this.textUsername);
            this.dbConGroup.Controls.Add(this.lblUser);
            this.dbConGroup.Controls.Add(this.textCode);
            this.dbConGroup.Controls.Add(this.lblCode);
            this.dbConGroup.Controls.Add(this.textIp);
            this.dbConGroup.Controls.Add(this.lblIP);
            this.dbConGroup.Location = new System.Drawing.Point(75, 472);
            this.dbConGroup.Name = "dbConGroup";
            this.dbConGroup.Size = new System.Drawing.Size(629, 152);
            this.dbConGroup.TabIndex = 23;
            this.dbConGroup.TabStop = false;
            this.dbConGroup.Text = "数据库连接信息";
            // 
            // txtDB
            // 
            this.txtDB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDB.Location = new System.Drawing.Point(117, 109);
            this.txtDB.Name = "txtDB";
            this.txtDB.Size = new System.Drawing.Size(160, 26);
            this.txtDB.TabIndex = 15;
            this.txtDB.Text = "mas";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 22);
            this.label1.TabIndex = 14;
            this.label1.Text = "数据库名称";
            // 
            // textPassword
            // 
            this.textPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textPassword.Location = new System.Drawing.Point(427, 65);
            this.textPassword.Name = "textPassword";
            this.textPassword.PasswordChar = '*';
            this.textPassword.Size = new System.Drawing.Size(170, 26);
            this.textPassword.TabIndex = 13;
            this.textPassword.Text = "tyj000001";
            // 
            // lblPws
            // 
            this.lblPws.Location = new System.Drawing.Point(341, 76);
            this.lblPws.Name = "lblPws";
            this.lblPws.Size = new System.Drawing.Size(54, 22);
            this.lblPws.TabIndex = 12;
            this.lblPws.Text = "密码";
            // 
            // textUsername
            // 
            this.textUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textUsername.Location = new System.Drawing.Point(117, 65);
            this.textUsername.Name = "textUsername";
            this.textUsername.Size = new System.Drawing.Size(160, 26);
            this.textUsername.TabIndex = 11;
            this.textUsername.Text = "tyj000001";
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(32, 76);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(64, 22);
            this.lblUser.TabIndex = 10;
            this.lblUser.Text = "用户名";
            // 
            // textCode
            // 
            this.textCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textCode.Location = new System.Drawing.Point(427, 22);
            this.textCode.Name = "textCode";
            this.textCode.Size = new System.Drawing.Size(170, 26);
            this.textCode.TabIndex = 9;
            this.textCode.Text = "tyj000001";
            // 
            // lblCode
            // 
            this.lblCode.Location = new System.Drawing.Point(341, 33);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(75, 21);
            this.lblCode.TabIndex = 8;
            this.lblCode.Text = "API编码";
            // 
            // textIp
            // 
            this.textIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textIp.Location = new System.Drawing.Point(117, 22);
            this.textIp.Name = "textIp";
            this.textIp.Size = new System.Drawing.Size(160, 26);
            this.textIp.TabIndex = 7;
            this.textIp.Text = "192.168.0.143";
            // 
            // lblIP
            // 
            this.lblIP.Location = new System.Drawing.Point(32, 33);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(64, 21);
            this.lblIP.TabIndex = 6;
            this.lblIP.Text = "IP地址";
            // 
            // textSmId
            // 
            this.textSmId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textSmId.Location = new System.Drawing.Point(224, 183);
            this.textSmId.Name = "textSmId";
            this.textSmId.Size = new System.Drawing.Size(181, 26);
            this.textSmId.TabIndex = 22;
            this.textSmId.Text = "10";
            // 
            // lblSMID
            // 
            this.lblSMID.Location = new System.Drawing.Point(117, 194);
            this.lblSMID.Name = "lblSMID";
            this.lblSMID.Size = new System.Drawing.Size(75, 22);
            this.lblSMID.TabIndex = 21;
            this.lblSMID.Text = "短信smID";
            // 
            // textContext
            // 
            this.textContext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textContext.Location = new System.Drawing.Point(224, 123);
            this.textContext.Multiline = true;
            this.textContext.Name = "textContext";
            this.textContext.Size = new System.Drawing.Size(395, 44);
            this.textContext.TabIndex = 20;
            this.textContext.Text = "使用NET2003,2005 API发送短信！";
            // 
            // lblContent
            // 
            this.lblContent.Location = new System.Drawing.Point(117, 134);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(86, 22);
            this.lblContent.TabIndex = 19;
            this.lblContent.Text = "短信内容";
            this.lblContent.Click += new System.EventHandler(this.lblContent_Click);
            // 
            // textMobile
            // 
            this.textMobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textMobile.Location = new System.Drawing.Point(224, 65);
            this.textMobile.Multiline = true;
            this.textMobile.Name = "textMobile";
            this.textMobile.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textMobile.Size = new System.Drawing.Size(395, 44);
            this.textMobile.TabIndex = 18;
            this.textMobile.Text = "13516870834";
            // 
            // lblMobile
            // 
            this.lblMobile.Location = new System.Drawing.Point(96, 76);
            this.lblMobile.Name = "lblMobile";
            this.lblMobile.Size = new System.Drawing.Size(107, 22);
            this.lblMobile.TabIndex = 17;
            this.lblMobile.Text = "目的手机号码";
            // 
            // lblSmType
            // 
            this.lblSmType.Location = new System.Drawing.Point(117, 341);
            this.lblSmType.Name = "lblSmType";
            this.lblSmType.Size = new System.Drawing.Size(96, 21);
            this.lblSmType.TabIndex = 34;
            this.lblSmType.Text = "短信类型";
            // 
            // rbnNormal
            // 
            this.rbnNormal.Checked = true;
            this.rbnNormal.Location = new System.Drawing.Point(224, 315);
            this.rbnNormal.Name = "rbnNormal";
            this.rbnNormal.Size = new System.Drawing.Size(139, 32);
            this.rbnNormal.TabIndex = 35;
            this.rbnNormal.TabStop = true;
            this.rbnNormal.Text = "一般短信";
            this.rbnNormal.CheckedChanged += new System.EventHandler(this.rbnNormal_CheckedChanged);
            // 
            // rbnWap
            // 
            this.rbnWap.Location = new System.Drawing.Point(384, 315);
            this.rbnWap.Name = "rbnWap";
            this.rbnWap.Size = new System.Drawing.Size(139, 32);
            this.rbnWap.TabIndex = 36;
            this.rbnWap.Text = "Wap Push短信";
            this.rbnWap.CheckedChanged += new System.EventHandler(this.rbnWap_CheckedChanged);
            // 
            // lblUrl
            // 
            this.lblUrl.Location = new System.Drawing.Point(43, 408);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(149, 22);
            this.lblUrl.TabIndex = 37;
            this.lblUrl.Text = "Wap Push短信的Url";
            // 
            // lblphoneID
            // 
            this.lblphoneID.Location = new System.Drawing.Point(21, 282);
            this.lblphoneID.Name = "lblphoneID";
            this.lblphoneID.Size = new System.Drawing.Size(171, 22);
            this.lblphoneID.TabIndex = 38;
            this.lblphoneID.Text = "手机上显示尾号srcID";
            // 
            // txtSrcID
            // 
            this.txtSrcID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSrcID.Location = new System.Drawing.Point(224, 271);
            this.txtSrcID.Name = "txtSrcID";
            this.txtSrcID.Size = new System.Drawing.Size(181, 26);
            this.txtSrcID.TabIndex = 39;
            this.txtSrcID.Text = "10";
            // 
            // labelTime
            // 
            this.labelTime.Location = new System.Drawing.Point(117, 239);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(96, 22);
            this.labelTime.TabIndex = 40;
            this.labelTime.Text = "发送时间";
            // 
            // txtSMTime
            // 
            this.txtSMTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMTime.Location = new System.Drawing.Point(224, 228);
            this.txtSMTime.Name = "txtSMTime";
            this.txtSMTime.Size = new System.Drawing.Size(181, 26);
            this.txtSMTime.TabIndex = 31;
            // 
            // lbSMTime
            // 
            this.lbSMTime.Location = new System.Drawing.Point(416, 231);
            this.lbSMTime.Name = "lbSMTime";
            this.lbSMTime.Size = new System.Drawing.Size(352, 21);
            this.lbSMTime.TabIndex = 41;
            this.lbSMTime.Text = "*yyyy-MM-dd hh:mm:ss(不写,默认为即时发送)";
            this.lbSMTime.Click += new System.EventHandler(this.lbSMTime_Click);
            // 
            // lbDemo
            // 
            this.lbDemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDemo.Location = new System.Drawing.Point(320, 11);
            this.lbDemo.Name = "lbDemo";
            this.lbDemo.Size = new System.Drawing.Size(117, 32);
            this.lbDemo.TabIndex = 42;
            this.lbDemo.Text = "NetDemo";
            // 
            // rbnSrcPdu
            // 
            this.rbnSrcPdu.Location = new System.Drawing.Point(223, 354);
            this.rbnSrcPdu.Name = "rbnSrcPdu";
            this.rbnSrcPdu.Size = new System.Drawing.Size(502, 33);
            this.rbnSrcPdu.TabIndex = 35;
            this.rbnSrcPdu.Text = "PDU短信(发送时只要填写数据库连接信息,其它参数在后台模拟)";
            this.rbnSrcPdu.CheckedChanged += new System.EventHandler(this.rbnSrcPdu_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(416, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 22);
            this.label2.TabIndex = 43;
            this.label2.Text = "该字段可为空";
            // 
            // FrmDemo
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
            this.ClientSize = new System.Drawing.Size(758, 721);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbDemo);
            this.Controls.Add(this.lbSMTime);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.txtSrcID);
            this.Controls.Add(this.textUrl);
            this.Controls.Add(this.textSmId);
            this.Controls.Add(this.textContext);
            this.Controls.Add(this.textMobile);
            this.Controls.Add(this.txtSMTime);
            this.Controls.Add(this.lblphoneID);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.rbnWap);
            this.Controls.Add(this.rbnNormal);
            this.Controls.Add(this.rbnSrcPdu);
            this.Controls.Add(this.lblSmType);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnRecvRpt);
            this.Controls.Add(this.btnRecvSM);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnRelease);
            this.Controls.Add(this.btnInit);
            this.Controls.Add(this.dbConGroup);
            this.Controls.Add(this.lblSMID);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.lblMobile);
            this.Name = "FrmDemo";
            this.Text = "API演示界面（NET2003,2005)";
            this.Load += new System.EventHandler(this.frmDemo_Load);
            this.dbConGroup.ResumeLayout(false);
            this.dbConGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}


		

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FrmDemo());
		}


		//Click 事件发生。。。


		//======================= Init=========================
		private void btnInit_Click(object sender, System.EventArgs e)
		{

			apiclient=new APIClient();

			int con=apiclient.init(this.textIp.Text.Trim(),this.textUsername.Text.Trim(),this.textPassword.Text.Trim(),this.textCode.Text.Trim(),this.txtDB.Text.Trim());
			con=System.Math.Abs(con);

			MessageBox.Show (initvalues[con], "", 
				MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			

		}
		
		//======================= Release=========================
		private void btnRelease_Click(object sender, System.EventArgs e)
		{
			if(apiclient!=null)
			{
				apiclient.release();	
				apiclient=null;
			}
			MessageBox.Show ("释放成功", "", 
				MessageBoxButtons.OK, MessageBoxIcon.Asterisk);	
		}
       
		
		
		//======================= Send =========================
		private void btnSend_Click(object sender, System.EventArgs e)
		{
			if(apiclient==null)
			{
				MessageBox.Show ("没有初始化", "", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return ;
			}
			String mobileStr = this.textMobile.Text.Trim();
			if(mobileStr=="")
			{
				MessageBox.Show ("没有输入手机号码", "IM", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return ;
			}
			String[] mobiles = mobileStr.Replace("，", ",").Split(',');
			String allMobiles = "";
			for(int i=0 ; i<mobiles.Length ; i++)
			{
				String tmp = mobiles[i];
				if( tmp.IndexOf("-") > 0)
				{
					long min = System.Convert.ToInt64(tmp.Substring(0, tmp.IndexOf("-")));
					long max = System.Convert.ToInt64 (tmp.Substring (tmp.IndexOf("-")+1));

					long j = min;
					while(j <= max)
					{ 
						allMobiles += j + ",";
						//list.Add(new String(System.Convert.ToString (j).ToCharArray()));
						j ++;
					}

				}
				else
				{
					allMobiles += tmp + ",";
				}
			}

			long smID = 0;
			try
			{
				smID = System.Convert.ToInt64(this.textSmId.Text);
			}
			catch(Exception)
			{
				MessageBox.Show ("smID非法", "IM", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return ;
			}
			long srcID = 0;
			try
			{
				if(this.txtSrcID.Text != null && !this.txtSrcID.Text.Equals(""))
				srcID = System.Convert.ToInt64(this.txtSrcID.Text);
			}
			catch(Exception)
			{
				MessageBox.Show ("srcID非法", "IM", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return ;
			}
			 String sendTime =this.txtSMTime.Text.Trim();	
			int sm = 1;
			if(rbnNormal.Checked == true)
			{
				sm=apiclient.sendSM(allMobiles.Split(','),this.textContext.Text,sendTime, smID,srcID);
				
			}
			else if(this.rbnWap.Checked == true)
			{
				sm= apiclient.sendSM(allMobiles.Split(','),this.textContext.Text, smID,srcID, this.textUrl.Text,sendTime);
				
			}
			else if(this.rbnSrcPdu.Checked == true)
			{
				String str = "使用NET API发送PDU短信！";
				byte[] content = System.Text.Encoding.Default.GetBytes( str);
				String[] tmpMobiles = new String[]{"13516870834"};
				long smid = 0;
				long srcid = 0;
				int msgFmt = 0;
				int tpPid = 0;
				int tpUdhi = 0;
				
				String feeTerminalID = "0";
				String feeType = "0";
				String feeCode  = "0" ;
				int feeUserType = 0;

			if(this.txtSrcID.Text == null || this.txtSrcID.Text.Equals(""))
					sm= apiclient.sendPDU(tmpMobiles, content, smid, msgFmt, tpPid, tpUdhi, feeTerminalID, feeType, feeCode, feeUserType);//不带srcID的PDU短信
				else
					sm= apiclient.sendPDU(tmpMobiles, content, smid, srcid, msgFmt, tpPid, tpUdhi, feeTerminalID, feeType, feeCode, feeUserType);//带srcID的PDU短信
			}
			//MessageBox.Show (allMobiles.Split(',') +","+ this.textContext.Text + ","+  smID +","+ srcID +","+ sendTime);

			sm=System.Math.Abs(sm);
			
			if(sm==0)
			{
				String ret = "";// writeLog(mobileStr, this.textContext.Text, smID,srcID, this.textUrl.Text,sendTime, this.textCode.Text);
				MessageBox.Show ("发送成功\n" + ret, "IM", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				MessageBox.Show (retunvalues[sm], "IM", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		
	
		//======================== RecvSM ===========================
		private void btnRecvSM_Click(object sender, System.EventArgs e)
		{
			//long srcID;
			//srcID = System.Convert.ToInt64(this.txtSrcID.Text);
			
			if(apiclient==null)
			{
				MessageBox.Show ("没有初始化", "IM", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return ;
			}
			MOItem[] mo=apiclient.receiveSM();
			//没有回复
			if(mo == null || mo.Length==0)
			{
				MessageBox.Show ("没有短信可接收", "IM", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		　　//有回复
			else
			{
				String info="回复条数："+mo.Length+" 条\n";
				for(int i=0;i<mo.Length;i++)
				{
					info=info+"ID："+mo[i].getSmID()+"\t手机号码："+mo[i].getMobile()+" \t内容："+mo[i].getContent()+"\tMO时间："+mo[i].getMoTime()+"\n";
				}
				//记录Mo日志
				String ret = "";//writeMoLog(mo);
				MessageBox.Show (info + ret, "IM", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);	
			}
		
		}

		private String writeMoLog(MOItem[] mos)
		{
			String ret = null;
    	
			String fileName = "mo.log";
			
			StreamWriter writer = null;

			if( !File.Exists(fileName) )
			{
				writer = File.CreateText(fileName);
			}
			else
			{
				writer = File.AppendText (fileName);
			}

			DateTime dt = DateTime.Now;

			for(int i=0; i<mos.Length; i++)
			{
				String tmp = dt.ToString("yyyy-MM-dd HH:mm:ss");
				tmp += ", smsId:" + mos[i].getSmID() + ", mobile:" 
					+ mos[i].getMobile() + ", content:" 
					+ mos[i].getContent() + ",moTime:"
					+ mos[i].getMoTime();

				writer.WriteLine (tmp);
			}
			writer.Close();
			
			ret = "记录MO日志成功！";		 
		
			return ret;
		}

		private String writeRptLog(RPTItem[] rpts)
		{
			String ret = null;
    	
			String fileName = "rpt.log";
			
			StreamWriter writer = null;

			if( !File.Exists(fileName) )
			{
				writer = File.CreateText(fileName);
			}
			else
			{
				writer = File.AppendText (fileName);
			}

			DateTime dt = DateTime.Now;

			for(int i=0; i<rpts.Length ; i++)
			{
				String tmp = dt.ToString("yyyy-MM-dd HH:mm:ss");
				tmp += ", smsId:" + rpts[i].getSmID() + ", mobile:" 
					+ rpts[i].getMobile() + ", rpt_code:" 
					+ rpts[i].getCode() + ", content:" 
					+ rpts[i].getDesc() + ", rpt_time:" 
					+ rpts[i].getRptTime();

				writer.WriteLine (tmp);
			}
			writer.Close();
			
			ret = "记录回执日志成功！";		 
		
			return ret;
		}


		//============================= RecvRpt ============================
		private void btnRecvRpt_Click(object sender, System.EventArgs e)
		{
			//long smID;
			//smID = System.Convert.ToInt64(this.textSmId.Text);

			if(apiclient==null)
			{
				MessageBox.Show ("没有初始化", "IM", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return ;
			}
			RPTItem[] retRpt=apiclient.receiveRPT();
			//没有回执
			if(retRpt == null || retRpt.Length==0)
			{
				MessageBox.Show ("没有回执", "IM", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
				//有回执
			else
			{
				String info="回执条数："+retRpt.Length+" 条\n";
				for(int i=0;i<retRpt.Length;i++)
				{
					info=info+"ID："+retRpt[i].getSmID()+"\t手机号码："+retRpt[i].getMobile()+"\t内容："+retRpt[i].getDesc()+"\t回执时间："+retRpt[i].getRptTime()+"\n";
				}
				//记录回执日志
				String ret = "";//writeRptLog(retRpt);
				MessageBox.Show (info + ret, "IM", 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);	
			}
		}
		
		private void lblContent_Click(object sender, System.EventArgs e)
		{
	
		}
		//======================= Exit ========================
		private void btnExit_Click(object sender, System.EventArgs e)
		{
			if(apiclient!=null)
			{
				apiclient.release();	
				apiclient=null;
			}
			Application.ExitThread();
		}

		private void frmDemo_Load(object sender, System.EventArgs e)
		{
			DateTime dt = DateTime.Now;
			this.txtSMTime.Text=dt.ToString("yyyy-MM-dd HH:mm:ss");
		}
		private void rbnNormal_CheckedChanged(object sender, System.EventArgs e)
		{
			textUrl.Enabled = false;
			if(this.rbnNormal.Checked == true)
			{
				this.rbnWap.Checked = false;
				this.rbnSrcPdu.Checked = false;
			}
		}

		private void rbnWap_CheckedChanged(object sender, System.EventArgs e)
		{
			textUrl.Enabled = true;
			if(this.rbnWap.Checked == true)
			{
				this.rbnNormal.Checked = false;
				this.rbnSrcPdu.Checked = false;
			}
		}
		private void rbnSrcPdu_CheckedChanged(object sender, System.EventArgs e)
		{
			textUrl.Enabled = false;
			if(this.rbnSrcPdu.Checked == true)
			{
				this.rbnWap.Checked = false;
				this.rbnNormal.Checked = false;
			}
		}

		private void lbSMTime_Click(object sender, System.EventArgs e)
		{
		
		}
		

		
	}
}
